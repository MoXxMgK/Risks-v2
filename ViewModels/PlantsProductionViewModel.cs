using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Forms.Integration;

using Risks_v2.Models;
using Risks_v2.SoilData;
using Risks_v2.Views;
using Risks_v2.Helpers;
using System.Windows.Threading;
using System.Windows;

namespace Risks_v2.ViewModels
{
    public class PlantsProductionViewModel : StandartExcelDataViewModel
    {
        public ObservableCollection<AgricultureBase> Products { get; set; } = new ObservableCollection<AgricultureBase>();

        public Normative PlantsNormative { get; set; }
        public RadiationLevel SeedsLevel { get; set; }
        public RadiationLevel PotatoesLevel { get; set; }
        public RadiationLevel VegetablesLevel { get; set; }

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

        public event Action<List<System.Windows.Media.Visual>>? OnDataDisplayReady;

        private readonly Dispatcher _dispatcher;

        public PlantsProductionViewModel(Dispatcher dispatcher)
        {
            Title = "Растениеводство";

            _dispatcher = dispatcher;

            PlantsNormative = new Normative();
            
            // Load from file or set default here
            SeedsLevel = new RadiationLevel(90, 11);
            PotatoesLevel = new RadiationLevel(80, 40);
            VegetablesLevel = new RadiationLevel(100, 40);

            _correctionYear = DateTime.Now.Year;

            CalculateCommand = new Command(CalculateResult);

            AddProducts();
        }

        private void AddProducts()
        {
            Dummy allDummy = new Dummy(-1, "All");

            allDummy.OnCheckedChanged += result =>
            {
                foreach (var c in Products.Where(p => p.GetType() != typeof(Dummy)))
                {
                    c.Checked = result;
                }
            };

            Products.Add(allDummy);
            Products.Add(new Seeds(100, "Зерно овса"));
            Products.Add(new Seeds(101, "Зерно озимой ржи"));
            Products.Add(new Seeds(103, "Зерно ячменя"));
            Products.Add(new Seeds(105, "Зерно гороха"));
            Products.Add(new Roots(110, "Картофель"));
            Products.Add(new Roots(111, "Морковь"));
            Products.Add(new Roots(112, "Свекла"));
            Products.Add(new Roots(113, "Капуста"));
            Products.Add(new Roots(114, "Лук репчатый"));
        }

        private async void CalculateResult(object? parameter)
        {
            if (Products.All(p => !p.Checked))
            {
                MessageBox.Show("Выберите не менее одного продукта", "Упс");
                return;
            }

            if (!PlantsNormative.CsNormative && !PlantsNormative.SrNormative)
            {
                MessageBox.Show("Выберите хотя бы один норматив", "Упс");
                return;
            }

            await Task.Run(Calculate);
        }

        private async Task Calculate()
        {
            if (!CalculationAvailable)
                return; // For save purpose

            List<ExcelDataRow> data = (await LoadExcelData()).ToList();

            Dictionary<string, List<ExcelDataRow>> orgData = Utils.SeparateByOrgs(data);

            IEnumerable<AgricultureBase> selected = Products.Where(p => p.GetType() != typeof(Dummy) && p.Checked);

            List<Organization> organizations = new List<Organization>();

            foreach (var kvPair in orgData)
            {
                Organization org = new Organization(kvPair.Key);

                foreach (ExcelDataRow f in kvPair.Value)
                {
                    foreach (AgricultureBase agriculture in selected)
                    {
                        Tuple<double, double> kp = KP.Instance.Get(agriculture.Id, f.SoilType, f.PH, f.Potassium);

                        int currYear = DateTime.Now.Year;

                        double actCs = f.Cs * Math.Exp((-0.693 * (currYear - _correctionYear))/ 30.16);
                        double actSr = f.Sr * Math.Exp((-0.693 * (currYear - _correctionYear))/ 29.12);

                        double qCs = Utils.Formula2(kp.Item1, actCs);
                        double qSr = Utils.Formula2(kp.Item2, actSr);

                        AgricultureBase? newCulture = Utils.GetCopy(agriculture);

                        if (newCulture == null)
                            continue;


                        int id = newCulture.Id;

                        newCulture.Cs = qCs;
                        newCulture.Sr = qSr;
                        newCulture.FieldArea = f.WorkRangeArea;

                        Type ncType = newCulture.GetType();

                        if (f.Cs != 0 && f.SoilType.Item1 != SoilType.Peat)
                        {
                            if (ncType == typeof(Roots))
                            {
                                newCulture.CsActivity = id % 110 == 0 ? Utils.Formula(PotatoesLevel.CsLevel, qCs) : Utils.Formula(VegetablesLevel.CsLevel, qCs);
                            }
                            else if (ncType == typeof(Seeds))  // Зерно
                            {
                                newCulture.CsActivity = Utils.Formula(SeedsLevel.CsLevel, qCs);
                            }
                        }

                        if (f.Sr != 0 && f.SoilType.Item1 != SoilType.Peat)
                        {
                            if (ncType == typeof(Roots))
                            {
                                newCulture.SrActivity = id % 110 == 0 ? Utils.Formula(PotatoesLevel.SrLevel, qSr) : Utils.Formula(VegetablesLevel.SrLevel, qSr);
                            }
                            else if (ncType == typeof(Seeds))  // Зерно
                            {
                                newCulture.SrActivity = Utils.Formula(SeedsLevel.SrLevel, qSr);
                            }
                        }

                        org.AddData(newCulture);
                    }

                }

                organizations.Add(org);
            }

            Display(organizations);
        }

        private void Display(List<Organization> orgs)
        {
            _dispatcher.Invoke(() =>
            {
                List<System.Windows.Media.Visual> toDisplay = new List<System.Windows.Media.Visual>();

                foreach (Organization org in orgs)
                {
                    
                    toDisplay.Add(Visualisation.GetLabel(org.Name));
                    if (PlantsNormative.CsNormative)
                    {
                        toDisplay.Add(Visualisation.GetLabel("Cs", 16));
                        toDisplay.Add(Visualisation.GetSummaryTable(org));
                    }
                    
                    if (PlantsNormative.SrNormative)
                    {
                        toDisplay.Add(Visualisation.GetLabel("Sr", 16));
                        toDisplay.Add(Visualisation.GetSummaryTable(org, true));
                    }
                    
                    if (PlantsNormative.CsNormative && PlantsNormative.SrNormative)
                    {
                        toDisplay.Add(Visualisation.GetLabel("Cs + Sr", 16));

                        foreach (var kvPair in org.Data)
                        {
                            toDisplay.Add(Visualisation.GetLabel(kvPair.Key, 16));
                            toDisplay.Add(Visualisation.GetMatrix(kvPair.Value));
                        }
                    }
                }

                OnDataDisplayReady?.Invoke(toDisplay);
            });
        }
    }
}
