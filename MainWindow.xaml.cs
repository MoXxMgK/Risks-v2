using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Risks_v2.Views;
using Risks_v2.ViewModels;

namespace Risks_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static NavigationService NavService => _navService;
        private static NavigationService _navService;

        private readonly MainWindowViemModel _vm;
        public MainWindow()
        {
            InitializeComponent();

            _navService = NavFrame.NavigationService;

            _vm = new();

            DataContext = _vm;

            _navService.Navigate(new SplashPage());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
