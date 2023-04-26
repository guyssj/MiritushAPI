using Microsoft.AspNetCore.SignalR;


namespace Miritush.Socket.Hubs
{
    public class MiritushHubs : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}