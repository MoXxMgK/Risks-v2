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

using Risks_v2.ViewModels;

namespace Risks_v2.Views
{
    /// <summary>
    /// Логика взаимодействия для FeedsProductionPage.xaml
    /// </summary>
    public partial class FeedsProductionPage : Page
    {
        private readonly FeedsProductionViewModel _vm;
        public FeedsProductionPage()
        {
            InitializeComponent();

            _vm = new FeedsProductionViewModel(Dispatcher);

            DataContext = _vm;

            _vm.OnDataDisplayReady += _vm_OnDataDisplayReady;
        }

        private void _vm_OnDataDisplayReady(List<Visual> toDisplay)
        {
            DataContainer.Children.Clear();

            foreach (UIElement element in toDisplay)
            {
                DataContainer.Children.Add(element);
            }
        }
    }
}
