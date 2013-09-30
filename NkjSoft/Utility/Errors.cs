using System;
using System.Collections.Generic;
using System.Text;

namespace NkjSoft.Utility
{
    /// <summary>
    /// 程序错误列表。
    /// </summary>
    internal class Errors
    {
        /// <summary>
        /// 没有指定数据访问程序节点信息错误。
        /// </summary>
        public static string NonProviderSettingException = "\r\n没有设置{0},请在配置文件的 {1} 节点中添加 {2} 的子节,并设置有效的值!";

    }
}
