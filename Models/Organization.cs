using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risks_v2.Models
{
    public class Organization
    {
        public string Name { get; set; }
        public Dictionary<string, List<AgricultureBase>> Data {get; set;}

        public Organization(string name)
        {
            Name = name;

            Data = new Dictionary<string, List<AgricultureBase>>();
        }

        public void AddData(AgricultureBase agriculture)
        {
            if (!Data.ContainsKey(agriculture.Name))
            {
                Data.Add(agriculture.Name, new List<AgricultureBase>());
            }

            Data[agriculture.Name].Add(agriculture);
        }
    }
}
