namespace ClientApp.Services;

using Microsoft.AspNetCore.SignalR.Client;

public class CodeRunStatusConnectorService(ServiceUrls serviceUrls)
{
    private HubConnection _hubConnection;

    public event Action<string, string> OnRunStatusUpdated;

    // Inject ServiceUrls through the constructor

    public async Task StartConnectionAsync()
    {
        // Build the HubConnection using the URL from ServiceUrls
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{serviceUrls.CodeRunServiceUrl}/codeRunHub")
            .Build();

        _hubConnection.On<string, string>("ReceiveStatusUpdate",
            (runId, status) => { OnRunStatusUpdated?.Invoke(runId, status); });

        await _hubConnection.StartAsync();
    }

    public async Task StopConnectionAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }
}