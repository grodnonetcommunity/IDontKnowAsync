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

            return Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                    cancelationToken.ThrowIfCancellationRequested();
                }
            }, cancelationToken);
        }
    }
}
