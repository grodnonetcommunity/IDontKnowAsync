using System;
using System.Diagnostics;
using System.Net.Http;
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

        private async void MakeItBeautifulOnClick(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            try
            {
                var stopwatch = Stopwatch.StartNew();

                var request = new HttpRequestMessage(HttpMethod.Get, "http://google.com");
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                ViewModel.Result = $"{result.Substring(0, 30)} in {stopwatch.ElapsedMilliseconds} ms";
            }
            catch (Exception exception)
            {
                ViewModel.Result = exception.ToString();
            }
        }
    }
}
