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
    /// Логика взаимодействия для AnimalsProductionPage.xaml
    /// </summary>
    public partial class AnimalsProductionPage : Page
    {
        private readonly AnimalProductionViewModel _vm;
        public AnimalsProductionPage()
        {
            InitializeComponent();

            _vm = new AnimalProductionViewModel(Dispatcher);

            DataContext = _vm;
        }
    }
}
