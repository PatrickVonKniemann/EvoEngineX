using System.Net;
using System.Text;
using System.Text.Json;
using DomainEntities;
using DomainEntities.CodeBaseDto.Query;
using ExternalDomainEntities.CodeBaseDto.Command;
using FluentAssertions;
using Generics.Enums;
using Generics.Pagination;
using UsersService.Tests;
using Xunit;

namespace Codebase.Tests
{
    public class CodebaseServiceTests(CustomWebApplicationFactory<Program> factory)
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly Guid _commonId = Guid.Parse("222e4567-e89b-12d3-a456-426614174000");

        #region Add CodeBase Tests

        [Fact]
        public async Task AddCodebase_Success_ShouldReturnSuccess()
        {
            // Arrange
            var expectedId = new Guid();
            var expectedCodebaseId = new Guid();

            var codeBase = new CreateCodeBaseRequest
            {
            };
            var content = CreateJsonContent(codeBase);

            // Act
            var response = await _client.PostAsync("/code-base/add", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain(expectedId.ToString());
            responseContent.Should().Contain(expectedCodebaseId.ToString());
        }

        #endregion

        #region Get CodeBase Tests

        [Fact]
        public async Task GetCodebases_Success_ShouldReturnCodebases()
        {
            // Arrange
            var requestContent = new ReadCodebaseListRequest
            {
                PaginationQuery = new PaginationQuery
                {
                    PageNumber = 1,
                    PageSize = 10
                }
            };
            var content = CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/code-base", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetCodebaseById_Success_ShouldReturnCodebase()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var codeBaseToSearch = _commonId;

            // Act
            var response = await _client.GetAsync($"/code-base/{codeBaseToSearch}");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task GetCodebaseById_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.GetAsync($"/code-base/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Update CodeBase Tests

        [Fact]
        public async Task UpdateCodebase_Success_ShouldReturnSuccess()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var codeBaseToUpdateId = _commonId;
            var expectedStatus = RunStatus.Done;
            var codeBaseToUpdate = new UpdateCodeBaseRequest
            {
                Status = expectedStatus
            };
            var content = new StringContent(JsonSerializer.Serialize(codeBaseToUpdate), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PatchAsync($"/code-base/{codeBaseToUpdateId}", content);

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain(JsonSerializer.Serialize(codeBaseToUpdateId));
            responseContent.Should().Contain(JsonSerializer.Serialize(expectedStatus));
            response.StatusCode.Should().Be(expectedStatusCode);
        }


        #endregion

        #region Delete CodeBase Tests

        [Fact]
        public async Task DeleteCodebase_Success_ShouldReturnEmptyContent()
        {
            // Arrange
            var codeBaseToDeleteId = _commonId;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;

            // Act
            var response = await _client.DeleteAsync($"/code-base/{codeBaseToDeleteId}");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task DeleteCodebase_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.DeleteAsync($"/code-base/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Helper Methods

        private static StringContent CreateJsonContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        #endregion
    }
}