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
            Task.WaitAny(
                makeRequestTask.ContinueWith(t => Console.WriteLine("Completed"),
                    TaskContinuationOptions.OnlyOnRanToCompletion),
                makeRequestTask.ContinueWith(t => Console.WriteLine("Faulted"),
                    TaskContinuationOptions.OnlyOnFaulted),
                makeRequestTask.ContinueWith(t => Console.WriteLine("Canceled"),
                    TaskContinuationOptions.OnlyOnCanceled)
            );

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
