using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Database.Model
{
    public class Ban
    {
        public string Userid { get; set; } = string.Empty;
        public string Endtime { get; set; } = DateTime.MaxValue.ToString("yyyy-MM-dd");
        public string Reason { get; set; } = string.Empty;
        public string Admin { get; set; } = string.Empty;
        public string Starttime { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
    }
}
