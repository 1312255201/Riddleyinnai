using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Database.Model
{
    public class Time
    {
        public string Userid { get; set; } = "";
        public long Timecount { get; set; } = 0;
        public long Today { get; set; } = 0;
        public string Updatetime { get; set; } = DateTime.MinValue.ToString("yyyy-MM-dd");
    }
}
