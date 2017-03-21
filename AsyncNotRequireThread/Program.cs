using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncNotRequireThread
{
    class Program
    {
        public static int counter = 0;

        static void Main()
        {
            ExecuteAll(100_000).Wait();
        }

        private static async Task ExecuteAll(int count)
        {
            var tasks = new Task[count];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Execute();
            }

            await Task.WhenAll(tasks);

            Console.WriteLine($"{counter}");
        }

        public static async Task Execute()
        {
            await Task.Delay(5000);
            Interlocked.Increment(ref counter);
        }
    }
}
