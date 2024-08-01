using System.Net;
using Common;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;
using FluentAssertions;
using Generics.Enums;
using Generics.Pagination;
using Xunit;


namespace CodeBase.Tests
{
    public class CodeBaseServiceTests(CodeBaseServiceWebApplicationFactory<Program> factory)
        : IClassFixture<CodeBaseServiceWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly Guid _commonId = Guid.Parse("123e4567-e89b-12d3-a456-426614174008");
        private readonly Guid _idToUpdate = Guid.Parse("123e4567-e89b-12d3-a456-426614174011");
        private readonly Guid _idToDelete = Guid.Parse("123e4567-e89b-12d3-a456-426614174007");


        #region Add CodeBase Tests

        [Fact]
        public async Task AddCodeBase_Success_ShouldReturnSuccess()
        {
            // Arrange
            var expectedCodeBaseUserId = _commonId;
            var codeBase = new CreateCodeBaseRequest
            {
                UserId = expectedCodeBaseUserId,
                Name = "My new codebase",
                SupportedPlatform = SupportedPlatformType.Csharp,
                Valid = false
            };
            var content = DeserializationHelper.CreateJsonContent(codeBase);

            // Act
            var response = await _client.PostAsync("/code-base/add", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var createCodeBaseResponse =
                await DeserializationHelper.DeserializeResponse<CreateCodeBaseResponse>(response);
            createCodeBaseResponse.UserId.Should().Be(expectedCodeBaseUserId);
        }

        [Fact]
        public async Task AddCodeBaseWithInvalidData_Fail_ShouldReturnBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            var codeBase = new { };
            var content = DeserializationHelper.CreateJsonContent(codeBase);

            // Act
            var response = await _client.PostAsync("/code-base/add", content);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Get CodeBase Tests

        [Fact]
        public async Task GetCodeBases_Success_ShouldReturnCodeBases()
        {
            // Arrange
            var requestContent = new ReadCodeBaseListRequest
            {
                PaginationQuery = new PaginationQuery
                {
                    PageNumber = 1,
                    PageSize = 10
                }
            };
            var content = DeserializationHelper.CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/code-base/all", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeBaseListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetCodeBasesNoPaginationQuery_Success_ShouldReturnCodeBases()
        {
            // Arrange
            var requestContent = new ReadCodeBaseListRequest();
            var content = DeserializationHelper.CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/code-base/all", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeBaseListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetCodeBasesByUserId_Success_ShouldReturnCodeBases()
        {
            // Arrange
            var userId = MockData.MockId;

            // Act
            var response = await _client.GetAsync($"/code-base/by-user-id/{userId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent =
                await DeserializationHelper.DeserializeResponse<ReadCodeBaseListByUserIdResponse>(response);
            responseContent.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCodeBasesByUserId_Fail_ShouldReturnResponseWithNullValues()
        {
            // Arrange
            var randomUserId = new Guid("5886293d-1569-4e50-881b-853abb880229");

            // Act
            var response = await _client.GetAsync($"/code-base/by-user-id/{randomUserId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent =
                await DeserializationHelper.DeserializeResponse<ReadCodeBaseListByUserIdResponse>(response);
            responseContent.CodeBaseListResponseItems.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCodeBaseById_Success_ShouldReturnCodeBase()
        {
            // Arrange
            var existingId = _commonId;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

            // Act
            var response = await _client.GetAsync($"/code-base/{existingId}");

            // Assert
            var codeBaseResponse = await DeserializationHelper.DeserializeResponse<ReadCodeBaseResponse>(response);
            response.StatusCode.Should().Be(expectedStatusCode);

            codeBaseResponse.Id.Should().Be(existingId);
        }

        [Fact]
        public async Task GetCodeBaseById_Fail_ShouldReturnDbEntityNotFound()
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
        public async Task UpdateCodeBase_Success_ShouldReturnSuccess()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var codeBaseToUpdateId = _idToUpdate;
            var codeBaseToUpdate = new UpdateCodeBaseRequest
            {
                Name = "Updated Name",
                SupportedPlatform = SupportedPlatformType.Csharp,
                Valid = true,
                Code = "Hello updated world"
            };
            var content = DeserializationHelper.CreateJsonContent(codeBaseToUpdate);

            // Act
            var response = await _client.PatchAsync($"/code-base/{codeBaseToUpdateId}", content);

            // Assert
            var updatedCodeBase = await DeserializationHelper.DeserializeResponse<UpdateCodeBaseResponse>(response);
            response.StatusCode.Should().Be(expectedStatusCode);

            updatedCodeBase.Id.Should().Be(codeBaseToUpdateId);
        }

        #endregion

        #region Delete CodeBase Tests

        [Fact]
        public async Task DeleteCodeBase_Success_ShouldReturnEmptyContent()
        {
            // Arrange
            var codeBaseToDeleteId = _idToDelete;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;

            // Act
            var response = await _client.DeleteAsync($"/code-base/{codeBaseToDeleteId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteCodeBase_Fail_ShouldReturnDbEntityNotFound()
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
    }
}