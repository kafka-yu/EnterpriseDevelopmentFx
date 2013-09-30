//--------------------------文档信息----------------------------
//       
//                 文件名: FileManager                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: Test
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/21 13:59:46
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.IO;
namespace NkjSoft.Common.IO
{
    /// <summary>
    /// 一个简单的通过递归实现的文件搜索器。无法继承此类
    /// </summary>
    public sealed class FileFinder
    {
        /// <summary>
        /// 在搜索到文件并定位到文件路径的时候触发。
        /// </summary>
        public event EventHandler<FileFoundEventArgs> FileFound;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFinder"/> class.
        /// </summary>
        public FileFinder()
        {
            Extension = "*.*";
        }
        /// <summary>
        /// Searches all files. With Event CallBack
        /// </summary>
        /// <param name="strPath">The STR path.</param>
        /// <param name="extension">The extension.</param>
        private void SearchAllFiles(string strPath)
        {
            string[] folders = Directory.GetDirectories(strPath);
            foreach (string folder in folders)
            {
                string[] files = Directory.GetFiles(folder, Extension);
                foreach (string file in files)
                {
                    //Console.WriteLine(file);
                    FileFound(this, new FileFoundEventArgs(file, false));
                }
                SearchAllFiles(folder);
            }
        }


        /// <summary>
        /// Searches all files. Without Event CallBack
        /// </summary>
        /// <param name="strPath">The STR path.</param>
        /// <param name="extension">The extension.</param>
        private void SearchAllFiles2(string strPath)
        {
            string[] folders = Directory.GetDirectories(strPath);
            foreach (string folder in folders)
            {
                string[] files = Directory.GetFiles(folder, Extension);
                SearchAllFiles2(folder);
            }
        }

        /// <summary>
        /// 搜索指定目录下的所有文件和文件夹。
        /// </summary>
        /// <param name="dirToSearch">搜索的目标目录.</param>
        public void SearchDirectory(string dirToSearch)
        {
            if (Directory.Exists(dirToSearch))
            {
                string[] rootFiles = Directory.GetFiles(dirToSearch, Extension);//, "*.aspx" 
                if (FileFound != null)
                    foreach (string rootfile in rootFiles)
                    {
                        FileFound(this, new FileFoundEventArgs(rootfile, true));// //Console.WriteLine(rootfile); 
                    }
                if (FileFound == null)
                {
                    SearchAllFiles2(dirToSearch);
                }
                else
                    SearchAllFiles(dirToSearch);
            } 
        }

        /// <summary>
        /// Raises the <see cref="E:FileFound"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FileFoundEventArgs"/> instance containing the event data.</param>
        private void OnFileFound(FileFoundEventArgs e)
        {
            if (FileFound != null)
                FileFound(this, e);
        }

        /// <summary>
        /// 指定要搜索的文件类型扩展名默认是*.*。（格式必须是："*.exe"）
        /// </summary>
        public string Extension { get; set; }
    }

    /// <summary>
    /// 为文件搜索器搜索到文件时的事件处理函数提供数据。
    /// </summary>
    public class FileFoundEventArgs : EventArgs
    {
        /// <summary>
        /// 获取一个值，表示当前定位的路径是否是父文件夹。
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// 获取当前定位的文件全路径。
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFoundEventArgs"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="isParent">if set to <c>true</c> [is parent].</param>
        public FileFoundEventArgs(string filename, bool isParent)
        {
            this.FileName = filename;
            this.IsParent = isParent;
        }
    }
}


