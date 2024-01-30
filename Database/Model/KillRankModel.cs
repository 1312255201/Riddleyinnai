using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Database.Model
{
    public class KillRankModel
    {
        public string Userid { get; set; }
        public string Nickname { get; set; }
        public long Killcount { get; set; }
        public long Level { get; set; }
        public string Prename { get; set; }
    }
}
