using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var s1 = GetBuyRecomendation("http://google.com");
            var s2 = GetBuyRecomendation("http://microsoft.com");
            var s3 = GetBuyRecomendation("http://thomson-reuters.com");

            var recomendations = new List<Task<(string, bool)>> {s1, s2, s3};

            var stopwatch = Stopwatch.StartNew();
            string server = null;
            bool recommended = false;
            while (recomendations.Count > 0)
            {
                var recomendation = await Task.WhenAny(recomendations);
                try
                {
                    (server, recommended) = await recomendation;
                    recomendations.Remove(recomendation);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    recomendations.Remove(recomendation);
                }
            }
            var elapsed = stopwatch.Elapsed;

            foreach (var recomendation in recomendations)
            {
                recomendation.ContinueWith(e => Console.WriteLine(e.Exception.InnerException.Message),
                    TaskContinuationOptions.OnlyOnFaulted);
            }

            Console.WriteLine($"{server} = {recommended} in {elapsed.TotalMilliseconds:F0}");
        }

        public static async Task<(string server, bool recomended)> GetBuyRecomendation(string server)
        {
            await Task.Delay(Rnd.Next(100, 1000));

            if (server == "http://google.com")
            {
                Console.WriteLine("Damn...");
                throw new Exception($"Web exception in {server}");
            }

            return (server: server, recomended: true);
        }
    }
}
