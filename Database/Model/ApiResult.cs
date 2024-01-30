namespace Riddleyinnai.Database.Model
{
    /// <summary>
    /// 核心返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> where T : class
    {
        public bool Success { get; set; } = false;
        public string Error { get; set; }
        public T Data { get; set; }
        /// <summary>
        /// 200 = OK
        /// 404 = 找不到，但是能连通数据库
        /// 500 = 数据库无法连接
        /// </summary>
        public int Code { get; set; } = 200;
    }
}