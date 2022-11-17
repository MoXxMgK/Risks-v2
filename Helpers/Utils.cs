using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Risks_v2.Models;

namespace Risks_v2
{
    public static class Utils
    {
        public static (SoilType, uint, uint) GetSoilType(uint soilCode)
        {
            var code1 = soilCode / 100 % 10;
            var code2 = soilCode / 1000 % 10;
            var code3 = soilCode / 10000;
            System.Diagnostics.Debug.WriteLine($"{code1}, {code2}, {code3}");
            SoilType t;

            if (code1 == 0)
            {
                t = SoilType.Peat;
            }
            else
            {
                if (code1 >= 2 && code1 <= 4)
                    t = SoilType.Loam;
                else if (code1 >= 5 && code1 <= 6)
                    t = SoilType.SandyLoam;
                else if (code1 >= 7 && code1 <= 8)
                    t = SoilType.Sand;
                else
                    t = SoilType.Peat;
            }

            return (t, code2, code3);
        }

        public static Dictionary<string, List<ExcelDataRow>> SeparateByOrgs(IEnumerable<ExcelDataRow> data)
        {
            Dictionary<string, List<ExcelDataRow>> result = new Dictionary<string, List<ExcelDataRow>>();

            foreach (ExcelDataRow row in data)
            {
                if (result.ContainsKey(row.OrganisationName))
                {
                    result[row.OrganisationName].Add(row);
                }
                else
                {
                    List<ExcelDataRow> rows = new List<ExcelDataRow>();
                    rows.Add(row);

                    result.Add(row.OrganisationName, rows);
                }
            }

            return result;
        }

        public static double Formula(double norm, double nuclq)
        {
            double r = (norm - nuclq) / (nuclq * 0.5);

            return (1 - (1 - Math.Pow(1 + 0.049867347 * r + 0.021141006 * Math.Pow(r, 2) + 0.003277626 * Math.Pow(r, 3) + 0.000038004 * Math.Pow(r, 4) + 0.000048891 * Math.Pow(r, 5) + 0.000005383 * Math.Pow(r, 6), -16) / 2)) * 100;
        }

        public static double Formula2(double kp, double q)
        {
            return kp * q * 37;
        }

        public static AgricultureBase? GetCopy(AgricultureBase original)
        {
            Type cultureType = original.GetType();
            var ctor = cultureType.GetConstructor(new Type[] { typeof(int), typeof(string) });

            AgricultureBase? newCulture = ctor?.Invoke(new object[] { original.Id, original.Name }) as AgricultureBase;

            return newCulture;
        }
    }
}
