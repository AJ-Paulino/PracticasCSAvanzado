namespace PracticasCSAvanzado
{
    public static class MemoCache
    {
        private static readonly Dictionary<string, object> _cache = new();

        public static TResult GetOrAdd<TResult>(string key, Func<TResult> computar)
        {
            if(_cache.TryGetValue(key, out var valor))
            {
                return (TResult)valor;
            }

            var resultado = computar();
            _cache[key] = resultado;
            return resultado;
        }
    }
}
