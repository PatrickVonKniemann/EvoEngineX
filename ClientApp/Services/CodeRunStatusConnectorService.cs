namespace ClientApp.Services;
using Microsoft.AspNetCore.SignalR.Client;

public class CodeRunStatusConnectorService
{
    private HubConnection _hubConnection;

    public event Action<string, string> OnRunStatusUpdated;

    public async Task StartConnectionAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5001/codeRunHub")
            .Build();

        _hubConnection.On<string, string>("ReceiveStatusUpdate",
            (runId, status) => { OnRunStatusUpdated?.Invoke(runId, status); });

        await _hubConnection.StartAsync();
    }

    public async Task StopConnectionAsync()
    {
        await _hubConnection.StopAsync();
        await _hubConnection.DisposeAsync();
    }
}