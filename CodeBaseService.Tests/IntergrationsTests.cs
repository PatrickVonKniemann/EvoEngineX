using System.Net;
using System.Net.Http.Headers;
using Common;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;
using FluentAssertions;
using Generics.Enums;
using Generics.Pagination;
using Xunit;

namespace CodeBase.Tests
{
    public class CodeBaseServiceTests : IClassFixture<CodeBaseServiceWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Guid _commonId = Guid.Parse("123e4567-e89b-12d3-a456-426614174008");
        private readonly Guid _idToUpdate = Guid.Parse("123e4567-e89b-12d3-a456-426614174011");
        private readonly Guid _idToDelete = Guid.Parse("123e4567-e89b-12d3-a456-426614174007");

        public CodeBaseServiceTests(CodeBaseServiceWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

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
            var requestContent = new ReadCodeBaseListRequest
            {
                PaginationQuery = new PaginationQuery
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
            responseContent.Items.Values.Should().HaveCount(expectedSize);
        }

        [Fact]
        public async Task GetCodeBases_ShouldReturnCodeBases_WhenNoPaginationQueryProvided()
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
        public async Task GetCodeBasesByUserId_ShouldReturnCodeBases_WhenNoPaginationQueryProvided()
        {
            // Arrange
            var userId = _commonId;

            // Act
            var response = await _client.GetAsync($"/code-base/by-user-id/{userId}");

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
            int expectedSize = 3;
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
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"/code-base/by-user-id/{userId}", UriKind.Relative),
                Content = content
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.SendAsync(request);

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
            var response = await _client.GetAsync($"/code-base/by-user-id/{randomUserId}");

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