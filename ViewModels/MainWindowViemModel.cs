using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Risks_v2.Views;

namespace Risks_v2.ViewModels
{
    public class MainWindowViemModel : BaseViewModel
    {
        public Command OpenCommand { get; set; }

        public MainWindowViemModel()
        {
            Title = "Risks";
            OpenCommand = new Command(Open);
        }

        private void Open(object? parameter)
        {
            if (parameter == null)
                return;

            Page? p = null;
            bool otherWindow = false;

            switch ((string)parameter)
            {
                case "Plants":
                    p = new PlantsProductionPage();
                    break;
                case "Feeds":
                    p = new FeedsProductionPage();
                    break;
                case "Animals":
                    p = new AnimalsProductionPage();
                    break;
                case "Info":
                    new InfoWindow().Show();
                    otherWindow = true;
                    break;
                default:
                    p = null;
                    break;
            }

            if (p == null || otherWindow)
                return;

            Title = (p.DataContext as BaseViewModel).Title;

            MainWindow.NavService.Navigate(p);
        }
    }
}
