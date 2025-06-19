using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PracticasCSAvanzado.ClientesConectados
{
    public class CCAplicacion1
    {
        private readonly HubConnection _hubConnection;

        public CCAplicacion1(string url)
        {
            //string url = "https://localhost:7009/recibirNotificacion";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("recibirNotificacion", (message) =>
            {
                Console.WriteLine($"Mensaje recibido: {message}");
            });                        
        }

        public async Task ConectarAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("Conexión establecida con el hub.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al conectar con el hub: {e.Message}");
            }
            Console.ReadLine();
        }
    }
}
