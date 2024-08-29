using System.Net;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;
using FluentAssertions;
using Generics.Enums;
using Generics.Pagination;
using Helpers;
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
        public async Task AddCodeBase_ShouldReturnSuccess_WhenDataIsValid()
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
        public async Task AddCodeBase_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var codeBase = new { };
            var content = DeserializationHelper.CreateJsonContent(codeBase);

            // Act
            var response = await _client.PostAsync("/code-base/add", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Get CodeBase Tests

        [Fact]
        public async Task GetCodeBases_ShouldReturnCodeBases_WhenCalled()
        {
            // Arrange
            int expectedSize = 10;
            var requestContent = new
            {
                paginationQuery = new PaginationQuery
                {
                    PageNumber = 1,
                    PageSize = expectedSize
                }
            };

            var content = DeserializationHelper.CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/code-base/all", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeBaseListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
            responseContent.Items.Values.Should().HaveCountGreaterThan(0).And.HaveCountLessThan(expectedSize);
        }

        [Fact]
        public async Task GetCodeBases_ShouldReturnCodeBases_WhenNoPaginationQueryProvided()
        {
            // Arrange
            var requestContent = new ReadCodeBaseListRequest();

            // Act
            var content = DeserializationHelper.CreateJsonContent(requestContent);

            var response = await _client.PostAsync($"/code-base/all", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeBaseListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetCodeBasesByUserId_ShouldReturnCodeBases_WhenNoPaginationQueryProvided()
        {
            // Arrange
            var userId = _commonId;

            // Act
            var content = DeserializationHelper.CreateJsonContent(new { });
            var response = await _client.PostAsync($"/code-base/by-user-id/{userId}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent =
                await DeserializationHelper.DeserializeResponse<ReadCodeBaseListByUserIdResponse>(response);
            responseContent.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCodeBasesByUserId_ShouldReturnCodeBases_WithPagination()
        {
            // Arrange
            var userId = _commonId;
            int expectedSize = 2;
            var requestContent = new
            {
                paginationQuery = new PaginationQuery
                {
                    PageNumber = 1,
                    PageSize = expectedSize
                }
            };

            // Act
            var content = DeserializationHelper.CreateJsonContent(requestContent);
            var response = await _client.PostAsync($"/code-base/by-user-id/{userId}", content);


            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent =
                await DeserializationHelper.DeserializeResponse<ReadCodeBaseListByUserIdResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
            responseContent.Items.Values.Should().HaveCount(expectedSize);
        }


        [Fact]
        public async Task GetCodeBasesByUserId_ShouldReturnEmpty_WhenUserIdIsInvalid()
        {
            // Arrange
            var randomUserId = new Guid("5886293d-1569-4e50-881b-853abb880229");

            // Act
            var content = DeserializationHelper.CreateJsonContent(new { });
            var response = await _client.PostAsync($"/code-base/by-user-id/{randomUserId}", content);


            // Assert
            response.EnsureSuccessStatusCode();


            var responseContent =
                await DeserializationHelper.DeserializeResponse<ReadCodeBaseListByUserIdResponse>(response);
            responseContent.Items.Values.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCodeBaseById_ShouldReturnCodeBase_WhenCodeBaseExists()
        {
            // Arrange
            var existingId = _commonId;

            // Act
            var response = await _client.GetAsync($"/code-base/{existingId}");

            // Assert
            var codeBaseResponse = await DeserializationHelper.DeserializeResponse<ReadCodeBaseResponse>(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            codeBaseResponse.Id.Should().Be(existingId);
        }

        [Fact]
        public async Task GetCodeBaseById_ShouldReturnNotFound_WhenCodeBaseDoesNotExist()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/code-base/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Update CodeBase Tests

        [Fact]
        public async Task UpdateCodeBase_ShouldReturnSuccess_WhenDataIsValid()
        {
            // Arrange
            var codeBaseToUpdateId = "123e4567-e89b-12d3-a456-426614174006";
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
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedCodeBase.Id.Should().Be(codeBaseToUpdateId);
        }

        #endregion

        #region Delete CodeBase Tests

        [Fact]
        public async Task DeleteCodeBase_ShouldReturnNoContent_WhenCodeBaseIsDeleted()
        {
            // Arrange
            var codeBaseToDeleteId = _idToDelete;

            // Act
            var response = await _client.DeleteAsync($"/code-base/{codeBaseToDeleteId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteCodeBase_ShouldReturnNotFound_WhenCodeBaseDoesNotExist()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/code-base/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}