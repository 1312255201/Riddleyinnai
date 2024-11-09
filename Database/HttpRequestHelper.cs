namespace Riddleyinnai.Database
{
    public static class HttpRequestHelper
    {
 /*       private static async Task<string> PostAsync(string url, string strJson)//post异步请求方法
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //由HttpClient发出异步Post请求
                var client = new HttpClient();
                HttpResponseMessage res = await client.PostAsync(url, content).ConfigureAwait(false);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 异步方法，用来获取异步POST返回的信息，并进行一些操作。
        /// </summary>
        /// <param name="url">POST请求地址</param>
        /// <param name="strJson">POST请求的json字符串参数</param>
        /// <returns></returns>
        public static async Task<string> Post(string url, string strJson)
        {
            var content = await PostAsync(url, strJson);
            return content;
        }
        private static async Task<string> GetAsync(string url)//post异步请求方法
        {
            try
            {
                //由HttpClient发出异步Get请求
                var client = new HttpClient();
                HttpResponseMessage res = await client.GetAsync(url).ConfigureAwait(false);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 异步方法，用来获取异步GET返回的信息，请将参数填在后面的dic中
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static async Task<string> Get(string url, Dictionary<string, string> dic)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            var content = await GetAsync(builder.ToString());
            return content;
        }
        /// <summary>
        /// 同步的Http post 方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string data)
        {
            try
            {
                //创建post请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                byte[] payload = Encoding.UTF8.GetBytes(data);
                request.ContentLength = payload.Length;

                //发送post的请求
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();

                //接受返回来的数据
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string value = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();

                return value;
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 同步的Http get 方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Dictionary<string, string> dic)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(url);
                if (dic.Count > 0)
                {
                    builder.Append("?");
                    int i = 0;
                    foreach (var item in dic)
                    {
                        if (i > 0)
                            builder.Append("&");
                        builder.AppendFormat("{0}={1}", item.Key, item.Value);
                        i++;
                    }
                }
                //创建Get请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(builder.ToString());
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                //接受返回来的数据
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                string retString = streamReader.ReadToEnd();

                streamReader.Close();
                stream.Close();
                response.Close();

                return retString;
            }
            catch (Exception)
            {
                return "";
            }
        }*/
    }
}
