using Exiled.API.Features;
using Riddleyinnai.Database;
using Riddleyinnai.Database.Model;
using Riddleyinnai.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Fuctions
{
    internal class Playtime
    {
        public static TimeSpan Get_today_time(Player player)
        {
            if(timecache.Count(x=>x.Userid == player.UserId) >= 1)
            {
                var model = timecache.Find(x => x.Userid == player.UserId);
                if(model != null)
                {
                    if(Jointime.TryGetValue(player.UserId,out var dateTime))
                    {
                        return (DateTime.Now - dateTime).Add(TimeSpan.FromSeconds(model.Today));
                    }
                    else
                    {
                        return TimeSpan.FromSeconds(model.Today);
                    }
                }
            }
            else
            {
                if (Jointime.TryGetValue(player.UserId, out var date))
                {
                    return (DateTime.Now - date);
                }
                else
                {
                    return TimeSpan.FromSeconds(0);
                }
            }
            return TimeSpan.FromSeconds(0);
        }
        private static Dictionary<string,DateTime> Jointime = new Dictionary<string,DateTime>();    
        public static void Reset()
        {
            Jointime.Clear();
            timecache.Clear();
        }
        public static void OnLeft(Exiled.Events.EventArgs.Player.LeftEventArgs ev)
        {
            if(ev.Player != null)
            {
                if(Jointime.TryGetValue(ev.Player.UserId,out var dateTime))
                {
                    var time = DateTime.Now - dateTime;
                    var model = timecache.Find(x => x.Userid == ev.Player.UserId);
                    if (model.Userid != "")
                    {
                        model.Timecount += (long)time.TotalSeconds;
                        model.Today += (long)time.TotalSeconds;

                        new Task(() =>
                        {
                           // Methods.Updatetime(model);
                        }).Start();
                    }
                }
                Jointime.Remove(ev.Player.UserId);
                timecache.RemoveAll(x => x.Userid == ev.Player.UserId);
            }
        }
        public static void OnJoined(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if(ev.Player != null)
            {
                Jointime.Add(ev.Player.UserId, DateTime.Now);
            }
            var timemodel = new Database.Model.Time();
            /*var res = await Methods.Gettime(ev.Player.UserId);
            if (res != null)
            {
                if (res.Success)
                {
                    var update = DateTime.ParseExact(res.Data.Updatetime, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    if (update < DateTime.Today)
                    {
                        //需要更新
                        timemodel = res.Data;
                        timemodel.Updatetime = DateTime.Today.ToString("yyyy-MM-dd");
                        timemodel.Today = 0;

                        timecache.Add(timemodel);
                    }
                    else
                    {
                        timemodel = res.Data;
                        timecache.Add(timemodel);
                    }
                }
                else
                {
                    if (res.Code == 404)
                    {
                        //
                        timemodel.Userid = ev.Player.UserId;
                        timemodel.Updatetime = DateTime.Today.ToString("yyyy-MM-dd");
                        timemodel.Today = 0;
                        timemodel.Timecount = 0;
                        timecache.Add(timemodel);
                    }
                    else
                    {
                        //
                    }
                }
            }
            */
        }
        private static List<Database.Model.Time> timecache = new List<Time>();
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Verified += OnJoined;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.Verified -= OnJoined;
        }
    }
}
