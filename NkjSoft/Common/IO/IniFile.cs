using System.Text;

namespace NkjSoft.Common.IO
{    
    //源自 博客园..
    /// <summary>
    /// 提供对指定文件路径的ini文件进行操作的对象。无法继承此类。
    /// </summary>
    public sealed class IniFile
    {
        #region --- 私有成员 ---

        private string path;
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion

        #region --- 构造函数 ---

        /// <summary>
        /// 初始化一个新的 <see cref="IniFile"/> 对象。
        /// </summary>
        /// <param name="iniFilePath">.ini文件路径</param>
        public IniFile(string iniFilePath)
        {
            path = iniFilePath;
        } 
        #endregion

        #region --- WriteValue ---

        /// <summary>       
        /// 向ini文件的指定节点写入键值对数据。   
        ///</summary>       
        ///<param name="Section">结点</param>        
        ///<param name="Key">名称</param> 
        ///<param name="Value">值</param>  
        public void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        #endregion

        #region --- ReadValue ---

        /// <summary>
        /// 从ini文件的指定节点和键名读取数据 。
        /// </summary>
        /// <param name="Section">结点</param>
        /// <param name="Key">名称</param>
        /// <returns>数据</returns>
        public string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
        #endregion
    }

}
