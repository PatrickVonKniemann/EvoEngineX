using System.Net;
using ExternalDomainEntities;
using FastEndpoints;
using Generics.Enums;

namespace CoreEngineService;

public class RunCodeEndpoint : Endpoint<RunCodeRequest, bool>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/run-code");
        AllowAnonymous();
    }

    public override Task HandleAsync(RunCodeRequest runCodeRequest, CancellationToken ct)
    {
        ExecuteCodeAsync(runCodeRequest.Code);
        return SendOkAsync(true, ct);
    }


    private void ExecuteCodeAsync(string code)
    {
        // Execute the code...
        bool executionSuccess = true; // Example execution result

        if (executionSuccess)
        {
            PublishStatusUpdate(RunStatus.Done);
            Task.FromResult(RunStatus.Done);
        }
        else
        {
            PublishStatusUpdate(RunStatus.ErrorRunning);
            Task.FromResult(RunStatus.ErrorRunning);
        }
    }

    private void PublishStatusUpdate(RunStatus status)
    {
        // Similar RabbitMQ publishing logic as in CodeRunService
    }
}