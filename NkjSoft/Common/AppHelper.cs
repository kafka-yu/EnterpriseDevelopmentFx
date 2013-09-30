//--------------------------文档信息----------------------------
//       
//                 文件名: AppHelper                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Common
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/26 18:43:27
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.Extensions;
namespace NkjSoft.Common
{
    /// <summary>
    /// 封装常用辅助方法。无法继承此类。
    /// </summary>
    public sealed class AppHelper
    {
        /// <summary>
        /// 从程序配置文件中的 AppSetting 节点获取指定Key的项的值。
        /// </summary>
        /// <param name="key">Key值</param>
        /// <returns></returns>  
        public static string GetAppSetting(string key)
        {
            string o = System.Configuration.ConfigurationManager.AppSettings[key];

            return null == o ? string.Empty : o;
        }

        /// <summary>
        /// 返回特定值类型的泛型结果。
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="key">Key值</param>
        /// <returns></returns>
        public static TResult GetAppSetting<TResult>(string key) where TResult : struct
        {
            string o = GetAppSetting(key);

            if (o.Trim().Length == 0) return default(TResult);

            return o.To<TResult>(default(TResult));

        }

        /// <summary>
        /// 获取指定名字的连接字符串。如果指定的项不存在，则返回 <see cref="System.String"/>.Empty。
        /// </summary>
        /// <param name="connName">Name</param>
        /// <returns></returns>
        public static string GetConnectionSetting(string connName)
        {
            var section = System.Configuration.ConfigurationManager.ConnectionStrings[connName];
            if (section == null)
                return string.Empty;
            return section.ConnectionString;
        }


    }
}
