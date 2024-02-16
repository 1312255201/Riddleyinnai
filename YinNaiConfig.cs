using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai
{
    public sealed class YinNaiConfig : IConfig
    {
        [Description("是否开启插件")]
        public bool IsEnabled { get; set; } = false;
        [Description("是否开启插件Debug模式")]
        public bool Debug { get; set; } = false;
        [Description("数据库地址")]
        public string Dataurl { get; set; } = "http://localhost:5000/api";
        [Description("系统核弹时间(单位:秒)")]
        public int Syswarhead { get; set; } = 1800;
        [Description("服务器名称(部分插件唯一标识)")]
        public string Servername { get; set; } = "Riddleyinnai";
        [Description("服务器经验倍率")]
        public int Server_plus { get; set; } = 1;
        [Description("SCP173最大血量")]
        public int HP173 { get; set; } = -1;
        [Description("SCP049最大血量")]
        public int HP049 { get; set; } = -1;
        [Description("SCP106最大血量")]
        public int HP106 { get; set; } = -1;
        [Description("SCP939最大血量")]
        public int HP939 { get; set; } = -1;
        [Description("SCP096最大血量")]
        public int HP096 { get; set; } = -1;
        [Description("SCP049-2最大血量")]
        public int HP0492 { get; set; } = -1;
        [Description("玩家列表提示文字")]
        public string Playerlistmsg { get; set; } = "<size=50><color=#ffffff>谜子音奈</color></size><size=35><color=#800080>新纯净服</color><color=#87CEFA>〔QQ群:875268324〕</color></size>\n<size=35>回合人数<color=#14c714>{player_count}|</color>本回合SCP数:<color=red>{scp_counter}|</color>[.ac求助管理]</size>\n<size=30>请遵守服务器规则|点击<u><color=red>Server info</color></u>查看详细规则</size>";
        [Description("API地址")]
        public string ConfigAPI { get; set; } = "";
        [Description("AuthKey")]
        public string AuthKey { get; set; } = "";

        [Description("D9341最少刷新人数")] 
        public int d9341minnum  { get; set; }= 0;
        [Description("Scp493最少刷新人数")] 
        public int scp493minnum  { get; set; }= 0;
        [Description("Scp069最少刷新人数")] 
        public int scp069minnum  { get; set; }= 0;
        [Description("Scp181最少刷新人数")] 
        public int scp181minnum  { get; set; }= 0;
        [Description("Scp550最少刷新人数")] 
        public int scp550minnum  { get; set; }= 0;
        [Description("Scpcn08最少刷新人数")] 
        public int scpcn08minnum  { get; set; }= 0;

        [Description("SCP999最少刷新人数")] 
        public int scp999minmnum { get; set; } = 0;
        [Description("SCP999最少刷新人数")] 
        public int scp682minnum { get; set; } = 0;
        [Description("SCP035最少刷新人数")] 
        public int scp035minnum { get; set; } = 0;
        [Description("九尾狐医疗兵最少刷新人数")] 
        public int ntfheltherminnum { get; set; } = 0;
        [Description("九尾狐医疗兵最少刷新人数")] 
        public int ntfsniperminnum { get; set; } = 0;
        [Description("SCP3114最小刷新人数")] 
        public int scp3114numnum { get; set; } = 0;
        [Description("scp2490最小刷新人数")] 
        public int scp2490minnum { get; set; } = 0;
    }
}
