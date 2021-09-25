using SharpScripter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpScripter
{
    public class IEData
    {
        public int ID { get; set; }
        public List<ScripterItem> Items { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
