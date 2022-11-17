using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Risks_v2.Models;

using OfficeOpenXml;
using Microsoft.Win32;

namespace Risks_v2.ViewModels
{
    public class ExcelDataViewModel : BaseViewModel
    {
        private string _excelFilePath = "";
        public string ExcelFilePath
        {
            get => _excelFilePath;
            set => SetField(ref _excelFilePath, value);
        }

        public virtual async Task<IEnumerable<ExcelDataRow>> LoadExcelData()
        {
            FileInfo excelFileInfo = new(ExcelFilePath);

            if (!excelFileInfo.Exists)
                throw new FileNotFoundException($"File {ExcelFilePath} does not exists.");

            using (ExcelPackage pkg = new(excelFileInfo))
            {
                List<ExcelDataRow> dataRows = new List<ExcelDataRow>();

                var sheet = pkg.Workbook.Worksheets[0];

                for (int row = 1; row <= sheet.Dimension.Rows; row++)
                {
                    string org = sheet.GetValue<string>(row, 1);
                    string fieldTypeName = sheet.GetValue<string>(row, 2);
                    int fieldId = sheet.GetValue<int>(row, 3);
                    double fieldArea = sheet.GetValue<double>(row, 4);
                    int workRangeId = sheet.GetValue<int>(row, 5);
                    double workRangeArea = sheet.GetValue<double>(row, 6);
                    int soilCode = sheet.GetValue<int>(row, 7);
                    string soilName = sheet.GetValue<string>(row, 8);
                    int hg = sheet.GetValue<int>(row, 9);
                    double ph = sheet.GetValue<double>(row, 10);
                    double hpc = sheet.GetValue<double>(row, 11);
                    int phosphorus = sheet.GetValue<int>(row, 12);
                    int potassium = sheet.GetValue<int>(row, 13);
                    int calcium = sheet.GetValue<int>(row, 14);
                    int magnium = sheet.GetValue<int>(row, 15);
                    double cs = sheet.GetValue<double>(row, 16);
                    double sr = sheet.GetValue<double>(row, 17);

                    ExcelDataRow dataRow = new ExcelDataRow(
                            org, fieldTypeName, fieldId, fieldArea, workRangeId, workRangeArea, soilCode, soilName,
                            hg, ph, hpc, phosphorus, potassium, calcium, magnium, cs, sr
                        );

                    dataRows.Add(dataRow);
                }

                var result = await Task.FromResult(dataRows);

                return result;
            }
        }

        public bool OpenSelectFileDialog()
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.RestoreDirectory = true;
            fd.Title = "Select excel file to open";
            fd.Filter = "Excel files (*.xls; *xlsx)|*.xls;*.xlsx";

            bool? result = fd.ShowDialog();

            if (result == null || result == false)
            {
                return false;
            }

            ExcelFilePath = fd.FileName;

            return true;
        }
    }
}
