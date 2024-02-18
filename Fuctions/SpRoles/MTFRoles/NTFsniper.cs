using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Fuctions.Items;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;

namespace Riddleyinnai.Fuctions.SpRoles.MTFRoles;

public class NTFsniper
{
    public static void SpawnASniper(Player player)
    {
        if (JuJiQiang.items.Count < 3)
        {
            YYYApi.MyApi.SetNickName("九尾狐狙击手", "", player);
            RoleManger.AddRole(player.Id,RoleManger.RoleName.九尾狐狙击手,"",Side.Mtf,false);
            JuJiQiang.GiveItem(player);
            foreach (Item item in player.Items)
            {
                if(item.Type == ItemType.GunE11SR)
                {
                    var gun = item as Firearm;
                    gun.Ammo = 1;
                }
            }
            Ui.PlayerMain.Send(player, "你是<color=#0F0>[九尾狐狙击手]</color>你使用E11将会造成巨量伤害，但是每次只能开一枪，并且将会消耗5枚子弹。", 5,
                Pos.正中偏下, 5);
        }
    }
}