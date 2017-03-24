using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAsync
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModel = new MainViewModel();
        }

        public MainViewModel ViewModel { get; }

        private void MakeItBeautifulOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var result = MakeCall().Result;

                ViewModel.Result = $"{result.Substring(0, 30)} in {stopwatch.ElapsedMilliseconds} ms";
            }
            catch (Exception exception)
            {
                ViewModel.Result = exception.ToString();
            }
        }

        private static async Task<string> MakeCall()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "http://google.com");
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
