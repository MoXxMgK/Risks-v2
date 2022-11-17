using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Risks_v2.Models;
using System.Windows.Controls;
using System.Windows.Forms;
using Label = System.Windows.Controls.Label;
using System.Windows.Forms.Integration;

namespace Risks_v2.Helpers
{
    public static class Visualisation
    {
        public static Label GetLabel(string text, int fontSize = 18)
        {
            return new Label()
            {
                Content = text,
                FontSize = fontSize
            };
        }

        public static WindowsFormsHost GetSummaryTable(Organization org, bool sr = false)
        {
            DataGridView dataGrid = new DataGridView();

            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToOrderColumns = false;
            dataGrid.AllowUserToResizeColumns = true;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;

            dataGrid.Columns.Add("name", "Культура");
            dataGrid.Columns.Add("area", "Площадь");
            dataGrid.Columns.Add("min", "Min");
            dataGrid.Columns.Add("max", "Max");
            dataGrid.Columns.Add("avg", "Avg");
            dataGrid.Columns.Add("m1", "<10");
            dataGrid.Columns.Add("m2", "10-50");
            dataGrid.Columns.Add("m3", "50-90");
            dataGrid.Columns.Add("m4", ">90");

            foreach (var kvPair in org.Data)
            {
                string cultrName = kvPair.Key;
                List<AgricultureBase> cults = kvPair.Value;

                if (cults.Count == 0)
                    continue;

                // Area
                double area;
                try
                {
                    area = sr ?
                    Math.Round(cults.Where(c => c.SrActivity != -999).Sum(c => c.FieldArea), 2)
                    :
                    Math.Round(cults.Where(c => c.CsActivity != -999).Sum(c => c.FieldArea), 2);

                }
                catch
                {
                    area = 0;
                }

                // Min
                double min;

                try
                {
                    min = sr ?
                    Math.Round(cults.Where(c => c.Sr > 0).Min(c => c.Sr), 2)
                    :
                    Math.Round(cults.Where(c => c.Cs > 0).Min(c => c.Cs), 2);
                }
                catch
                {
                    min = 0;
                }

                // Max
                double max;

                try
                {
                    max = sr ?
                    Math.Round(cults.Max(c => c.Sr), 2)
                    :
                    Math.Round(cults.Max(c => c.Cs), 2);
                }
                catch
                {
                    max = 0;
                }

                // Average
                double avg;

                try
                {
                    avg = sr ?
                    Math.Round(cults.Where(c => c.SrActivity != -999).Average(c => c.Sr), 2)
                    :
                    Math.Round(cults.Where(c => c.CsActivity != -999).Average(c => c.Cs), 2);
                }
                catch
                {
                    avg = 0;
                }

                // M1

                double m1 = sr ?
                    Math.Round(cults.Where(c => c.SrActivity >= 0 && c.SrActivity < 10).Sum(c => c.FieldArea), 2)
                    :
                    Math.Round(cults.Where(c => c.CsActivity >= 0 && c.CsActivity < 10).Sum(c => c.FieldArea), 2);

                // M2

                double m2 = sr ?
                    Math.Round(cults.Where(c => c.SrActivity >= 10 && c.SrActivity < 50).Sum(c => c.FieldArea), 2)
                    :
                    Math.Round(cults.Where(c => c.CsActivity >= 10 && c.CsActivity < 50).Sum(c => c.FieldArea), 2);

                // M3

                double m3 = sr ?
                    Math.Round(cults.Where(c => c.SrActivity >= 50 && c.SrActivity < 90).Sum(c => c.FieldArea), 2)
                    :
                    Math.Round(cults.Where(c => c.CsActivity >= 50 && c.CsActivity < 90).Sum(c => c.FieldArea), 2);

                // M2

                double m4 = sr ?
                    Math.Round(cults.Where(c => c.SrActivity >= 90).Sum(c => c.FieldArea), 2)
                    :
                    Math.Round(cults.Where(c => c.CsActivity >= 90).Sum(c => c.FieldArea), 2);


                dataGrid.Rows.Add(cultrName, area, min, max, avg, m1, m2, m3, m4);
            }

            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = dataGrid;

            return host;
        }

        public static WindowsFormsHost GetMatrix(List<AgricultureBase> data)
        {
            DataGridView dataGrid = new DataGridView();

            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToOrderColumns = false;
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;

            dataGrid.Columns.Add("empty", "");
            dataGrid.Columns.Add("m1", "<10");
            dataGrid.Columns.Add("m2", "10-50");
            dataGrid.Columns.Add("m3", "50-90");
            dataGrid.Columns.Add("m4", ">90");
            dataGrid.Columns.Add("cs", "Cs-137");

            // Row 1
            double r0c0 = Math.Round(data.Where(c => c.CsActivity >= 0 && c.CsActivity < 10 && c.SrActivity >= 0 && c.SrActivity < 10).Sum(c => c.FieldArea), 2);
            double r0c1 = Math.Round(data.Where(c => c.CsActivity >= 10 && c.CsActivity < 50 && c.SrActivity >= 0 && c.SrActivity < 10).Sum(c => c.FieldArea), 2);
            double r0c2 = Math.Round(data.Where(c => c.CsActivity >= 50 && c.CsActivity < 90 && c.SrActivity >= 0 && c.SrActivity < 10).Sum(c => c.FieldArea), 2);
            double r0c3 = Math.Round(data.Where(c => c.CsActivity > 90 && c.SrActivity >= 0 && c.SrActivity < 10).Sum(c => c.FieldArea), 2);

            dataGrid.Rows.Add("<10", r0c0, r0c1, r0c2, r0c3);

            // Row 2
            double r1c0 = Math.Round(data.Where(c => c.CsActivity >= 0 && c.CsActivity < 10 && c.SrActivity >= 10 && c.SrActivity < 50).Sum(c => c.FieldArea), 2);
            double r1c1 = Math.Round(data.Where(c => c.CsActivity >= 10 && c.CsActivity < 50 && c.SrActivity >= 10 && c.SrActivity < 50).Sum(c => c.FieldArea), 2);
            double r1c2 = Math.Round(data.Where(c => c.CsActivity >= 50 && c.CsActivity < 90 && c.SrActivity >= 10 && c.SrActivity < 50).Sum(c => c.FieldArea), 2);
            double r1c3 = Math.Round(data.Where(c => c.CsActivity > 90 && c.SrActivity >= 10 && c.SrActivity < 50).Sum(c => c.FieldArea), 2);

            dataGrid.Rows.Add("10-50", r1c0, r1c1, r1c2, r1c3);

            // Row 3
            double r2c0 = Math.Round(data.Where(c => c.CsActivity >= 0 && c.CsActivity < 10 && c.SrActivity >= 50 && c.SrActivity < 90).Sum(c => c.FieldArea), 2);
            double r2c1 = Math.Round(data.Where(c => c.CsActivity >= 10 && c.CsActivity < 50 && c.SrActivity >= 50 && c.SrActivity < 90).Sum(c => c.FieldArea), 2);
            double r2c2 = Math.Round(data.Where(c => c.CsActivity >= 50 && c.CsActivity < 90 && c.SrActivity >= 50 && c.SrActivity < 90).Sum(c => c.FieldArea), 2);
            double r2c3 = Math.Round(data.Where(c => c.CsActivity > 90 && c.SrActivity >= 50 && c.SrActivity < 90).Sum(c => c.FieldArea), 2);

            dataGrid.Rows.Add("50-90", r2c0, r2c1, r2c2, r2c3);

            // Row 4
            double r3c0 = Math.Round(data.Where(c => c.CsActivity >= 0 && c.CsActivity < 10 && c.SrActivity >= 90).Sum(c => c.FieldArea), 2);
            double r3c1 = Math.Round(data.Where(c => c.CsActivity >= 10 && c.CsActivity < 50 && c.SrActivity >= 90).Sum(c => c.FieldArea), 2);
            double r3c2 = Math.Round(data.Where(c => c.CsActivity >= 50 && c.CsActivity < 90 && c.SrActivity >= 90).Sum(c => c.FieldArea), 2);
            double r3c3 = Math.Round(data.Where(c => c.CsActivity > 90 && c.SrActivity >= 90).Sum(c => c.FieldArea), 2);

            dataGrid.Rows.Add(">90", r3c0, r3c1, r3c2, r3c3);

            dataGrid.Rows.Add("Sr-90");

            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = dataGrid;

            return host;
        }
    }
}
