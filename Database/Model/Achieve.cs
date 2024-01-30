using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Database.Model
{
    public class Achieve
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public bool status { get; set; }
        public int progress { get; set; }
        public bool hide { get; set; }
    }
}
