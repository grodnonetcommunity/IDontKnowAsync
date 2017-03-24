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
        }

        private async void MakeItBeautifulOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var result = await MakeCall();

                ResultTextBox.Text = $"{result.Substring(0, 30)} in {stopwatch.ElapsedMilliseconds} ms";
            }
            catch (Exception exception)
            {
                ResultTextBox.Text = exception.ToString();
            }
        }

        private static async Task<string> MakeCall()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "http://google.com");
            var response = await client.SendAsync(request).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
