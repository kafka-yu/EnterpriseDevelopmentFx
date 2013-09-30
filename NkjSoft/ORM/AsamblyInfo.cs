using System;
using System.Collections.ObjectModel;
using System.Xml;

namespace NkjSoft.ORM.Special
{
    /// <summary>
    /// 描述当前 NkjSoft.ORM 的信息。
    /// </summary>
    public class AsamblyInfo
    {
        private static Version latestVersion;
        /// <summary>
        /// 获取 NkjSoft.ORM 框架最后一次版本信息。
        /// </summary>
        public static Version LatestVersion
        {
            get { return latestVersion; }
        }


        private static string updateInfo;

        /// <summary>
        /// 获取ORM框架更新信息。
        /// </summary>
        public static string UpdateInfo
        {
            get { return updateInfo; }
        }

        static AsamblyInfo()
        {
            latestVersion = new Version(Properties.Resources.LatestVersion);
            updateInfo = Properties.Resources.AssamblyInfo;
        }
    }

}