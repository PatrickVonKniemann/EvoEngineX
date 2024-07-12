using System.Text;
using System.Text.Json;
using DomainEntities.Users.Query;
using FluentAssertions;
using Generics.Pagination;
using Xunit;

namespace UsersService.Tests
{
    public class UserServiceTests(CustomWebApplicationFactory<Program> factory)
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        // private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5127") };
        private readonly Guid _commonId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");
        private readonly HttpClient _client = factory.CreateClient();
        private readonly CustomWebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task GetUsers_ShouldReturnUsers()
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
            var serializeRequest = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/users", serializeRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateUser_ShouldReturnSuccess()
        {
            // Arrange
            var user = new
            {
                UserName = "jdoe",
                Email = "jdoe@example.com",
                Name = "John Doe",
                Language = "English"
            };
            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/users/add", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("jdoe");
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/users/{_commonId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnSuccess()
        {
            // Arrange
            var userToUpdateId = _commonId;
            var userToUpdate = new
            {
                Email = "updatedemail@example.com",
                Name = "Updated Name",
                Language = "Updated Language"
            };
            var content = new StringContent(JsonSerializer.Serialize(userToUpdate), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PatchAsync($"/users/{userToUpdateId}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("updatedemail@example.com");
        }
    }
}