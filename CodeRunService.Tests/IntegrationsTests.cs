using System.Net;
using Xunit;
using FluentAssertions;
using ExternalDomainEntities.CodeRunDto.Command;
using ExternalDomainEntities.CodeRunDto.Query;
using Generics.Enums;
using Generics.Pagination;
using Helpers;

namespace CodeRunService.Tests;

public class CodeRunServiceTests(CodeRunServiceWebApplicationFactory<Program> factory)
    : IClassFixture<CodeRunServiceWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly Guid _commonId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001");
    private readonly Guid _commonIdToUpdate = Guid.Parse("123e4567-e89b-12d3-a456-426614174002");
    private readonly Guid _commonIdToDelete = Guid.Parse("123e4567-e89b-12d3-a456-426614174003");

    #region Add CodeRun Tests

    [Fact]
    public async Task AddCodeRun_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var expectedCodeBaseId = _commonId;
        var codeRun = new CreateCodeRunRequest
        {
            CodeBaseId = expectedCodeBaseId,
            Code = "Hellow world",
        };
        var content = DeserializationHelper.CreateJsonContent(codeRun);

        // Act
        var response = await _client.PostAsync("/code-run/add", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseObject = await DeserializationHelper.DeserializeResponse<CreateCodeRunResponse>(response);
        responseObject.CodeBaseId.Should().Be(expectedCodeBaseId);
    }

    [Fact]
    public async Task AddCodeRun_ShouldReturnBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var codeRun = new { };
        var content = DeserializationHelper.CreateJsonContent(codeRun);

        // Act
        var response = await _client.PostAsync("/code-run/add", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Get CodeRun Tests

    [Fact]
    public async Task GetCodeRuns_ShouldReturnCodeRuns_WithPagination()
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
        var response = await _client.PostAsync($"/code-run/all", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeRunListResponse>(response);
        responseContent.Items.Values.Should().NotBeEmpty();
        responseContent.Items.Values.Should().HaveCount(expectedSize);
    }

    [Fact]
    public async Task GetCodeRuns_ShouldReturnCodeRuns_SuccessNoPagination()
    {
        // Arrange

        var content = DeserializationHelper.CreateJsonContent(new { });

        // Act
        var response = await _client.PostAsync($"/code-run/all", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await DeserializationHelper.DeserializeResponse<ReadCodeRunListResponse>(response);
        responseContent.Items.Values.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetCodeRunsByCodeBaseId_ShouldReturnCodeRuns_WhenCodeBaseIdIsValid()
    {
        // Arrange
        var codeBaseId = MockData.MockId;

        // Act
        var content = DeserializationHelper.CreateJsonContent(new { });
        var response = await _client.PostAsync($"/code-run/by-code-base-id/{codeBaseId}", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent =
            await DeserializationHelper.DeserializeResponse<ReadCodeRunListByCodeBaseIdResponse>(response);
        responseContent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCodeRunsByCodeBaseId_ShouldReturnEmpty_WhenCodeBaseIdIsInvalid()
    {
        // Arrange
        var codeBaseId = new Guid("133e4567-e39b-1233-a456-423314174009");

        // Act
        var content = DeserializationHelper.CreateJsonContent(new { });
        var response = await _client.PostAsync($"/code-run/by-code-base-id/{codeBaseId}", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent =
            await DeserializationHelper.DeserializeResponse<ReadCodeRunListByCodeBaseIdResponse>(response);
        responseContent.Items.Values.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCodeRunById_ShouldReturnCodeRun_WhenCodeRunExists()
    {
        // Arrange
        var codeRunToSearch = _commonId;

        // Act
        var response = await _client.GetAsync($"/code-run/{codeRunToSearch}");

        // Assert
        var codeRunResponse = await DeserializationHelper.DeserializeResponse<ReadCodeRunResponse>(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        codeRunResponse.Id.Should().Be(codeRunToSearch);
    }

    [Fact]
    public async Task GetCodeRunById_ShouldReturnNotFound_WhenCodeRunDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/code-run/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Update CodeRun Tests

    [Fact]
    public async Task UpdateCodeRun_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedCodeRun.Id.Should().Be(codeRunToUpdateId);
    }

    #endregion

    #region Delete CodeRun Tests

    [Fact]
    public async Task DeleteCodeRun_ShouldReturnNoContent_WhenCodeRunIsDeleted()
    {
        // Arrange
        var codeRunToDeleteId = _commonIdToDelete;

        // Act
        var response = await _client.DeleteAsync($"/code-run/{codeRunToDeleteId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteCodeRun_ShouldReturnNotFound_WhenCodeRunDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/code-run/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}