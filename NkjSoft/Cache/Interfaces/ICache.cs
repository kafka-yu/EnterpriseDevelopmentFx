
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace NkjSoft.Cache.Interfaces
{
    /// <summary>
    /// 定义支持缓存能力提供的功能
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获取缓存对象的个数。
        /// </summary>
        int Count { get; }


        /// <summary>
        /// 获取缓存的所有对象的键集合。
        /// </summary>
        ICollection Keys { get; }


        /// <summary>
        /// 返回一个值，表示当前缓存容器中是否包含指定键的项。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);


        /// <summary>
        /// 从当前缓存容器中返回一个 <see cref="System.Object"/> 对象。
        /// </summary>
        object Get(object key);


        /// <summary>
        /// 从当前缓存容器中返回一个已知类型的对象。
        /// </summary>
        T Get<T>(object key);


        /// <summary>
        /// 从当前缓存容器中移除一个对象。
        /// </summary>
        void Remove(object key);


        /// <summary>
        /// 从当前缓存容器中移除所有键名包含在 <paramref name="keys"/>中的项。
        /// </summary>
        /// <param name="keys">键</param>
        void RemoveAll(ICollection keys);


        /// <summary>
        /// 清空当前缓存容器中所有缓存对象。
        /// </summary>
        void Clear();


        /// <summary>
        /// 向当前缓存容器添加一个项，该项以 <paramref name="Key"/> 为键。
        /// </summary>
        /// <param name="key">键.</param>
        /// <param name="value">值.</param>
        void Insert(object key, object value);


        /// <summary>
        /// 向当前缓存容器中加入一项，并制定对象的生存时长，以及是否受同级的对象的生命周期影响。
        /// </summary>
        void Insert(object key, object value, int timeToLive, bool slidingExpiration);


        /// <summary>
        /// 向当前缓存容器中加入一项，并制定对象的级别，生存时长，以及是否受同级的对象的生命周期影响。
        /// </summary>
        void Insert(object key, object value, int timeToLive, bool slidingExpiration, CacheItemPriority priority);
    }
}
