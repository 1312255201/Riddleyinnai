using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Fuctions.SpRoleManage;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.Scientists;

public class scpcn08
{
    public static void SpawnACn08(Player player)
    {
        RoleManger.AddRole(player.Id,RoleManger.RoleName.SCPCN08,"",Side.Mtf,false);
        YYYApi.MyApi.SetNickName("SCP-CN-80","yellow",player);
        player.Scale = new Vector3(1, 1, 0.1f);
    }
}