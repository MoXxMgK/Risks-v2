using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using Risks_v2.Helpers;
using Risks_v2.SoilData;
using Risks_v2.Models;

namespace Risks_v2.ViewModels
{
    public class FeedsProductionViewModel : StandartExcelDataViewModel
    {
        public ObservableCollection<AgricultureBase> Products { get; set; } = new ObservableCollection<AgricultureBase>();
        public ObservableCollection<string> Usages { get; set; } = new ObservableCollection<string>();

        private string _usage = "";
        public string Usage
        {
            get => _usage;
            set
            {
                PlantsNormative.SrEnabled = true;
                SetField(ref _usage, value);

                if (value == "Мясо")
                {
                    PlantsNormative.SrEnabled = false;
                    PlantsNormative.SrNormative = false;

                    SeedsLevel.CsLevel = 480; SeedsLevel.SrLevel = 0;
                    GreensLevel.CsLevel = 240; GreensLevel.SrLevel = 0;
                    SenoLevel.CsLevel = 1300; SenoLevel.SrLevel = 0;
                    SolomaLevel.CsLevel = 700; SolomaLevel.SrLevel = 0;
                    SenageLevel.CsLevel = 500; SenageLevel.SrLevel = 0;
                    SilosLevel.CsLevel = 240; SilosLevel.SrLevel = 0;
                    PotatoesLevel.CsLevel = 300; PotatoesLevel.SrLevel = 0;
                }
                else if (value == "Молоко")
                {
                    SeedsLevel.CsLevel = 180; SeedsLevel.SrLevel = 100;
                    GreensLevel.CsLevel = 165; GreensLevel.SrLevel = 37;
                    SenoLevel.CsLevel = 1300; SenoLevel.SrLevel = 260;
                    SolomaLevel.CsLevel = 330; SolomaLevel.SrLevel = 185;
                    SenageLevel.CsLevel = 500; SenageLevel.SrLevel = 100;
                    SilosLevel.CsLevel = 240; SilosLevel.SrLevel = 50;
                    PotatoesLevel.CsLevel = 160; PotatoesLevel.SrLevel = 37;
                }
                else if (value == "Молоко-переработка")
                {
                    SeedsLevel.CsLevel = 600; SeedsLevel.SrLevel = 500;
                    GreensLevel.CsLevel = 600; GreensLevel.SrLevel = 185;
                    SenoLevel.CsLevel = 1850; SenoLevel.SrLevel = 1300;
                    SolomaLevel.CsLevel = 900; SolomaLevel.SrLevel = 900;
                    SenageLevel.CsLevel = 900; SenageLevel.SrLevel = 500;
                    SilosLevel.CsLevel = 600; SilosLevel.SrLevel = 250;
                    PotatoesLevel.CsLevel = 600; PotatoesLevel.SrLevel = 185;
                }
            }
        }

        public Normative PlantsNormative { get; set; }
        public RadiationLevel SeedsLevel { get; set; }
        public RadiationLevel GreensLevel { get; set; }
        public RadiationLevel SenoLevel { get; set; }
        public RadiationLevel SolomaLevel { get; set; }
        public RadiationLevel SenageLevel { get; set; }
        public RadiationLevel SilosLevel { get; set; }
        public RadiationLevel PotatoesLevel { get; set; }

        public Command CalculateCommand { get; set; }

        public event Action<List<System.Windows.Media.Visual>>? OnDataDisplayReady;

        private readonly Dispatcher _dispatcher;

        public FeedsProductionViewModel(Dispatcher dispatcher)
        {
            Title = "Кормопроизводство";

            _dispatcher = dispatcher;

            PlantsNormative = new Normative();

            // Load from file or set default here
            SeedsLevel = new RadiationLevel(180, 100);
            GreensLevel = new RadiationLevel(165, 37);
            SenoLevel = new RadiationLevel(1300, 260);
            SolomaLevel = new RadiationLevel(330, 185);
            SenageLevel = new RadiationLevel(500, 100);
            SilosLevel = new RadiationLevel(240, 50);
            PotatoesLevel = new RadiationLevel(160, 37);

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
            Products.Add(new GreenMassa(210, "Озимая рожь ЗМ"));
            Products.Add(new GreenMassa(214, "Многолетние злаковые ЗМ"));
            Products.Add(new GreenMassa(215, "Многолетние бобово-злаковые ЗМ"));
            Products.Add(new GreenMassa(216, "Кукуруза ЗМ"));
            Products.Add(new GreenMassa(217, "Горохо-овсяная смесь ЗМ"));
            Products.Add(new GreenMassa(218, "Вика-овсяная смесь ЗМ"));
            Products.Add(new Seno(220, "Многолетние злаковые Сено"));
            Products.Add(new Seno(221, "Многолетние бобово-злаковые Сено"));
            Products.Add(new Soloma(230, "Овес солома"));
            Products.Add(new Soloma(232, "Ячмень Солома"));
            Products.Add(new Senage(240, "Многолетние злаковые Сенаж"));
            Products.Add(new Senage(240, "Многолетние бобово-злаковые Сенаж"));
            Products.Add(new Silos(250, "Кукуруза Силос"));
            Products.Add(new Roots(110, "Картофель"));
            Products.Add(new Roots(111, "Морковь"));
            Products.Add(new Roots(112, "Свекла"));

            Usages.Add("Молоко");
            Usages.Add("Молоко-переработка");
            Usages.Add("Мясо");

            Usage = Usages[0];
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

                        double qCs = Utils.Formula2(kp.Item1, f.Cs);
                        double qSr = Utils.Formula2(kp.Item2, f.Sr);

                        AgricultureBase? newCulture = Utils.GetCopy(agriculture);

                        if (newCulture == null)
                            continue;


                        int id = newCulture.Id;

                        newCulture.Cs = qCs;
                        newCulture.Sr = qSr;
                        newCulture.FieldArea = f.WorkRangeArea;

                        Type ncType = newCulture.GetType();

                        if (f.Cs != 0)
                        {
                            if (ncType == typeof(Roots) && f.SoilType.Item1 != SoilType.Peat)
                            {
                                newCulture.CsActivity = Utils.Formula(PotatoesLevel.CsLevel, qCs);
                            }
                            else if (ncType == typeof(Seeds) && f.SoilType.Item1 != SoilType.Peat)  // Зерно
                            {
                                newCulture.CsActivity = Utils.Formula(SeedsLevel.CsLevel, qCs);
                            }
                            else if (ncType == typeof(Soloma) && f.SoilType.Item1 != SoilType.Peat)  // Зерно
                            {
                                newCulture.CsActivity = Utils.Formula(SolomaLevel.CsLevel, qCs);
                            }
                            else if (ncType == typeof(Silos))
                            {
                                newCulture.CsActivity = Utils.Formula(SilosLevel.CsLevel, qCs);
                            }
                            else if (ncType == typeof(Senage))
                            {
                                newCulture.CsActivity = Utils.Formula(SenageLevel.CsLevel, qCs);
                            }
                            else if (ncType == typeof(Seno))
                            {
                                newCulture.CsActivity = Utils.Formula(SenoLevel.CsLevel, qCs);
                            }
                            else if (ncType == typeof(GreenMassa) && (f.SoilType.Item1 != SoilType.Peat || new int[] { 214, 215, 216 }.Contains(newCulture.Id)))
                            {
                                newCulture.CsActivity = Utils.Formula(GreensLevel.CsLevel, qCs);
                            }

                        }

                        if (f.Sr != 0)
                        {
                            if (ncType == typeof(Roots) && f.SoilType.Item1 != SoilType.Peat)
                            {
                                newCulture.SrActivity = Utils.Formula(PotatoesLevel.SrLevel, qSr);
                            }
                            else if (ncType == typeof(Seeds) && f.SoilType.Item1 != SoilType.Peat)  // Зерно
                            {
                                newCulture.SrActivity = Utils.Formula(SeedsLevel.SrLevel, qSr);
                            }
                            else if (ncType == typeof(Soloma) && f.SoilType.Item1 != SoilType.Peat)  // Зерно
                            {
                                newCulture.SrActivity = Utils.Formula(SolomaLevel.SrLevel, qSr);
                            }
                            else if (ncType == typeof(Silos))
                            {
                                newCulture.SrActivity = Utils.Formula(SilosLevel.SrLevel, qSr);
                            }
                            else if (ncType == typeof(Senage))
                            {
                                newCulture.SrActivity = Utils.Formula(SenageLevel.SrLevel, qSr);
                            }
                            else if (ncType == typeof(Seno))
                            {
                                newCulture.SrActivity = Utils.Formula(SenoLevel.SrLevel, qSr);
                            }
                            else if (ncType == typeof(GreenMassa) && (f.SoilType.Item1 != SoilType.Peat || new int[] { 214, 215, 216 }.Contains(newCulture.Id)))
                            {
                                newCulture.SrActivity = Utils.Formula(GreensLevel.SrLevel, qSr);
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
