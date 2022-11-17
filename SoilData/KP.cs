using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Risks_v2.Models;

namespace Risks_v2.SoilData
{
    public class KP
    {
        private static KP? _instance = null;
        public static KP Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KP();
                }

                return _instance;
            }
        }

        public static string DataFolder = "SoilData";

        private XmlElement? PeatM1;
        private XmlElement? PeatL1;
        private XmlElement? Sand;
        private XmlElement? SandyLoam;
        private XmlElement? Loam;

        private KP()
        {
            Load();
        }

        private void Load()
        {
            XmlElement? ReadFile(string name)
            {
                string dataPath = Path.Combine(Directory.GetCurrentDirectory(), DataFolder);
                string path = Path.Combine(dataPath, name + ".xml");

                if (!File.Exists(path))
                    throw new FileNotFoundException($"Missing file {path}");

                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                return doc.DocumentElement;
            }

            PeatM1 = ReadFile("PeatM1");
            PeatL1 = ReadFile("PeatL1");
            Sand = ReadFile("Sand");
            SandyLoam = ReadFile("SandyLoam");
            Loam = ReadFile("Loam");
        }

        public Tuple<double, double> Get(int id, (SoilType, uint, uint) soilData, double ph, double potassium)
        {
            XmlElement? data = null;

            var t = soilData.Item1;

            if (t == SoilType.Peat)
            {
                if (soilData.Item2 == 9)
                    data = PeatM1;
                else
                    data = PeatL1;
            }
            else if (t == SoilType.Sand)
                data = Sand;
            else if (t == SoilType.Loam)
                data = Loam;
            else if (t == SoilType.SandyLoam)
                data = SandyLoam;

            if (data == null)
                throw new InvalidOperationException("Error while geting data");

            double cs = 0;
            double sr = 0;

            XmlNode? cultureData = null;
            foreach (XmlElement n in data)
            {
                if (Convert.ToInt32(n.Attributes.GetNamedItem("id")?.Value) == id)
                {
                    cultureData = n;
                    break;
                }
            }

            if (cultureData == null)
                return new Tuple<double, double>(cs, sr);

            foreach (XmlNode row in cultureData["ph"]!.ChildNodes)
            {
                var attrs = row.Attributes;

                if (ph <= Convert.ToDouble(attrs?.GetNamedItem("level")?.Value))
                {
                    sr = Convert.ToDouble(attrs?.GetNamedItem("value")?.Value);
                    break;
                }
            }

            foreach (XmlNode row in cultureData["potassium"]!.ChildNodes)
            {
                var attrs = row.Attributes;

                if (potassium <= Convert.ToUInt32(attrs?.GetNamedItem("level")?.Value))
                {
                    cs = Convert.ToDouble(attrs?.GetNamedItem("value")?.Value);
                    break;
                }
            }

            if (cs == 0)
            {
                XmlNode? last = cultureData["potassium"]!.ChildNodes[^1];
                var attrs = last?.Attributes;

                cs = Convert.ToDouble(attrs?.GetNamedItem("value")?.Value);
            }

            return new Tuple<double, double>(cs, sr);
        }
    }
}
