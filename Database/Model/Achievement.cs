using System.Collections.Generic;

namespace Riddleyinnai.Database.Model
{
    public class Achievement
    {
        public string Userid { get; set; }
        public List<Achieve> Achieves { get; set; }
    }
}
