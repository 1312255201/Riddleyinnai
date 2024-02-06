using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.Scientists;

public class scpcn08
{
    public static void SpawnACn08(Player player)
    {
        RoleManger.AddRole(player.Id,RoleManger.RoleName.SCPCN08,"",Side.Mtf,false);
        YYYApi.MyApi.SetNickName("SCP-CN-80","cyan",player);
        Ui.PlayerMain.Send(player,"<color=#FFFFCC>你是:</color><color=#0066FF>[SCP-cn08]</color>\n<color=#FFFFCC>1.你只是个</color><color=#FF3333>纸片人</color>",10,Pos.正中偏下,5);
        player.Scale = new Vector3(1, 1, 0.1f);
    }
}