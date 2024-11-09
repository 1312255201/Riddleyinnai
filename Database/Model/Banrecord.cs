namespace Riddleyinnai.Database.Model
{
    public class Banrecord
    {
        public bool IsBan { get; set; } = false;
        public string Msg { get; set; } = string.Empty;
        public string Endtime { get; set; }
    }
}
