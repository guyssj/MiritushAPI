using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Miritush.Socket.Hubs;
using Miritush.Socket.Models;

namespace Miritush.Socket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<MiritushHubs, IMiritushHubs> hubContext;

        public NotificationsController(IHubContext<MiritushHubs, IMiritushHubs> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] PayloadNotification payload, CancellationToken cancelToken)
        {
            try
            {
                var messagePayLoad = JsonSerializer.Serialize(payload);
                await hubContext.Clients.All.BookChange(messagePayLoad);
                return Ok();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

    }
}