//--------------------------文档信息----------------------------
//       
//                 文件名: GeneratorConfigHelper                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Tools.ModelBuilder
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/19 10:56:06
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.Common.IO;
using System.IO;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 进行在Ini文件中配置选项的帮助类。该类无法被继承。
    /// </summary>
    public sealed class GeneratorConfigHelper
    {
        /// <summary>
        /// 获取或设置 ini 配置文件的路径,默认在 应用程序目录下面。
        /// </summary>
        public string IniFilePath { get; set; }

        /// <summary>
        /// Ini 文件读写器
        /// </summary>
        private IniFile iniFileHandler;

        #region --- 构造函数 ---

        /// <summary>
        /// 指定一个ini文件路径实例化 <see cref="GeneratorConfigHelper"/> 。
        /// </summary>
        /// <param name="iniFilename">The ini filename.</param>
        public GeneratorConfigHelper(string iniFilename)
        {
            InitializeIniHandler(iniFilename);
        }

        /// <summary>
        /// 通过默认获取应用程序下的Ini配置文件实例化 <see cref="GeneratorConfigHelper"/> 。
        /// </summary>
        public GeneratorConfigHelper()
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "generator.ini");
            InitializeIniHandler(filename);
        }

        private void InitializeIniHandler(string filename)
        {
            this.iniFileHandler = new IniFile(filename);
        }

        #endregion

    }
}
