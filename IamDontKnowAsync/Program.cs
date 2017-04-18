using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace IamDontKnowAsync
{
    class Program
    {
        private static ExceptionDispatchInfo _dispatchInfo;

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

            Console.WriteLine("There are no exceptions");

            if (makeRequestTask.IsCanceled)
            {
                _dispatchInfo.Throw();
            }

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
                try
                {
                    for (var i = 0; i < 100; i++)
                    {
                        await Task.Delay(100);

                        throw new TaskCanceledException();
                    }
                }
                catch (Exception e)
                {
                    _dispatchInfo = ExceptionDispatchInfo.Capture(e);
                    throw;
                }
            }, new CancellationToken())
            .Unwrap();
        }
    }
}
