using System.Net;
using Xunit;
using FluentAssertions;
using Common;
using ExternalDomainEntities.CodeRunDto.Command;
using ExternalDomainEntities.CodeRunDto.Query;
using Generics.Enums;
using Generics.Pagination;

namespace CodeRunService.Tests
{
    public class CodeRunServiceTests(CodeRunServiceWebApplicationFactory<Program> factory)
        : IClassFixture<CodeRunServiceWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly Guid _commonId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");


        #region Add CodeRun Tests

        [Fact]
        public async Task AddCodeRun_Success_ShouldReturnSuccess()
        {
            // Arrange
            var expectedCodeBaseId = _commonId;
            var codeRun = new CreateCodeRunRequest
            {
                CodeBaseId = expectedCodeBaseId
            };
            var content = DeserializationHelper.CreateJsonContent(codeRun);

            // Act
            var response = await _client.PostAsync("/code-run/add", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain(expectedCodeBaseId.ToString());
        }

        [Fact]
        public async Task AddCodeRunWithInvalidData_Fail_ShouldReturnBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            var codeRun = new { };
            var content = DeserializationHelper.CreateJsonContent(codeRun);

            // Act
            var response = await _client.PostAsync("/code-run/add", content);

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
            var content = DeserializationHelper.CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/code-run", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeRunListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task GetCodeRunsNoPaginationQuery_Success_ShouldReturnCodeRuns()
        {
            // Arrange
            var requestContent = new ReadCodeRunListRequest();
            var content = DeserializationHelper.CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/code-run", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeRunListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetCodeRunById_Success_ShouldReturnCodeRun()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var codeRunToSearch = _commonId;

            // Act
            var response = await _client.GetAsync($"/code-run/{codeRunToSearch}");

            // Assert
            var codeRunResponse = await DeserializationHelper.DeserializeResponse<ReadCodeRunResponse>(response);
            response.StatusCode.Should().Be(expectedStatusCode);

            codeRunResponse.Id.Should().Be(codeRunToSearch);
        }

        [Fact]
        public async Task GetCodeRunById_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.GetAsync($"/code-run/{nonExistingId}");

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
            var codeRunToUpdateId = MockData.MockIdUpdate;
            var expectedStatus = RunStatus.Done;
            var codeRunToUpdate = new UpdateCodeRunRequest
            {
                Status = expectedStatus
            };
            var content = DeserializationHelper.CreateJsonContent(codeRunToUpdate);

            // Act
            var response = await _client.PatchAsync($"/code-run/{codeRunToUpdateId}", content);

            // Assert
            var updatedCodeRun = await DeserializationHelper.DeserializeResponse<UpdateCodeRunResponse>(response);
            response.StatusCode.Should().Be(expectedStatusCode);

            updatedCodeRun.Id.Should().Be(codeRunToUpdateId);
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
            var response = await _client.DeleteAsync($"/code-run/{codeRunToDeleteId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteCodeRun_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.DeleteAsync($"/code-run/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion
    }
}