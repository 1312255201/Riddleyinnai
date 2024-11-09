using Exiled.API.Features;
using System.Collections.Generic;

namespace Riddleyinnai
{
    internal class Publicmethod
    {
        public static bool Checkpermission(Player player,List<string> groups)
        {
            foreach (string group in groups)
            {
                if (player.GroupName.ToLower().Contains(group.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsMsgOk(string msg,out string reason)
        {
            if (msg == "")
            {
                reason = "您还没有输入内容";
                return false;
            }
            if(msg.Contains("<color=") || msg.Contains("<size="))
            {
                reason = "消息中包含富文本代码 请勿使用!";
                return false;
            }
            if(msg.Length >= 25)
            {
                reason = "请勿在聊天中使用太多字符![上限25字符]";
                return false;
            }

            reason = "";
            return true;
        }
    }
}
