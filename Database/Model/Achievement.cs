using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Database.Model
{
    public class Achievement
    {
        public string Userid { get; set; }
        public List<Achieve> Achieves { get; set; }
    }
}
