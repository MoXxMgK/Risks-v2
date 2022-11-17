using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risks_v2.Models
{
    public class AnimalsResult
    {
        public double CsAvg;

        public double CsMin;

        public double CsMax;

        public double SrAvg;

        public double SrMin;

        public double SrMax;

        public AnimalsResult(double csAvg, double csMin, double csMax, double srAvg, double srMin, double srMax)
        {
            CsAvg = csAvg;
            CsMin = csMin;
            CsMax = csMax;
            SrAvg = srAvg;
            SrMin = srMin;
            SrMax = srMax;
        }
    }
}
