using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Risks_v2.Models;
using System.Windows;

namespace Risks_v2.ViewModels
{
    public class StandartExcelDataViewModel : ExcelDataViewModel
    {
        public static readonly string DataFolder = "Data";

        public ObservableCollection<string> Regions { get; private set; }
        public ObservableCollection<string> Districts { get; private set; }
        public ObservableCollection<string> Organizations { get; private set; }

        private string _selectedRegion = "";
        public string SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                SetField(ref _selectedRegion, value);
                // Update Districts
                LoadDistricts();

                DistrictsVisibility = Visibility.Visible;
                OrganisationsVisibility = Visibility.Collapsed;

                OnPropertyChanged(nameof(DistrictsVisibility));
                OnPropertyChanged(nameof(OrganisationsVisibility));

                CalculationAvailable = false;

                ExcelFilePath = "";
            }
        }

        private string _selectedDistrict = "";
        public string SelectedDistrict
        {
            get => _selectedDistrict;
            set
            {
                SetField(ref _selectedDistrict, value);
                // Update Organisations
                LoadOrganisations();

                OrganisationsVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(OrganisationsVisibility));

                ExcelFilePath = "";

                CalculationAvailable = false;
            }
        }

        private string _selectedOrganization = "";
        public string SelectedOrganization
        {
            get => _selectedOrganization;
            set
            {
                SetField(ref _selectedOrganization, value);
                // Construct path to excel file
                ExcelFilePath = Path.Combine(DataFolder, SelectedRegion, SelectedDistrict, value + ".xlsx");

                CalculationAvailable = true;
            }
        }

        public Visibility RegionVisibility { get; private set; }
        public Visibility DistrictsVisibility { get; private set; }
        public Visibility OrganisationsVisibility { get; private set; }

        private bool _calculationAvailable = false;
        public bool CalculationAvailable
        {
            get => _calculationAvailable;
            set
            {
                SetField(ref _calculationAvailable, value);

                if (value)
                {
                    CalculationButtonVisibility = Visibility.Visible;
                }
                else
                {
                    CalculationButtonVisibility = Visibility.Collapsed;
                }

                OnPropertyChanged(nameof(CalculationButtonVisibility));
            }
        }
        public Visibility CalculationButtonVisibility { get; private set; } = Visibility.Collapsed;

        public Command OpenFileCommand { get; set; }

        public StandartExcelDataViewModel()
        {
            OpenFileCommand = new Command(OpenFile);
            Regions = new ObservableCollection<string>();
            Districts = new ObservableCollection<string>();
            Organizations = new ObservableCollection<string>();

            DistrictsVisibility = Visibility.Collapsed;
            OrganisationsVisibility = Visibility.Collapsed;

            LoadRegions();
        }

        private void LoadRegions()
        {
            foreach (var dir in Directory.EnumerateDirectories(DataFolder))
            {
                Regions.Add(Path.GetFileName(dir));
            }
        }

        private void LoadDistricts()
        {
            Districts.Clear();

            string path = Path.Combine(DataFolder, SelectedRegion);

            foreach (var dir in Directory.EnumerateDirectories(path))
            {
                Districts.Add(Path.GetFileName(dir));
            }
        }

        private void LoadOrganisations()
        {
            Organizations.Clear();

            string path = Path.Combine(DataFolder, SelectedRegion, SelectedDistrict);

            foreach (var file in Directory.EnumerateFiles(path))
            {
                Organizations.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        private void OpenFile(object? parameter)
        {
            bool result = OpenSelectFileDialog();

            if (!result)
                return;

            RegionVisibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(RegionVisibility));
            DistrictsVisibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(DistrictsVisibility));
            OrganisationsVisibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(OrganisationsVisibility));

            CalculationAvailable = true;
        }
    }
}
