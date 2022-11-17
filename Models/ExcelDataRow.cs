using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risks_v2.Models
{
    public enum SoilType
    {
        Peat,
        Sand,
        Loam,
        SandyLoam
    }

    public struct ExcelDataRow
    {
        public string OrganisationName { get; set; }
        public string FieldTypeName { get; set; }
        public int FieldId { get; set; }
        public double FieldArea { get; set; }
        public int WorkRangeId { get; set; }
        public double WorkRangeArea { get; set; }
        public int SoilCode { get; set; }
        public string SoilName { get; set; }
        public (SoilType, uint, uint) SoilType { get; set; }
        public int HG { get; set; }
        public double PH { get; set; }
        public double HPC { get; set; }
        public int Phoosphorus { get; set; }
        public int Potassium { get; set; }
        public int Calcium { get; set; }
        public int Magnium { get; set; }
        public double Cs { get; set; }
        public double Sr { get; set; }

        public ExcelDataRow(string orgName, string fieldTypeName, int fieldId, double fieldArea, int workRangeId, double workRangeArea,
            int soilCode, string soilName, int hg, double ph, double hpc, int phosphorus, int potassium, int calcium, int magnium, double cs, double sr)
        {
            OrganisationName = orgName;
            FieldTypeName = fieldTypeName;
            FieldId = fieldId;
            FieldArea = fieldArea;
            WorkRangeId = workRangeId;
            WorkRangeArea = workRangeArea;
            SoilCode = soilCode;
            SoilName = soilName;
            HG = hg;
            PH = ph;
            HPC = hpc;
            Phoosphorus = phosphorus;
            Potassium = potassium;
            Calcium = calcium;
            Magnium = magnium;
            Cs = cs;
            Sr = sr;

            SoilType = Utils.GetSoilType((uint)soilCode);
        }
    }
}
