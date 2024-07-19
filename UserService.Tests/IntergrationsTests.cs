using System.Net;
using System.Text;
using System.Text.Json;
using Common;
using DomainEntities.UserDto.Command;
using DomainEntities.UserDto.Query;
using ExternalDomainEntities.UserDto.Query;
using FluentAssertions;
using Xunit;

namespace UserService.Tests
{
    public class UserServiceTests(UserServiceWebApplicationFactory<Program> factory)
        : IClassFixture<UserServiceWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();


        #region Add User Tests

        [Fact]
        public async Task AddUser_Success_ShouldReturnSuccess()
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
        public async Task AddUserWithInvalidData_Fail_ShouldReturnBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            var invalidUser = new { };
            var content = DeserializationHelper.CreateJsonContent(invalidUser);

            // Act
            var response = await _client.PostAsync("/user/add", content);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Get User Tests

        [Fact]
        public async Task GetUsers_Success_ShouldReturnUsers()
        {
            // Arrange
            var requestContent = new ReadUserListRequest
            {
                PaginationQuery = MockData.MockPaginationQuery
            };
            var content = DeserializationHelper.CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/user/all", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var userListResponse = await DeserializationHelper.DeserializeResponse<ReadUserListResponse>(response);
            userListResponse.Items.Values.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUserById_Success_ShouldReturnUser()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var userToSearch = MockData.MockId;
            var expectedUser = MockData.ExpectedUser;
            // Act
            var response = await _client.GetAsync($"/user/{userToSearch}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);

            var userResponse = await DeserializationHelper.DeserializeResponse<ReadUserResponse>(response);
            userResponse.Id.Should().Be(expectedUser.Id);
            userResponse.Name.Should().Be(expectedUser.Name);
        }

        [Fact]
        public async Task GetUserById_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.GetAsync($"/user/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Update User Tests

        [Fact]
        public async Task UpdateUser_Success_ShouldReturnSuccess()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var userToUpdateId = MockData.MockIdUpdate;
            var userToUpdate = new
            {
                Email = "updatedemail@example.com",
                Name = "Updated Name",
                Language = "Updated Language"
            };
            var content = DeserializationHelper.CreateJsonContent(userToUpdate);

            // Act
            var response = await _client.PatchAsync($"/user/{userToUpdateId}", content);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);

            var updatedUser = await DeserializationHelper.DeserializeResponse<UpdateUserResponse>(response);

            updatedUser.Id.Should().Be(userToUpdateId);
            updatedUser.Email.Should().Be(userToUpdate.Email);
            updatedUser.Name.Should().Be(userToUpdate.Name);
        }

        #endregion

        #region Delete User Tests

        [Fact]
        public async Task DeleteUser_Success_ShouldReturnEmptyContent()
        {
            // Arrange
            var userToDeleteId = MockData.MockId;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;

            // Act
            var response = await _client.DeleteAsync($"/user/{userToDeleteId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteUser_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.DeleteAsync($"/user/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion
        
    }
}