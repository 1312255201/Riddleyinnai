using Exiled.API.Features;
using Riddleyinnai.Database;
using Riddleyinnai.Database.Model;
using Riddleyinnai.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.User
{
    internal class Permission
    {
        public static void OnJoined(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if (ev != null)
            {
              /*  new Task(() =>
                {
                    var res = Methods.Getpermission(ev.Player.UserId);
                    if (res != null)
                    {
                        if (res.Success)
                        {
                            var permission = res.Data;
                            var endtime = DateTime.ParseExact(permission.Disable, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                            if (endtime < DateTime.Now)
                            {
                                try
                                {
                                    UserGroup userGroup = ServerStatic.GetPermissionsHandler().GetGroup(permission.Group);
                                    ev.Player.ReferenceHub.serverRoles.SetGroup(userGroup, false, true);
                                    ev.Player.RankColor = userGroup.BadgeColor;
                                    ev.Player.RankName = userGroup.BadgeText;
                                }
                                catch (Exception ex) {
                                    Log.Error(ex.Message);

                                }
                            }
                        }
                    }


                }).Start();*/
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Verified += OnJoined;
        }
        public static void Unregister() => Exiled.Events.Handlers.Player.Verified -= OnJoined;
    }
}
