using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Riddleyinnai.Fuctions
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Teleportcmd : ParentCommand
    {
        public override string Command => "teleport";

        public override string[] Aliases => new string[] { };

        public override string Description => "传送玩家";

        public override void LoadGeneratedCommands()
        {

        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            player.Teleport(new Vector3(float.Parse(arguments.At(0)), float.Parse(arguments.At(1)), float.Parse(arguments.At(2))));
            response = string.Empty;
            return true;
        }
    }
    internal class SupplyMap
    {
        private static void SpawnItem(Vector3 ord_pos, ItemType type, Tuple<Vector3, Vector3> tuple)
        {
            var pos = ord_pos;
            pos.x += tuple.Item1.x;
            pos.y += tuple.Item1.y;
            pos.z += tuple.Item1.z;
            Exiled.API.Features.Pickups.Pickup.CreateAndSpawn(type, pos, Quaternion.Euler(tuple.Item2));
            Log.Info($"Item:{type},spawned on {pos} ,rot = {tuple.Item2}");
        }
        private static void SpawnItem(Vector3 ord_pos, ItemType type, Tuple<Vector3, Quaternion> tuple)
        {
            var pos = ord_pos;
            pos.x += tuple.Item1.x;
            pos.y += tuple.Item1.y;
            pos.z += tuple.Item1.z;
            Exiled.API.Features.Pickups.Pickup.CreateAndSpawn(type, pos, tuple.Item2);
            Log.Info($"Item:{type},spawned on {pos} ,rot = {tuple.Item2}");
        }
        private static void Lcz_Plants()
        {
            var room = Room.Get(Exiled.API.Enums.RoomType.LczPlants);
            var spawnpoint = room.Position;
            var rot_v3 = room.Rotation.eulerAngles;
            rot_v3.y += 90;
            var dic_pos = new Dictionary<ItemType, Tuple<Vector3, Vector3>>()
            {
                //pos and rot
                {ItemType.KeycardResearchCoordinator,new Tuple<Vector3, Vector3>(new Vector3(-1.50999999f,1.13999999f,5.64599991f),rot_v3)},
                {ItemType.GunCOM15,new Tuple<Vector3, Vector3>(new Vector3(5.00299978f,1.13999999f,6.82000017f),rot_v3)},
            };
            foreach (var item in dic_pos)
            {
                SpawnItem(spawnpoint, item.Key, item.Value);
            }
        }
        private static void Lcz_Scp330()
        {
            var spawnpoint = Room.Get(Exiled.API.Enums.RoomType.Lcz330).Position;
            var dic_pos = new Dictionary<ItemType, Tuple<Vector3, Vector3>>()
            {
                //pos and rot
                {ItemType.SCP330,new Tuple<Vector3, Vector3>(new Vector3(1.26800001f,0.621999979f,-3.63700008f),new Vector3(0,0,0))},
                {ItemType.SCP330,new Tuple<Vector3, Vector3>(new Vector3(1.26800001f,0.621999979f,-3.3900001f),new Vector3(0,0,0))},
                {ItemType.SCP330,new Tuple<Vector3, Vector3>(new Vector3(1.86699998f,0.621999979f,-3.98600006f),new Vector3(0,0,0))},
                {ItemType.SCP330,new Tuple<Vector3, Vector3>(new Vector3(1.26800001f,1.57599998f,-3.37299991f),new Vector3(0,0,0))},
                {ItemType.SCP330,new Tuple<Vector3, Vector3>(new Vector3(1.26800001f,1.47800004f,-2.51099992f),new Vector3(0,0,0))},
                {ItemType.SCP330,new Tuple<Vector3, Vector3>(new Vector3(2.39299989f,1.47800004f,-2.51099992f),new Vector3(0,0,0))},
                {ItemType.SCP330,new Tuple<Vector3, Vector3>(new Vector3(0.972000003f,1.47800004f,-4.03299999f),new Vector3(0,0,0))},
            };
            foreach (var item in dic_pos)
            {
                SpawnItem(spawnpoint, item.Key, item.Value);
            }
        }
        private static void Lcz_Armory()
        {
            var spawnpoint = Door.Get(Exiled.API.Enums.DoorType.LczArmory).Position;
            var dic_pos = new Dictionary<ItemType, List<Tuple<Vector3, Vector3>>>
{
    {
        ItemType.GunFSP9, new List<Tuple<Vector3, Vector3>>
        {
            new Tuple<Vector3, Vector3>(new Vector3(-2.98399997f, 1.52699995f, 1.46800005f), new Vector3(0, 0, 0)),
            new Tuple<Vector3, Vector3>(new Vector3(2.86999989f, 1.52699995f, 1.47000003f), new Vector3(0, 0, 0))
        }
    },
    {
        ItemType.GunCrossvec, new List<Tuple<Vector3, Vector3>>
        {
            new Tuple<Vector3, Vector3>(new Vector3(-0.300000012f, 1.52699995f, 5.73999977f), new Vector3(0, 0, 0)),
            new Tuple<Vector3, Vector3>(new Vector3(0.245000005f, 1.52699995f, 5.73999977f), new Vector3(0, 0, 0))
        }
    },
    {
        ItemType.GunAK, new List<Tuple<Vector3, Vector3>>
        {
            new Tuple<Vector3, Vector3>(new Vector3(-0.143000007f, 1.48000002f, 3.2809999f), new Vector3(0, 0, 0))
        }
    }
};

            foreach (var item in dic_pos)
            {
                foreach (var itemInfo in item.Value)
                {
                    SpawnItem(spawnpoint, item.Key, itemInfo);
                }
            }

        }
        private static void Lcz_GR18()
        {
            var spawnpoint = Room.Get(Exiled.API.Enums.RoomType.LczGlassBox).Position;
            var dic_pos = new Dictionary<ItemType, Tuple<Vector3, Vector3>>()
            {
                //pos and rot
                {ItemType.ArmorCombat,new Tuple<Vector3, Vector3>(new Vector3(3.69300008f,1.11899996f,0.147f),new Vector3(0,0,0))},
                {ItemType.ArmorCombat,new Tuple<Vector3, Vector3>(new Vector3(3.69300008f,1.11899996f,0.147f),new Vector3(0,0,0))},
                {ItemType.ArmorCombat,new Tuple<Vector3, Vector3>(new Vector3(3.69300008f,1.11899996f,0.147f),new Vector3(0,0,0))},
                {ItemType.ArmorHeavy,new Tuple<Vector3, Vector3>(new Vector3(5.62900019f,1.11899996f,0.147f),new Vector3(0,0,0))},
                {ItemType.ArmorHeavy,new Tuple<Vector3, Vector3>(new Vector3(5.62900019f,1.11899996f,0.147f),new Vector3(0,0,0))},
                {ItemType.ArmorHeavy,new Tuple<Vector3, Vector3>(new Vector3(5.62900019f,1.11899996f,0.147f),new Vector3(0,0,0))},
            };
            foreach (var item in dic_pos)
            {
                SpawnItem(spawnpoint, item.Key, item.Value);
            }
        }
        public static void Roundstart()
        {
            Lcz_Armory();
            //Lcz_Scp330();
            Lcz_Plants();
            //Lcz_GR18();
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += Roundstart;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= Roundstart;
        }
    }
}
