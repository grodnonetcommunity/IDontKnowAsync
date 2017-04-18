using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IamDontKnowAsync
{
    class Program
    {
        static void Main()
        {
            MakeRequest().Wait();

            Console.ReadLine();
        }

        public static async Task MakeRequest()
        {
        }
    }
}
