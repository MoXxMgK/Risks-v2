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
    /// Логика взаимодействия для PlantsProductionPage.xaml
    /// </summary>
    public partial class PlantsProductionPage : Page
    {
        private readonly PlantsProductionViewModel _vm;
        public PlantsProductionPage()
        {
            InitializeComponent();

            _vm = new PlantsProductionViewModel(this.Dispatcher);

            _vm.OnDataDisplayReady += _vm_OnDataDisplayReady;

            DataContext = _vm;
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
