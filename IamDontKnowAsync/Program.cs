using System;
using System.Threading;
using System.Threading.Tasks;

namespace IamDontKnowAsync
{
    class Program
    {
        static void Main()
        {
            var makeRequestTask = MakeRequest();
            try
            {
                makeRequestTask.Wait();
            }
            catch { }

            Console.WriteLine(makeRequestTask.Status);
        }

        private static Task MakeRequest()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
            });

            return Task.Factory.StartNew(async () =>
            {
                for (var i = 0; i < 100; i++)
                {
                    await Task.Delay(100);
                    throw new TaskCanceledException();
                }
            }, new CancellationToken())
            .Unwrap();
        }
    }
}
