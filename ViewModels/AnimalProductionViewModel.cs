using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Risks_v2.Models;
using Risks_v2.Helpers;
using Risks_v2.SoilData;
using System.Windows.Threading;

namespace Risks_v2.ViewModels
{
    internal class AnimalProductionViewModel : StandartExcelDataViewModel
    {
        // Сено
        private AgricultureBase _selectedSeno = new Dummy(-1, "");
        public AgricultureBase SelectedSeno
        {
            get => _selectedSeno;
            set
            {
                SetField(ref _selectedSeno, value);
                foreach (AgricultureBase c in SenoItems)
                    c.Checked = false;

                value.Checked = true;
            }
        }

        public ObservableCollection<AgricultureBase> SenoItems { get; set; } = new ObservableCollection<AgricultureBase>();

        // Солома
        private AgricultureBase _selectedSoloma = new Dummy(-1, "");
        public AgricultureBase SelectedSoloma
        {
            get => _selectedSoloma;
            set
            {
                SetField(ref _selectedSoloma, value);
                foreach (AgricultureBase c in SolomaItems)
                    c.Checked = false;

                value.Checked = true;
            }
        }

        public ObservableCollection<AgricultureBase> SolomaItems { get; set; } = new ObservableCollection<AgricultureBase>();

        // Сенаж
        private AgricultureBase _selectedSenage = new Dummy(-1, "");
        public AgricultureBase SelectedSenage
        {
            get => _selectedSenage;
            set
            {
                SetField(ref _selectedSenage, value);
                foreach (AgricultureBase c in SenageItems)
                    c.Checked = false;

                value.Checked = true;
            }
        }

        public ObservableCollection<AgricultureBase> SenageItems { get; set; } = new ObservableCollection<AgricultureBase>();

        // Силос
        private AgricultureBase _selectedSilos = new Dummy(-1, "");
        public AgricultureBase SelectedSilos
        {
            get => _selectedSilos;
            set
            {
                SetField(ref _selectedSilos, value);
                foreach (AgricultureBase c in SilosItems)
                    c.Checked = false;

                value.Checked = true;
            }
        }

        public ObservableCollection<AgricultureBase> SilosItems { get; set; } = new ObservableCollection<AgricultureBase>();

        // Зернофураж
        private AgricultureBase _selectedSeeds = new Dummy(-1, "");
        public AgricultureBase SelectedSeeds
        {
            get => _selectedSeeds;
            set
            {
                SetField(ref _selectedSeeds, value);
                foreach (AgricultureBase c in SeedsItems)
                    c.Checked = false;

                value.Checked = true;
            }
        }

        public ObservableCollection<AgricultureBase> SeedsItems { get; set; } = new ObservableCollection<AgricultureBase>();

        // Корнеплоды
        private AgricultureBase _selectedRoots = new Dummy(-1, "");
        public AgricultureBase SelectedRoots
        {
            get => _selectedRoots;
            set
            {
                SetField(ref _selectedRoots, value);
                foreach (AgricultureBase c in RootsItems)
                    c.Checked = false;

                value.Checked = true;
            }
        }

        public ObservableCollection<AgricultureBase> RootsItems { get; set; } = new ObservableCollection<AgricultureBase>();

        // All
        private List<AgricultureBase> _allCultures = new List<AgricultureBase>();

        // Results 

        public ObservableCollection<string> Results { get; set; } = new ObservableCollection<string>();

        public Normative AnimalsNormative { get; set; }

        public RadiationLevel MilkLevel { get; set; } = new RadiationLevel(100, 3.7);
        public RadiationLevel MeatLevel { get; set; } = new RadiationLevel(500);

        private int _correctionYear;
        public int CorrectionYear
        {
            get => _correctionYear;
            set
            {
                int year = DateTime.Now.Year;
                if (value > year)
                {
                    _correctionYear = year;
                }
                else
                {
                    _correctionYear = value;
                }
            }
        }
        public Command CalculateCommand { get; set; }

        private readonly Dispatcher _dispatcher;

        public AnimalProductionViewModel(Dispatcher dispatcher)
        {
            Title = "Животноводство";

            _dispatcher = dispatcher;

            AnimalsNormative = new Normative();

            _correctionYear = DateTime.Now.Year;

            CalculateCommand = new Command(CalculateResult);

            AddProducts();
        }

        private void AddProducts()
        {
            Seno s1 = new Seno(220, "Многолетние злаковые");
            Seno s2 = new Seno(221, "Многолетние бобово-злаковые");
            SenoItems.Add(s1);
            SenoItems.Add(s2);

            _allCultures.Add(s1);
            _allCultures.Add(s2);

            Soloma sl1 = new Soloma(230, "Овес");
            Soloma sl2 = new Soloma(232, "Ячмень");

            SolomaItems.Add(sl1);
            SolomaItems.Add(sl2);

            _allCultures.Add(sl1);
            _allCultures.Add(sl2);

            Senage sn1 = new Senage(240, "Многолетние злаковые");
            Senage sn2 = new Senage(241, "Многолетние бобово-злаковые");

            SenageItems.Add(sn1);
            SenageItems.Add(sn2);

            _allCultures.Add(sn1);
            _allCultures.Add(sn2);

            Silos sls = new Silos(250, "Кукуруза");

            SilosItems.Add(sls);

            _allCultures.Add(sls);

            Seeds sd1 = new Seeds(100, "Овес");
            Seeds sd2 = new Seeds(101, "Озимая рожь");
            Seeds sd3 = new Seeds(103, "Ячмень");
            Seeds sd4 = new Seeds(104, "Горох");

            SeedsItems.Add(sd1);
            SeedsItems.Add(sd2);
            SeedsItems.Add(sd3);
            SeedsItems.Add(sd4);

            _allCultures.Add(sd1);
            _allCultures.Add(sd2);
            _allCultures.Add(sd3);
            _allCultures.Add(sd4);

            Roots r1 = new Roots(110, "Картофель");
            Roots r2 = new Roots(112, "Свекла");

            RootsItems.Add(r1);
            RootsItems.Add(r2);

            _allCultures.Add(r1);
            _allCultures.Add(r2);
        }

        private async void CalculateResult(object? parameter)
        {
            if (_allCultures.All(p => !p.Checked))
            {
                MessageBox.Show("Выберите не менее одного продукта", "Упс");
                return;
            }

            if (!AnimalsNormative.CsNormative && !AnimalsNormative.SrNormative)
            {
                MessageBox.Show("Выберите хотя бы один норматив", "Упс");
                return;
            }

            if (!CalculationAvailable)
                return; // For save purpose

            await Task.Run(Calculate);
        }

        private async Task Calculate()
        {
            IEnumerable<ExcelDataRow> data = (await LoadExcelData()).ToList();

            IEnumerable<AgricultureBase> selected = _allCultures.Where(c => c.Checked);

            List<AnimalsResult> results = new List<AnimalsResult>();

            foreach (AgricultureBase cult in selected)
            {
                List<AgricultureBase> cultData = new List<AgricultureBase>();

                foreach (ExcelDataRow row in data)
                {
                    Tuple<double, double> kp = KP.Instance.Get(cult.Id, row.SoilType, row.PH, row.Potassium);

                    int currYear = DateTime.Now.Year;

                    double actCs = row.Cs * Math.Exp((-0.693 * (currYear - _correctionYear)) / 30.16);
                    double actSr = row.Sr * Math.Exp((-0.693 * (currYear - _correctionYear)) / 29.12);

                    double qCs = Utils.Formula2(kp.Item1, actCs);
                    double qSr = Utils.Formula2(kp.Item2, actSr);

                    AgricultureBase? copy = Utils.GetCopy(cult);

                    if (copy == null)
                        continue;

                    copy.CsActivity = qCs;
                    copy.SrActivity = qSr;
                    cult.Quantity = cult.Quantity;

                    Type cType = copy.GetType();

                    if ((cType == typeof(Soloma) || cType == typeof(Roots) || cType == typeof(Seeds)) && row.SoilType.Item1 == SoilType.Peat)
                    {
                        copy.CsActivity = -999;
                        copy.SrActivity = -999;
                    }

                    cultData.Add(copy);
                }

                double csAvg = cultData.Where(c => c.CsActivity > 0).Average(c => c.CsActivity * cult.Quantity);
                double srAvg = cultData.Where(c => c.SrActivity > 0).Average(c => c.SrActivity * cult.Quantity);

                results.Add(new AnimalsResult(csAvg, 0, 0, srAvg, 0, 0));
            }

            double csSum = results.Sum(c => c.CsAvg);
            double srSum = results.Sum(c => c.SrAvg);

            double milkCs = csSum * 0.01;
            double milkSr = srSum * 0.01 * 0.14;
            double beef = csSum * 0.04;

            double MRCS = Utils.Formula(MilkLevel.CsLevel, milkCs);
            double MRSR = Utils.Formula(MilkLevel.SrLevel, milkSr);
            double BR = Utils.Formula(MeatLevel.CsLevel, beef);

            string patters = "Прогнозное значение - {0} Бк/кг, Риск - {1}%";

            _dispatcher.Invoke(() =>
            {
                Results.Clear();
                Results.Add(String.Format("Молоко Cs: " + patters, Math.Round(milkCs, 2), Math.Round(MRCS, 2)));
                Results.Add(String.Format("Молоко Sr: " + patters, Math.Round(milkSr, 2), Math.Round(MRSR, 2)));
                Results.Add(String.Format("Мясо Cs: " + patters, Math.Round(beef, 2), Math.Round(BR, 2)));
            });
        }
    }
}
