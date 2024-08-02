using System.Net;
using ExternalDomainEntities.UserDto.Command;
using ExternalDomainEntities.UserDto.Query;
using FluentAssertions;
using Generics.Pagination;
using Helpers;
using Xunit;

namespace UserService.Tests
{
    public class UserServiceTests(UserServiceWebApplicationFactory<Program> factory)
        : IClassFixture<UserServiceWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly Guid _commonId = new Guid("123e4567-e89b-12d3-a456-426614174003");
        private readonly Guid _commonIdToUpdate = new Guid("123e4567-e89b-12d3-a456-426614174004");
        private readonly Guid _commonIdToDelete = new Guid("123e4567-e89b-12d3-a456-426614174005");

        #region Add User Tests

        [Fact]
        public async Task AddUser_ShouldReturnSuccess_WhenUserIsValid()
        {
            // Arrange
            var user = MockData.MockUser;
            var content = DeserializationHelper.CreateJsonContent(user);

            // Act
            var response = await _client.PostAsync("/user/add", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("jdoe");
        }

        [Fact]
        public async Task AddUser_ShouldReturnBadRequest_WhenUserIsInvalid()
        {
            // Arrange
            var invalidUser = new { };
            var content = DeserializationHelper.CreateJsonContent(invalidUser);

            // Act
            var response = await _client.PostAsync("/user/add", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Get User Tests

        [Fact]
        public async Task GetUsers_ShouldReturnUsers_WithPagination()
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

            // Act
            var request = HttpRequestHelper.CreateGetRequestWithBody($"/user/all", requestContent);
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadUserListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
            responseContent.Items.Values.Should().HaveCount(expectedSize);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnUsers_NoPagination()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("user/all");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await DeserializationHelper.DeserializeResponse<ReadUserListResponse>(response);
            responseContent.Items.Values.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userToSearch = _commonId;
            var expectedUser = MockData.ExpectedUser;

            // Act
            var response = await _client.GetAsync($"/user/{userToSearch}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userResponse = await DeserializationHelper.DeserializeResponse<ReadUserResponse>(response);
            userResponse.Id.Should().Be(expectedUser.Id);
            userResponse.Name.Should().Be(expectedUser.Name);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/user/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Update User Tests

        [Fact]
        public async Task UpdateUser_ShouldReturnSuccess_WhenUserIsValid()
        {
            // Arrange
            var userToUpdateId = _commonIdToUpdate;
            var userToUpdate = new UpdateUserRequest
            {
                Email = "updatedemail@example.com",
                Name = "Updated Name",
                Password = "Test pass",
                UserName = "Updated UserName",
                Language = "Updated Language"
            };
            var content = DeserializationHelper.CreateJsonContent(userToUpdate);

            // Act
            var response = await _client.PatchAsync($"/user/{userToUpdateId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedUser = await DeserializationHelper.DeserializeResponse<UpdateUserResponse>(response);
            updatedUser.Id.Should().Be(userToUpdateId);
            updatedUser.Email.Should().Be(userToUpdate.Email);
            updatedUser.Name.Should().Be(userToUpdate.Name);
        }

        #endregion

        #region Delete User Tests

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserIsDeleted()
        {
            // Arrange
            var userToDeleteId = _commonIdToDelete;

            // Act
            var response = await _client.DeleteAsync($"/user/{userToDeleteId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/user/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}