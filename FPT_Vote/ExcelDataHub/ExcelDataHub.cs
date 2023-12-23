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
    public async Task SendDataTableUpdate(IndividuallyNGroup datas)
    {
        await Clients.All.SendMessage(datas);
    }
    // public async Task SendDataTableSort(List<ExcelData> datas)
    // {
    //     await Clients.All.SendMessage(datas, "Sorted");
    // }
    public async Task EndGame(string message)
    {
        await Clients.All.ReceiveMessage(message);
    }
    public async Task Connected()
    {
        await Clients.All.ReceiveMessage("Connected new client");
    }
}
public interface ITableMessageClient
{
    Task ReceiveMessage(string message);
    Task SendMessage(IndividuallyNGroup excelData, string message = "Imported");
}