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
            var cancelationTokenSource = new CancellationTokenSource();
            var cancelationToken = cancelationTokenSource.Token;

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                cancelationTokenSource.Cancel();
            });

            return Task.Factory.StartNew(async () =>
            {
                for (var i = 0; i < 100; i++)
                {
                    await Task.Delay(100);
                    cancelationToken.ThrowIfCancellationRequested();
                }
            }, new CancellationToken())
            .Unwrap();
        }
    }
}
