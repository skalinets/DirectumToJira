using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    public static class SyncWrapper
    {
        /// <summary>
        /// Преобразовывает вызов асинхронного делегата в синхронный
        /// </summary>
        /// <param name="action"></param>
        public static void Execute(Func<Task> action)
        {
            action().GetAwaiter().GetResult();
        }
    }
}
