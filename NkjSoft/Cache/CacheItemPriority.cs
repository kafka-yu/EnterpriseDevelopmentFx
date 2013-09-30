
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;



namespace NkjSoft.Cache
{

    /// <summary>
    /// 定义一组值,表示缓存项的优先级别。 
    /// </summary>
    public enum CacheItemPriority
    {
        /// <summary>
        /// 低级别,可能比较快被缓存管理器删除。
        /// </summary>
        Low,

        /// <summary>
        /// 默认基本级别。
        /// </summary>        
        Normal,

        /// <summary>
        /// 高级别，很少被删除。
        /// </summary>
        High,

        /// <summary>
        /// 不被删除的级别。
        NotRemovable,

        /// <summary>
        /// 默认级别。 
        /// </summary>
        Default = Normal
    }
}
