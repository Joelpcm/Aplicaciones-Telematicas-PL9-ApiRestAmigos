using Microsoft.AspNetCore.SignalR;

namespace Amigos
{
    public class SignalRNotification : Hub
    {
        // Método para notificar de nuevas ubicaciones a todos los clientes conectados
        public async Task SendToAll()
        {
            await Clients.All.SendAsync("NotifyLocationUpdate");
        }
    }
}
