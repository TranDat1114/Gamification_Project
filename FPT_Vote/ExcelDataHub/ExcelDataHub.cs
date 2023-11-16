using FPT_Vote.Models;
using Microsoft.AspNetCore.SignalR;

namespace FPT_Vote.ExcelDataHub;

public class ExcelDataHub : Hub<ITableMessageClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).ReceiveMessage("Connected");
        await base.OnConnectedAsync();
    }

}
public interface ITableMessageClient
{
    Task ReceiveMessage(string message);
    Task SendMessage(List<ExcelData> excelData);
}