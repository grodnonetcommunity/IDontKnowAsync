using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IamDontKnowAsync
{
    class Program
    {
        private static readonly Random Rnd = new Random();

        static void Main()
        {
            MakeRequest().Wait();

            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();

            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();

            Console.ReadLine();
        }

        public static async Task MakeRequest()
        {
            var cancellationTokenSource = new CancellationTokenSource(3000);
            var completion = new TaskCompletionSource<(string server, bool recommended)>();

            var urls = new[] {"http://google.com", "http://microsoft.com", "http://thomson-reuters.com"};

            var cancellationToken = cancellationTokenSource.Token;
            var recomendations = urls
                .Select(url => GetBuyRecomendation(url, cancellationToken)
                    .ContinueWith(task => ProcessResult(task, completion, cancellationTokenSource)))
                .ToList();

            #pragma warning disable 4014
            Task.WhenAll(recomendations).ContinueWith(r => completion.TrySetResult((null, false)));
            #pragma warning restore 4014

            var (server, recommended) = await completion.Task;

            Console.WriteLine($"{server} = {recommended}");
        }

        private static void ProcessResult(Task<(string server, bool recomended)> obj, TaskCompletionSource<(string server, bool recommended)> completion, CancellationTokenSource cancellationTokenSource)
        {
            if (obj.IsCanceled)
            {
                Console.WriteLine("Canceled");
            }
            else if (obj.IsFaulted)
            {
                Console.WriteLine(obj.Exception?.InnerException?.Message ?? "Faulted");
            }
            else
            {
                completion.TrySetResult(obj.Result);
                cancellationTokenSource.Cancel();
            }
        }

        public static async Task<(string server, bool recomended)> GetBuyRecomendation(string server, CancellationToken token)
        {
            await Task.Delay(Rnd.Next(100, 1000), token);

            return server == "http://google.com"
                ? throw new Exception($"Web exception in {server}")
                : (server: server, recomended: true);
        }
    }
}
