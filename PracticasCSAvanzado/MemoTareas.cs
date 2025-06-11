using DomainLayer.Models;

namespace PracticasCSAvanzado
{
    public static class MemoTareas
    {
        public static double CalcularPorcentajeCompletadas(List<Tarea> tareas)
        {
            string key = $"completadas:{tareas.Count}:{string.Join(",", tareas.Select(t => t.Status))}";

            return MemoCache.GetOrAdd(key, () =>
            {
                if (tareas.Count == 0) return 0;
                int completadas = tareas.Count(t => t.Status == "Completada");
                return (double)completadas / tareas.Count * 100;
            });
        }

        public static List<Tarea> FiltrarTareasPorEstado(List<Tarea> tareas, string estado)
        {
            string key = $"estado:{estado}:{tareas.Count}";

            return MemoCache.GetOrAdd(key, () =>
            {
                return tareas.Where(t => t.Status == estado).ToList();
            });
        }
    }
}

