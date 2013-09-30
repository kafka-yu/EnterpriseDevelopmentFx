using System;
using System.Collections.Generic;
using NkjSoft.Tools.ModelBuilder.DataBaseImpl;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// DataBaseServer 工厂
    /// </summary>
    public class Factory
    {
        static Dictionary<string, DataBaseServer> cache = null;
        /// <summary>
        /// Initializes the <see cref="Factory"/> class.
        /// </summary>
        static Factory()
        {
            if (cache == null)
            {
                cache = new Dictionary<string, DataBaseServer>();
                cache.Add(DataClient.SqlDbProvider.ToString(), new SqlDataBaseServer());
                cache.Add(DataClient.OleDbProvider.ToString(), new AccessDataBaseServer());
            }

        }
        /// <summary>
        /// 获取指定类型的 <see cref="NkjSoft.Tools.ModelBuilder.DataBaseServer"/> 的实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataBaseServer GetImpl<T>() where T : DataBaseServer
        {
            return getIDataBaseServiceByName(typeof(T).Name);
        }
        /// <summary>
        /// Gets the name of the I data base service by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static DataBaseServer getIDataBaseServiceByName(string name)
        {

            DataBaseServer server = null;
            cache.TryGetValue(name, out server);
            //throw new NullReferenceException(string.Format("容器中没有{0},请先将{0}对象加入容器!", name));
            return server;
        }
        /// <summary>
        /// 根据 <see cref="NkjSoft.Tools.ModelBuilder.DataBaseService"/> 获取指定类型的 <see cref="NkjSoft.Tools.ModelBuilder.DataBaseServer"/> 的实例。
        /// </summary>
        /// <param name="dbc">The DBC.</param>
        /// <returns></returns>
        public static DataBaseServer GetImpl(DataClient dbc)
        {
            return getIDataBaseServiceByName(dbc.ToString());
        }

        /// <summary>
        /// 将 <see cref="NkjSoft.Tools.ModelBuilder.DataBaseService"/> 实现加入到缓存。
        /// </summary>
        /// <param name="server"></param>
        public static void AddImpl(DataBaseServer server)
        {
            string name = server.GetType().Name;
            if (!cache.ContainsKey(name))
                cache[name] = server;
        }
        /// <summary>
        /// 将 <see cref="NkjSoft.Tools.ModelBuilder.DataBaseService"/> 实现加入到缓存。
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="name">The name.</param>
        public static void AddImpl(DataBaseServer server, string name)
        {
            if (!cache.ContainsKey(name))
                cache[name] = server;
        }
    }
}
