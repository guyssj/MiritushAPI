using Microsoft.AspNetCore.SignalR;
using Miritush.Socket.Models;

namespace Miritush.Socket.Hubs
{
    public interface IMiritushHubs
    {
        Task BookChange(string payload);
    }
    public class MiritushHubs : Hub<IMiritushHubs>
    {
        public async Task RegisterToBookState(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"BOOK_STATE");
        }
        public async Task BookStateChange(string payload)
        {
            await Clients.Group("BOOK_STATE").BookChange(payload);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "BOOK_STATE");
            await base.OnConnectedAsync();
        }
        // public async Task SendMessage(string user, string message)
        // {
        //     await Clients.All.("ReceiveMessage", user, message);
        // }


    }
}