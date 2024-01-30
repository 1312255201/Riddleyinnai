using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Database.Model
{
    public class User
    {
        public long Uid { get; set; } = 0;
        public string Userid { get; set; }
        public string Nickname { get; set; }
        public string Ipaddress { get; set; }
        public long Exp { get; set; } = 0;
        public string Slot { get; set; } = DateTime.MinValue.ToString("yyyy-MM-dd");
        public long Expplus { get; set; } = 1;
        public string Expplustime { get; set; } = DateTime.MinValue.ToString("yyyy-MM-dd");
        public string Joinboardcast { get; set; } = string.Empty;
        public string Lastkjointime { get; set; } = DateTime.MinValue.ToString("yyyy-MM-dd");
    }
}
