using ClientApp.Services;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;
using ExternalDomainEntities.CodeRunDto.Command;
using ExternalDomainEntities.CodeRunDto.Query;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClientApp.Pages.Features.CodeBases;

public partial class CodeBaseDetail
{
    [Parameter] public required string CodeBaseId { get; set; }

    private ReadCodeRunListByCodeBaseIdResponse? _codeRuns;
    private int _currentPage = 1;
    private const int PageSize = 10;
    private ReadCodeBaseResponse? _codeBase;
    private bool _isRunning;
    private bool _isSaving;

    // Injected services
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] private CodeFormatService CodeFormatService { get; set; } = default!;
    [Inject] private CodeBaseService CodeBaseService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private CodeRunService CodeRunService { get; set; } = default!;

    #region Initialization

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        await LoadCodeBase();
        await LoadCodeRuns();
    }

    #endregion

    #region Data Loading Methods

    private async Task LoadCodeBase()
    {
        _codeBase = await CodeBaseService.GetEntityAsync(CodeBaseId);
        StateHasChanged();
    }

    private async Task LoadCodeRuns()
    {
        _codeRuns = await CodeRunService.GetDataAsync($"by-code-base-id/{CodeBaseId}", _currentPage, PageSize);
        StateHasChanged();
    }

    #endregion

    #region Event Handlers

    private async Task HandlePageChanged(int pageNumber)
    {
        _currentPage = pageNumber;
        await LoadCodeRuns();
    }

    private async Task HandleCodeChange(string newCode)
    {
        if (_codeBase != null)
        {
            _codeBase.Code = newCode;
            await InvokeAsync(StateHasChanged); // Ensures the component is re-rendered
        }
    }

    private async Task HandleCodeFormat(string codeToFormat)
    {
        if (!string.IsNullOrWhiteSpace(codeToFormat))
        {
            try
            {
                // Format the code using the service
                var formattedCode =
                    await CodeFormatService.FormatCodeAsync(codeToFormat, _codeBase.SupportedPlatform.ToString());

                // Update the editor with the formatted code
                await JsRuntime.InvokeVoidAsync("setMonacoEditorValue", formattedCode);

                // Update the component state
                await HandleCodeChange(formattedCode);
            }
            catch (Exception ex)
            {
                NotificationService.ShowMessage($"Code formatting failed: {ex.Message}");
            }
        }
    }

    private async Task HandleRunChanged()
    {
        if (_codeBase != null && _codeBase.Code != null)
        {
            _isRunning = true;
            try
            {
                var newRun = new CreateCodeRunRequest
                {
                    CodeBaseId = _codeBase.Id,
                    Code = _codeBase.Code
                };

                // Validate and run the code
                await CodeRunService.AddEntityAsync(newRun);
                await LoadCodeRuns();

                await Task.Delay(2000); // Show the "Running..." message for 2 seconds
            }
            catch (Exception ex)
            {
                NotificationService.ShowMessage($"Code running failed: {ex.Message}");
            }
            finally
            {
                _isRunning = false;
            }
        }
    }

    private async Task HandleSaveChanged()
    {
        if (_codeBase != null)
        {
            _isSaving = true;
            try
            {
                var updateRequest = new UpdateCodeBaseRequest
                {
                    Id = _codeBase.Id,
                    Code = _codeBase.Code,
                    SupportedPlatform = _codeBase.SupportedPlatform,
                    UserId = _codeBase.UserId
                    // Map other necessary properties from _codeBase to updateRequest
                };

                await CodeBaseService.UpdateEntityAsync(CodeBaseId, updateRequest);

                await Task.Delay(2000); // Show the "Saved" message for 2 seconds
            }
            finally
            {
                _isSaving = false;
            }
        }
    }

    #endregion
}