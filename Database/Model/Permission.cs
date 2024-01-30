using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Database.Model
{
    public class Permission
    {
        public string UserId { get; set; } = "";
        public string Group { get; set; } = "";
        public string Disable { get; set; } = DateTime.MinValue.ToString("yyyy-MM-dd");
        public long QQ { get; set; } = -1;
        public string NickName { get; set; } = "";
    }
}
