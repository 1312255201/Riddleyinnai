using Exiled.API.Features;
using System;

namespace Riddleyinnai.Ui
{
    public enum Pos
    {
        顶部两行,
        顶部正中,
        屏幕偏右六行,
        屏幕正中左边五行,
        正下方,
        正中偏下,
        底部两行
    }
    public class Model
    {
        public Player player { get; set; }
        public string text { get; set; }
        public DateTime endtime { get; set; }
        public Pos pos { get; set; }
        public int weight { get; set; } = 5;
        public bool countdown { get; set; } = true;
    }
}
