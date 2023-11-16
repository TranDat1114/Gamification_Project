using FPT_Vote.Models;
using Microsoft.AspNetCore.SignalR;

namespace FPT_Vote.ExcelDataHub;

public class ExcelDataHub : Hub<ITableMessageClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.ReceiveMessage("Connected");
        await base.OnConnectedAsync();
    }
    public async Task SendDataTableUpdate(List<ExcelData> datas)
    {
        await Clients.All.SendMessage(datas);
    }
    public async Task EndGame(string message)
    {
        await Clients.All.ReceiveMessage(message);
    }
}
public interface ITableMessageClient
{
    Task ReceiveMessage(string message);
    Task SendMessage(List<ExcelData> excelData);
}