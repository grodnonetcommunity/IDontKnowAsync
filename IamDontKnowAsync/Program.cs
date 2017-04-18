using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IamDontKnowAsync
{
    class Program
    {
        private static readonly Random Rnd = new Random();

        static void Main()
        {
            MakeRequest();

            Console.ReadLine();
        }

        public static async void MakeRequest()
        {
            var s1 = GetBuyRecomendation("http://google.com");
            var s2 = GetBuyRecomendation("http://microsoft.com");
            var s3 = GetBuyRecomendation("http://thomson-reuters.com");

            var stopwatch = Stopwatch.StartNew();
            var (server, recommended) = await await Task.WhenAny(s1, s2, s3);
            var elapsed = stopwatch.Elapsed;

            Console.WriteLine($"{server} = {recommended} in {elapsed.TotalMilliseconds:F0}");
        }

        public static async Task<(string server, bool recomended)> GetBuyRecomendation(string server)
        {
            await Task.Delay(Rnd.Next(100, 1000));

            return (server: server, recomended: true);
        }
    }
}
