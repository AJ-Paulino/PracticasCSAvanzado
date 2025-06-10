using System.Collections.Concurrent;
using System.Reactive.Subjects;

namespace PracticasCSAvanzado
{
    public class ManejadorTareasSecuencial
    {
        private readonly Subject<Func<Task>> _tareasQueue = new();
        private readonly ConcurrentQueue<Func<Task>> _queue = new();
        private bool _isProcessing = false;

        public ManejadorTareasSecuencial()
        {
            _tareasQueue.Subscribe(async tarea =>
            {
                _queue.Enqueue(tarea);
                await ProcesarQueue();
            });
        }

        public void AgregarTarea(Func<Task> tarea)
        {
            _tareasQueue.OnNext(tarea);
        }

        private async Task ProcesarQueue()
        {
            if (_isProcessing)
                return;
            _isProcessing = true;
            while (_queue.TryDequeue(out var tarea))
            {
                try
                {
                    await tarea();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar la tarea: {ex.Message}");
                }
            }
            _isProcessing = false;
        }
    }
}
