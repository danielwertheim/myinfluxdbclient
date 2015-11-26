using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace InfluxDbClient.Extensions
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable ForAwait(this Task t)
        {
            return t.ConfigureAwait(false);
        }

        public static ConfiguredTaskAwaitable<T> ForAwait<T>(this Task<T> t)
        {
            return t.ConfigureAwait(false);
        }
    }
}