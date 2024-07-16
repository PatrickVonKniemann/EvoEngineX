using System.Net;
using System.Text;
using System.Text.Json;
using DomainEntities.UserDtos.Query;
using FluentAssertions;
using Generics.Pagination;
using UsersService.Tests;
using Xunit;

namespace UserService.Tests
{
    public class UserServiceTests(CustomWebApplicationFactory<Program> factory)
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly Guid _commonId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");

        #region Add User Tests

        [Fact]
        public async Task AddUser_Success_ShouldReturnSuccess()
        {
            // Arrange
            var user = new
            {
                UserName = "jdoe",
                Email = "jdoe@example.com",
                Name = "John Doe",
                Language = "English"
            };
            var content = CreateJsonContent(user);

            // Act
            var response = await _client.PostAsync("/users/add", content);

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
            var user = new { };
            var content = CreateJsonContent(user);

            // Act
            var response = await _client.PostAsync("/users/add", content);

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
                PaginationQuery = new PaginationQuery
                {
                    PageNumber = 1,
                    PageSize = 10,
                    FilterParams = new Dictionary<string, string>
                    {
                        { "Language", "Spanish" }
                    }
                }
            };
            var content = CreateJsonContent(requestContent);

            // Act
            var response = await _client.PostAsync("/users", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetUserById_Success_ShouldReturnUser()
        {
            // Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var userToSearch = _commonId;

            // Act
            var response = await _client.GetAsync($"/users/{userToSearch}");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task GetUserById_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.GetAsync($"/users/{nonExistingId}");

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
            var userToUpdateId = _commonId;
            var userToUpdate = new
            {
                Email = "updatedemail@example.com",
                Name = "Updated Name",
                Language = "Updated Language"
            };
            var content = CreateJsonContent(userToUpdate);

            // Act
            var response = await _client.PatchAsync($"/users/{userToUpdateId}", content);

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("updatedemail@example.com");
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        #endregion

        #region Delete User Tests

        [Fact]
        public async Task DeleteUser_Success_ShouldReturnEmptyContent()
        {
            // Arrange
            var userToDeleteId = _commonId;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;

            // Act
            var response = await _client.DeleteAsync($"/users/{userToDeleteId}");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeEmpty();
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task DeleteUser_Fail_ShouldReturnDbEntityNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.DeleteAsync($"/users/{nonExistingId}");

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
