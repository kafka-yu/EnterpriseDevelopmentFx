//--------------------------文档信息----------------------------
//       
//                 文件名: Templates                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Tools.ModelBuilder.Templates
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/9 12:36:29
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Tools.ModelBuilder
{
    using NkjSoft.Extensions;
    /// <summary>
    /// 模板文件。无法继承此类。
    /// </summary>
    public static class Templates
    {
        /// <summary>
        /// 获取用于生成实体类文件的默认模板内容。该字段是只读的。
        /// </summary>
        public static readonly string DefaultModelTemplate = Properties.Resources.DefaultTemplate;
        /// <summary>
        /// 获取用于生成 DataContext 文件的默认模板内容。该字段是只读的。
        /// </summary>
        public static readonly string DefaultDataContextTemplate = Properties.Resources.DataContextTemplate;
         
        /// <summary>
        /// BllBase 的模版信息。该字段是只读的。
        /// </summary>
        public static readonly string BllBaseTemplate = Properties.Resources.BllBaseTemplate;

        /// <summary>
        /// Bll 层 Class 的模版。该字段是只读的。
        /// </summary>
        public static readonly string BllClassTemplate = Properties.Resources.BllClassTemplate;

        /// <summary>
        /// 获取用于标记在模版属性中选择程序集内容资源绑定的默认模版的Key。
        /// </summary>
        public static string DefaultDefaultModelTemplateTag { get { return "<UseResource>"; } }

        /// <summary>
        /// Gets the default value tag.
        /// </summary>
        public static string DefaultValueTag { get { return "<default>"; } }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string OnPropertyChanged(string obj)
        {
            return OnPropertyChanged(obj, DefaultValueTag);

        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="special">The special.</param>
        /// <returns></returns>
        internal static string OnPropertyChanged(string obj, string special)
        {
            //if (string.IsNullOrEmpty(obj) || obj.Trim().Length == 0)
            //{
            //    return special;
            //}
            //return obj;
            return obj;
        }

        /// <summary>
        /// Gets the filename from.
        /// </summary>
        /// <param name="gt">The gt.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="checker">The checker.</param>
        public static void GetFilenameFrom(this Generator gt, string propertyName, Func<string, bool> checker)
        {
            if (propertyName.IsNullOrEmpty())
                return;
            // get the target property value 
            var type = typeof(Generator);

            //get  property 
            var property = type.GetProperty(propertyName);

            var val = property.GetValue(gt, null);



        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly string ModelClassSessionName = "ModelOptions";
        /// <summary>
        /// 
        /// </summary>
        public static readonly string BllClassSessionName = "BllOptions";
        /// <summary>
        /// 
        /// </summary>
        public static readonly string BuilderOptions = "BuilderOptions";
    }
}
