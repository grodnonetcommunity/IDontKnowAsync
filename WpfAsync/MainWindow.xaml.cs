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
            ViewModel.Result = "Asyn is Beautiful";
        }
    }
}
