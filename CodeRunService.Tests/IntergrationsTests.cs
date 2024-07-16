using System.Net;
using System.Text;
using System.Text.Json;
using DomainEntities;
using DomainEntities.CodeRunDto.Command;
using DomainEntities.CodeRunDto.Query;
using FluentAssertions;
using Generics.Pagination;
using UsersService.Tests;
using Xunit;

namespace CodeRunService.Tests
{
    public class CodeRunServiceTests(CustomWebApplicationFactory<Program> factory)
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly Guid _commonId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");

        #region Add CodeRun Tests

        [Fact]
        public async Task AddCodeRun_Success_ShouldReturnSuccess()
        {
            // Arrange
            var expectedId = new Guid();
            var expectedCodeBaseId = new Guid();

            var codeRun = new CreateCodeRunRequest
            {
                Id = expectedId,
                CodeBaseId = expectedCodeBaseId
            };
            var content = CreateJsonContent(codeRun);

            // Act
            var response = await _client.PostAsync("/code-runs/add", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain(expectedId.ToString());
            responseContent.Should().Contain(expectedCodeBaseId.ToString());
        }

        [Fact]
        public async Task AddCodeRunWithInvalidData_Fail_ShouldReturnBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            var codeRun = new { };
            var content = CreateJsonContent(codeRun);

            // Act
            var response = await _client.PostAsync("/code-runs/add", content);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Get CodeRun Tests

        [Fact]
        public async Task GetCodeRuns_Success_ShouldReturnCodeRuns()
        {
            // Arrange
            var requestContent = new ReadCodeRunListRequest
            {
                PaginationQuery = new PaginationQuery
                {
                    PageNumber = 1,
                    PageSize = 10
                }
            };
            var content = CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/code-runs", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetCodeRunById_Success_ShouldReturnCodeRun()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var codeRunToSearch = _commonId;

            // Act
            var response = await _client.GetAsync($"/code-runs/{codeRunToSearch}");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task GetCodeRunById_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.GetAsync($"/code-runs/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Update CodeRun Tests

        [Fact]
        public async Task UpdateCodeRun_Success_ShouldReturnSuccess()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var codeRunToUpdateId = _commonId;
            var expectedStatus = RunStatus.Done;
            var codeRunToUpdate = new UpdateCodeRunRequest
            {
                Status = expectedStatus
            };
            var content = CreateJsonContent(codeRunToUpdate);

            // Act
            var response = await _client.PatchAsync($"/code-runs/{codeRunToUpdateId}", content);

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain(codeRunToUpdate.ToString());
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Delete CodeRun Tests

        [Fact]
        public async Task DeleteCodeRun_Success_ShouldReturnEmptyContent()
        {
            // Arrange
            var codeRunToDeleteId = _commonId;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;

            // Act
            var response = await _client.DeleteAsync($"/code-runs/{codeRunToDeleteId}");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task DeleteCodeRun_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.DeleteAsync($"/code-runs/{nonExistingId}");

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