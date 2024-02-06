using CommandSystem.Commands.RemoteAdmin.Decontamination;
using Discord;
using Exiled.API.Features;
using Hints;
using Newtonsoft.Json;
using Riddleyinnai.Database.Model;
using Riddleyinnai.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YinnaiAPI;
using Badge = Riddleyinnai.Database.Model.Badge;
using User = Riddleyinnai.Database.Model.User;

namespace Riddleyinnai.Database
{
    /// <summary>
    /// 所有接口的集合
    /// </summary>
    public sealed class Methods
    {
        public static API YinnaiApi;
        private static YinNaiConfig _yinNaiConfig;
        private static Dictionary<string,API.BasicInfo> player_info = new Dictionary<string, API.BasicInfo>();
        public static void Init(YinNaiConfig Oconfig)
        {
            _yinNaiConfig = Oconfig;
            YinnaiApi = new API(_yinNaiConfig.ConfigAPI, _yinNaiConfig.AuthKey);
        }
        public static void InitPlayer(Player player)
        {
            YinnaiApi.InitPlayer(player.Nickname, player.UserId);
            var res = YinnaiApi.GetBasicInfo(player.UserId);
            if(res.Success)
            {
                player_info[player.UserId] =res;
            }
            else
            {
                Log.Warn("API拉取失败，请确定是否配置正常");
            }
        }
        public static API.Badge GetBandage(string userid)
        {
            if(player_info.ContainsKey(userid))
            {
                return player_info[userid].Badge;
            }
            else
            {
                return null;
            }
        }
        public static long GetExp(string userid)
        {
            if (player_info.ContainsKey(userid))
            {
                return (long)player_info[userid].Exp;
            }
            else
            {
                return 0;
            }
        }
        public static void AddExp(string userid, float num)
        {
            if(player_info.ContainsKey(userid))
            {
                YinnaiApi.AddExp(userid, (num * player_info[userid].ExpRatio ));
                player_info[userid].Exp += (num * player_info[userid].ExpRatio);
            }
        }
        /*public static ApiResult<Model.User> Get_user(string userid)
        {
            var result = new ApiResult<Model.User>();
            //默认值的user
            var res = HttpRequestHelper.HttpGet(url + "/User/Find", new Dictionary<string, string>() { { "Userid", userid } });
            if (res != null)
            {
                result = JsonConvert.DeserializeObject<ApiResult<Model.User>>(res);
            }
            return result;//没有的话会返回默认的
        }
        public static ApiResult<string> Update_user(Model.User user)
        {
            var result = new ApiResult<string>();

            var jsonstr = JsonConvert.SerializeObject(user);
            if (jsonstr != null)
            {
                var res = HttpRequestHelper.HttpPost(url + "/User/Edit", jsonstr);
                if (res != null)
                {
                    var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                    if (apiresult != null)
                    {
                        result = apiresult;
                    }

                }
            }
            return result;
        }*/
        /*     public static ApiResult<Banrecord> Checkban(string userid)
             {
                 var result = new ApiResult<Banrecord>();

                 var res = HttpRequestHelper.HttpGet(url + "/Ban/Check", new Dictionary<string, string> { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Banrecord>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static ApiResult<string> Addban(Ban ban)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(ban);
                 if(jsonstr != null)
                 {
                     var res = HttpRequestHelper.HttpPost(url + "/Ban/Add", jsonstr);
                     if(res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if(apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }
             public static ApiResult<string> Delban(string userid)
             {
                 var result = new ApiResult<string>();

                 var res = HttpRequestHelper.HttpGet(url + "/Ban/Del", new Dictionary<string, string>() { { "Userid", userid } });

                 if(res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                 }

                 return result;
             }
             public static ApiResult<List<Ban>> Findban(string userid)
             {
                 var result = new ApiResult<List<Ban>>();

                 var res = HttpRequestHelper.HttpGet(url + "/Ban/Find", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<List<Ban>>>(res);
                 }

                 return result;
             }
             public static async Task<ApiResult<Model.User>> Getuser(string userid)
             {
                 var result = new ApiResult<Model.User>();
                 //默认值的user
                 var res = await HttpRequestHelper.Get(url + "/User/Find", new Dictionary<string, string>() { { "Userid", userid } });
                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Model.User>>(res);
                 }
                 return result;//没有的话会返回默认的
             }

             public static ApiResult<string> Updateuser(Model.User user)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(user);
                 if (jsonstr != null)
                 {
                     var res = HttpRequestHelper.HttpPost(url + "/User/Edit", jsonstr);
                     if (res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if (apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }

             public static ApiResult<Permission> Getpermission(string userid)
             {
                 var result = new ApiResult<Permission>();
                 //默认值
                 var res = HttpRequestHelper.HttpGet(url + "/Permission/Get", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Permission>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static async Task<ApiResult<Guaranteed>> Getguaranteed(string userid)
             {
                 var result = new ApiResult<Guaranteed>();
                 //默认值
                 var res = await HttpRequestHelper.Get(url + "/Guaranteed/Get", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Guaranteed>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static async Task<ApiResult<string>> Updateguaranteed(Model.Guaranteed guaranteed)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(guaranteed);
                 if (jsonstr != null)
                 {
                     var res = await HttpRequestHelper.Post(url + "/Guaranteed/Edit", jsonstr);
                     if (res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if (apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }
             public static async Task<ApiResult<Time>> Gettime(string userid)
             {
                 var result = new ApiResult<Model.Time>();
                 //默认值的user
                 var res = await HttpRequestHelper.Get(url + "/Time/Get", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Model.Time>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static ApiResult<string> Updatetime(Model.Time time)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(time);
                 if (jsonstr != null)
                 {
                     var res = HttpRequestHelper.HttpPost(url + "/Time/Edit", jsonstr);
                     if (res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if (apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }
             public static ApiResult<Model.Achievement> Getachievement(string userid)
             {
                 var result = new ApiResult<Model.Achievement>();
                 //默认值的user
                 var res = HttpRequestHelper.HttpGet(url + "/Achievement/Find", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Model.Achievement>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static ApiResult<string> Updateachievement(Model.Achievement user)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(user);
                 if (jsonstr != null)
                 {
                     var res = HttpRequestHelper.HttpPost(url + "/Achievement/Edit", jsonstr);
                     if (res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if (apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }
             public static async Task<ApiResult<Model.Money>> Getmoney(string userid)
             {
                 var result = new ApiResult<Model.Money>();
                 //默认值的user
                 var res = await HttpRequestHelper.Get(url + "/Money/Get", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Model.Money>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static async Task<ApiResult<string>> Updatemoney(Model.Money user)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(user);
                 if (jsonstr != null)
                 {
                     var res = await HttpRequestHelper.Post(url + "/Money/Edit", jsonstr);
                     if (res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if (apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }
             public static ApiResult<Model.Weapons> Getweapons(string userid)
             {
                 var result = new ApiResult<Model.Weapons>();
                 //默认值的user
                 var res = HttpRequestHelper.HttpGet(url + "/Weapons/Get", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Model.Weapons>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static ApiResult<string> Updateweapons(Model.Weapons user)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(user);
                 if (jsonstr != null)
                 {
                     var res = HttpRequestHelper.HttpPost(url + "/Weapons/Edit", jsonstr);
                     if (res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if (apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }
             public static async Task<ApiResult<Model.KillRankModel>> Getkillrank_user(string userid)
             {
                 var result = new ApiResult<Model.KillRankModel>();
                 //默认值的user
                 var res = await HttpRequestHelper.Get(url + "/KillRank/GetUser", new Dictionary<string, string>() { { "Userid", userid } });

                 if (res != null)
                 {
                     result = JsonConvert.DeserializeObject<ApiResult<Model.KillRankModel>>(res);
                 }
                 return result;//没有的话会返回默认的
             }
             public static async Task<ApiResult<string>> Updatekillrank_user(Model.KillRankModel user)
             {
                 var result = new ApiResult<string>();

                 var jsonstr = JsonConvert.SerializeObject(user);
                 if (jsonstr != null)
                 {
                     var res = await HttpRequestHelper.Post(url + "/KillRank/Edit", jsonstr);
                     if (res != null)
                     {
                         var apiresult = JsonConvert.DeserializeObject<ApiResult<string>>(res);
                         if (apiresult != null)
                         {
                             result = apiresult;
                         }

                     }
                 }
                 return result;
             }
         */
    }
}