using CommandSystem;
using Discord;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using Riddleyinnai.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riddleyinnai.Fuctions.SpRoleManage;
using UnityEngine;
using YinnaiAPI;

namespace Riddleyinnai.User
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Hidebadge : ICommand
    {
        public string Command => "hidebadge";

        public string[] Aliases => new string[] { "hide" };

        public string Description => "隐藏自己的称号";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (!Badge.Hidebadges.Contains(player.UserId))
            {
                Badge.Hidebadges.Add(player.UserId);
                player.BadgeHidden = true;
                response = "成功隐藏了称号";
                return true;
            }
            else
            {
                Badge.Hidebadges.Remove(player.UserId);
                player.BadgeHidden = false;
                response = "成功显示了称号";
                return true;
            }
        }
    }
    public class Component : MonoBehaviour
    {
        public Player player;
        private float _counter;
        private int loopcount = 0;
        private bool first;
        private void Update()
        {
            _counter += Time.deltaTime;
            if (_counter < 1)
                return;
            _counter = 0;

            if (player != null)
            {
                //Log.Info($"nickname:{player.Nickname}");
                if (RoleManger.IsRole(player.Id))
                {
                    return;
                }
                if (Badge.badges.TryGetValue(player.UserId, out var badgemodel))
                {
                    if (Badge.Hidebadges.Contains(player.UserId) || player.GlobalBadge.HasValue)
                    {
                        return;
                    }
                    else
                    {
                        //Log.Info($"type = {badgemodel.Type} && text = {badgemodel.Text}");
                        if (badgemodel.Color == "rainbow")
                        {
                            //彩色
                            var ranks = badgemodel.Name.Split('#');
                            if (ranks.Length == 1)
                            {
                                player.RankColor = Badge.colors.RandomItem();
                                //不滚动
                                if (!first)
                                {
                                    player.RankName = badgemodel.Name;
                                    first = true;
                                }
                            }
                            else
                            {
                                player.RankName = ranks[loopcount];
                                player.RankColor = Badge.colors.RandomItem();
                                loopcount++;
                                if (loopcount > ranks.Length - 1)
                                {
                                    loopcount = 0;
                                }
                            }
                        }
                        else
                        {
                            //纯色
                            var ranks = badgemodel.Name.Split('#');
                            if (ranks.Length == 1)
                            {
                                if (!first)
                                {
                                    player.RankColor = badgemodel.Color;
                                    player.RankName = badgemodel.Name;
                                    first = true;
                                }
                            }
                            else
                            {
                                player.RankName = ranks[loopcount];
                                if (!first)
                                {
                                    first = true;
                                    player.RankColor = badgemodel.Color;
                                }
                                loopcount++;
                                if (loopcount > ranks.Length - 1)
                                {
                                    loopcount = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    
                }
            }
        }
    }
    internal class Badge
    {
        public static readonly List<string> colors = new List<string>()
       {
        ///青色 
        "cyan", 
        //番茄
        "tomato", 
        //银色 
        "silver", 
        //洋红 
        "magenta",
        //棕色 
        "brown", 
        //亮绿色 
        "light_green", 
        //深粉色 
        "deep_pink", 
        //深红色 
        "crimson", 
        //南瓜 
        "pumpkin", 
        //黄色
        "yellow", 
        //红色
        "red",
        //绿色
        "green", 
        //粉色
        "pink", 
        //水蓝色
        "aqua", 
        //白色
        "white", 
        //橙色
        "orange", 
        //荧光绿
        "mint"
    };

        public static List<string> Hidebadges = new List<string> { };

        public static Dictionary<string,API.Badge> badges = new Dictionary<string, API.Badge>();
        private static void OnJoined(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if (ev != null)
            {
                new Task(() =>
                {
                    Methods.InitPlayer(ev.Player);
                    var result = Methods.GetBandage(ev.Player.UserId);
                    if (result != null)
                    {
                        if (badges.TryGetValue(ev.Player.UserId, out var badge))
                        {
                            badges[ev.Player.UserId] = result;
                        }
                        else
                        {
                            badges.Add(ev.Player.UserId, result);
                        }

                        if (ev.Player.GameObject.TryGetComponent<Component>(out var _))
                        {
                            return;
                        }
                        ev.Player.GameObject.AddComponent<Component>().player = ev.Player;
                        //这里不需要判断是否加入新的
                    }
                }).Start();

            }
        }
        private static Dictionary<string,int> loopcount_dic = new Dictionary<string,int>();
        public static void BadgeCheck()
        {
            foreach (var item in badges)
            {
                if (Player.TryGet(item.Key, out var player))
                {
                    //Log.Info($"nickname:{player.Nickname}");
                    if (player != null)
                    {
                        var loopcount = 0;
                        if (!loopcount_dic.ContainsKey(player.UserId))
                        {
                            loopcount_dic.Add(player.UserId, 0);
                        }
                        try
                        {
                            loopcount = loopcount_dic[player.UserId];
                        }
                        catch { }
                        var badgemodel = item.Value;
                        if (Hidebadges.Contains(player.UserId) || player.GlobalBadge.HasValue)
                        {
                            /*
                            Log.Info("skip?");
                            Log.Info($"hidebadges:{Hidebadges.Where(x=>x == player.UserId).Count()}");
                            Log.Info($"global:{player.GlobalBadge.Value.Text}");
                            */
                            continue;
                            
                        }
                        else
                        {
                            //
                            if (badgemodel.Color == "rainbow")
                            {
                                //彩色
                                var ranks = badgemodel.Name.Split('#');
                                if (ranks.Length == 1)
                                {
                                    //不滚动
                                    player.RankColor = colors.RandomItem();
                                    //player.RankName = badgemodel.Name;
                                }
                                else
                                {
                                    player.RankName = ranks[loopcount];
                                    player.RankColor = colors.RandomItem();
                                    loopcount++;
                                    if (loopcount > ranks.Length - 1)
                                    {
                                        loopcount = 0;
                                    }
                                }
                            }
                            else
                            {
                                //纯色
                                var ranks = badgemodel.Name.Split('#');
                                //Log.Info($"ranks length = {ranks.Length}");
                                if (ranks.Length == 1)
                                {
                                    //不滚动
                                    player.RankColor = badgemodel.Color;
                                    player.RankName = badgemodel.Name;
                                    //Log.Info($"type = {badgemodel.Type} && text = {badgemodel.Text}");
                                    
                                }
                                else
                                {
                                    player.RankName = ranks[loopcount];
                                    player.RankColor = badgemodel.Color;
                                    loopcount++;
                                    if (loopcount > ranks.Length - 1)
                                    {
                                        loopcount = 0;
                                    }
                                }
                            }
                        }
                        loopcount_dic[player.UserId] = loopcount;
                    }
                }
            }
        }
        private static void Reset()
        {
            badges.Clear();
            Hidebadges.Clear();
            loopcount_dic.Clear();
        }
        public static void OnLeft(Exiled.Events.EventArgs.Player.DestroyingEventArgs ev)
        {
            badges.Remove(ev.Player.UserId);
            Hidebadges.RemoveAll(x => x == ev.Player.UserId);
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Destroying += OnLeft;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Verified += OnJoined;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Destroying -= OnLeft;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Verified -= OnJoined;
        }
    }
}
