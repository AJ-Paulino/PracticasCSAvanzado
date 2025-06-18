using Microsoft.AspNetCore.SignalR;

namespace PracticasCSAvanzado.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task EnviarNotificacion(string mensaje)
            => await Clients.All.SendAsync("RecibirNotificacion", mensaje);
    }
}
