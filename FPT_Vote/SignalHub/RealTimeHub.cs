using Microsoft.AspNetCore.SignalR;

namespace FPT_Vote.SignalHub;

public class RealTimeHub : Hub<IRealTimeClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).ReceiveMessage("Connected");
        await base.OnConnectedAsync();
    }
}

public interface IRealTimeClient
{
    Task ReceiveMessage(string message);
}