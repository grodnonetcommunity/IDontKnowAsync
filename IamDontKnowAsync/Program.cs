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

            Console.ReadLine();
        }

        public static async Task MakeRequest()
        {
            var s1 = GetBuyRecomendation("http://google.com");
            var s2 = GetBuyRecomendation("http://microsoft.com");
            var s3 = GetBuyRecomendation("http://thomson-reuters.com");

            var recomendations = new List<Task<(string server, bool recommended)>> { s1, s2, s3 };

            var stopwatch = Stopwatch.StartNew();
            while (recomendations.Count > 0)
            {
                var recommendation = await Task.WhenAny(recomendations);
                try
                {
                    var result = await recommendation;
                    Console.WriteLine($"{result.server} = {result.recommended} after {stopwatch.ElapsedMilliseconds} ms");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message} after {stopwatch.ElapsedMilliseconds} ms");
                }

                recomendations.Remove(recommendation);
            }
        }

        public static async Task<(string server, bool recommended)> GetBuyRecomendation(string server, CancellationToken token = default(CancellationToken))
        {
            await Task.Delay(Rnd.Next(100, 1000), token);

            if (server == "http://google.com")
            {
                Console.WriteLine("Damn...");
                throw new Exception($"Web exception in {server}");
            }

            return (server: server, recommended: true);
        }
    }
}
