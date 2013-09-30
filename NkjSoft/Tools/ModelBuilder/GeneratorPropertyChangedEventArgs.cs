//--------------------------文档信息----------------------------
//       
//                 文件名: GeneratorPropertyChangedEventArgs                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Tools.ModelBuilder
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/19 11:04:24
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 为 <see cref="GeneratorPropertyChangedEventHandler"/> 事件处理函数提供数据。
    /// </summary>
    public class GeneratorPropertyChangedEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; private set; }


        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value { get; private set; }


        /// <summary>
        /// Gets the session.
        /// </summary>
        public string Session { get; private set; }
         


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="sessionName">Name of the session.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="val">The val.</param>
        public GeneratorPropertyChangedEventArgs(string sessionName, string propertyName, string val)
        {
            this.Session = sessionName;
            this.Key = propertyName;
            this.Value = val;
        }
    }


    /// <summary>
    /// 定义当生成器某个属性值变化的时候发生的事件的处理函数。
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="NkjSoft.Tools.ModelBuilder.GeneratorPropertyChangedEventArgs"/> instance containing the event data.</param>
    public delegate void GeneratorPropertyChangedEventHandler(object sender, GeneratorPropertyChangedEventArgs e);
}
