using Microsoft.AspNetCore.SignalR;

namespace CodeRunService;

public class CodeRunHub : Hub
{
    public async Task SendStatusUpdate(string runId, string status)
    {
        await Clients.All.SendAsync("ReceiveStatusUpdate", runId, status);
    }
}