using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Usables;
using MapGeneration;
using MEC;
using PlayerRoles;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.YYYApi;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.ChaosRoles;

public class SCP2490Event
{
    public static bool scp2490spawnyes;
    public static bool coincd;
    public static bool flashcd;
    public static void SpawnAScp2490(Player player)
    {
        player.Role.Set(RoleTypeId.ChaosRifleman);
        Timing.CallDelayed(0.04f, () =>
        {
            SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP2490,"",Side.ChaosInsurgency,false);
            YYYApi.MyApi.SetNickName("SCP-2490", "", player);
            Ui.PlayerMain.Send(player, $"<color=#FFFFCC>你是:</color><color=#66FF66>[SCP-2490]</color>\n<color=#FFFFCC>1.匿迹：你的 SCP-268 使用冷却为</color><color=#FF3333>45秒</color>\n<color=#FFFFCC>2.精神干扰（暗杀）：使用帽子（SCP-268）时，背包额外获得一把USP或左轮，USP或左轮能</color><color=#FF3333>造成大量伤害</color>\n<color=#FFFFCC>3.传送：右键硬币时，传送到随机敌对阵营玩家身边。右键手电筒时，传送到随机队友身边。</color>",15,Ui.Pos.正中偏下,5);
            player.ClearInventory();
            player.AddItem(ItemType.SCP268);
            player.AddItem(ItemType.Medkit);
            player.AddItem(ItemType.Painkillers);
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Flashlight);
            player.Health = 650;
            player.EnableEffect<CustomPlayerEffects.Scp207>();
            foreach (var variablRoom in Room.List)
            {
                if (variablRoom.RoomName == RoomName.EzRedroom)
                {
                    player.Position = variablRoom.Position + Vector3.up;
                    break;
                }
            }
            player.ReferenceHub.playerEffectsController.ChangeState<CustomPlayerEffects.Scp207>(4);
        });
    }

    public static void OnShooting(ShootingEventArgs ev)
    {
        if (SpRoleManage.RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP2490))
        {
            if (ev.Player.CurrentItem.Type == ItemType.GunRevolver)
            {
                Timing.CallDelayed(0.02f, () =>
                {
                    ev.Player.RemoveItem(ev.Player.CurrentItem);
                });
            }
            else
            {
                ev.IsAllowed = false;
            }
        }
    }
    public static List<T> RandomSort<T>(List<T> list)
    {
        var random = new System.Random();
        var newList = new List<T>();
        foreach (var item in list)
        {
            newList.Insert(random.Next(newList.Count), item);
        }
        return newList;
    }
    public static void OnPlayerDropingItem(DroppingItemEventArgs ev)
    {
        if (SpRoleManage.RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP2490))
        {
            if(ev.Item.Serial == 24908)
            {
                ev.IsAllowed = false;
            }
            if (ev.Item.Type == ItemType.Coin)
            {
                ev.IsAllowed = false;
                if (!coincd)
                {
                    coincd = true;
                    Timing.CallDelayed(60f, () =>
                    {
                        coincd = false;
                    });
                    List<Player> awa = new List<Player>();
                    List<Player> awa2 = new List<Player>();
                    foreach (var pl in Player.List)
                    {
                        if (pl.IsAlive && pl.Role.Team != ev.Player.Role.Team && pl.Role.Type != RoleTypeId.ClassD)
                        {
                            awa.Add(pl);
                        }
                        awa2 = RandomSort(awa);
                        if (awa2.Any())
                        {
                            ev.Player.Position = awa2.First().Position + Vector3.up;
                        }
                    }
                }

            }

            if (ev.Item.Type == ItemType.Flashlight)
            {
                ev.IsAllowed = false;
                if (!flashcd)
                {
                    flashcd = true;
                    Timing.CallDelayed(60f, () =>
                    {
                        flashcd = false;
                    });
                    List<Player> awa = new List<Player>();
                    List<Player> awa2 = new List<Player>();
                    foreach (var pl in Player.List)
                    {
                        if (pl.IsAlive && pl.Role.Team == ev.Player.Role.Team && pl.Role.Type == RoleTypeId.ClassD)
                        {
                            awa.Add(pl);
                        }
                        awa2 = RandomSort(awa);
                        if (awa2.Any())
                        {
                            ev.Player.Position = awa2.First().Position + Vector3.up;
                        }
                        else
                        {
                            if (!Warhead.IsDetonated)
                            {
                                ev.Player.Position = GetRandomRoom().Position + Vector3.up;
                            }
                        }
                    }
                }
            }
        }
    }
    public static Room GetRandomRoom()
    {
        var rooms = new List<Room>();

        foreach (var room in Room.List)
        {
            if (room.Zone == ZoneType.Surface)
            {
                continue;
            }
            if (!NoContainsRadomRoom(room)) continue;
            if (Exiled.API.Features.Map.IsLczDecontaminated)
            {
                if (room.Zone != ZoneType.LightContainment)
                {
                    rooms.Add(room);
                }
            }
            else
            {
                rooms.Add(room);
            }
        }

        return rooms.GetRandomValue();
    }
    private static bool NoContainsRadomRoom(Room room)
    {
        return room.Type != RoomType.EzIntercom && room.Type != RoomType.EzShelter && room.Type != RoomType.Lcz173 && room.Type != RoomType.HczTesla &&
               room.Type != RoomType.Pocket && room.Type != RoomType.Hcz939 && room.Type != RoomType.EzCollapsedTunnel && room.Type != RoomType.HczTestRoom;
    }
    public static IEnumerator<float> SetCd(Player player)
    {
        while(true)
        {
            yield return Timing.WaitForSeconds(1f);
            if(UsableItemsController.GetHandler(player.ReferenceHub).PersonalCooldowns.ContainsKey(ItemType.SCP268))
            {
                if (UsableItemsController.GetHandler(player.ReferenceHub).PersonalCooldowns[ItemType.SCP268] >= Time.timeSinceLevelLoad + 45)
                {
                    UsableItemsController.GetHandler(player.ReferenceHub).PersonalCooldowns[ItemType.SCP268] = Time.timeSinceLevelLoad + 45;
                    Log.Info("我被执行了");
                    break;
                }
            }
        }
    }
    public static void OnUsingItem(UsingItemEventArgs ev)
    {
        if (SpRoleManage.RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP2490))
        {
            if (ev.Item.Type != ItemType.SCP268 && !ev.Item.IsAmmo && !ev.Item.Type.IsMedical() &&
                !ev.Item.IsThrowable && !ev.Item.IsKeycard)
            {
                ev.IsAllowed = false;
            }
            if (ev.Item.Type == ItemType.SCP268)
            {
                if (ev.IsAllowed)
                {
                    if (!UsableItemsController.GetHandler(ev.Player.ReferenceHub).PersonalCooldowns.ContainsKey(ItemType.SCP268)||UsableItemsController.GetHandler(ev.Player.ReferenceHub).PersonalCooldowns[ItemType.SCP268] <=
                        Time.timeSinceLevelLoad)
                    {
                        var awa = ev.Player.ReferenceHub.inventory.ServerAddItem(ItemType.GunRevolver, (ushort)24908) as Firearm ;
                        awa.Status = new FirearmStatus(8, awa.Status.Flags, awa.Status.Attachments);
                        Timing.RunCoroutine(SetCd(ev.Player));
                        Timing.CallDelayed(13f, () =>
                        {
                            if(ev.Player.Items.Where(x => x.Serial == 24908).Any())
                            {
                                ev.Player.RemoveItem(ev.Player.Items.Where(x => x.Serial == 24908).First());
                            }
                        });
                    }

                }
            }
        }
    }
    public static void OnUsedItem(UsedItemEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP2490))
        {
            if(ev.Item.Type == ItemType.SCP500)
            {
                Timing.CallDelayed(1f, () =>
                {
                    ev.Player.EnableEffect<CustomPlayerEffects.Scp207>();
                    ev.Player.ReferenceHub.playerEffectsController.ChangeState<CustomPlayerEffects.Scp207>(4);
                });
            }
        }

    }

    private static void OnPlayerPickingUpItem(PickingUpItemEventArgs ev)
    {        
        if (SpRoleManage.RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP2490))
        {
            if (ev.Pickup.Type != ItemType.SCP268 && !ev.Pickup.Type.IsAmmo() && !ev.Pickup.Type.IsMedical() &&
                !ev.Pickup.Type.IsThrowable() && !ev.Pickup.Type.IsKeycard())
            {
                ev.IsAllowed = false;
            }
        }
        
    }

    private static void OnRoundEnded(RoundEndedEventArgs ev)
    {
        scp2490spawnyes = false;
    }
    public static void Register()
    {
        Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
        Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Player.Shooting += OnShooting;
        Exiled.Events.Handlers.Player.DroppingItem += OnPlayerDropingItem;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
        Exiled.Events.Handlers.Player.Shooting -= OnShooting;        
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Player.DroppingItem -= OnPlayerDropingItem;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
    }
}