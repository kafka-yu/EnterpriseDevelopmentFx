//--------------------------文档信息----------------------------
//       
//                 文件名: IOExtensions                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Extensions.IO
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/1 16:35:33
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.IO;
using System.Net;

namespace NkjSoft.Framework.Extensions
{
    /// <summary>
    /// 对 System.IO 的扩展。
    /// </summary>
    public static class IOExtensions
    {
        /// <summary>
        /// 读取Web 页面的内容..
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static string ReadWebPage(string url)
        {
            string webPage;
            WebRequest request = WebRequest.Create(url);
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                webPage = sr.ReadToEnd();
                sr.Close();
            }
            return webPage;
        }

        /// <summary>
        /// 获取当前包含路径信息的目录下的所有文件全路径信息数组。
        /// <para>
        ///    必须确保调用的字符串是合法的路径信息,否则会引发异常! 
        /// </para>
        /// </summary>
        /// <param name="path">文件目录路径字符串</param>
        /// <returns>路径下的所有文件全路径信息数组</returns>
        /// <exception cref="System.IO.IOException" >path 必须是一个文件名。</exception>
        ///<exception cref="System.UnauthorizedAccessException" >调用方没有所要求的权限。</exception>
        ///<exception cref="System.ArgumentException" >path 是一个零长度字符串，仅包含空白或者包含一个或多个由 System.IO.Path.InvalidPathChars 定义的无效字符。</exception>
        ///<exception cref="System.ArgumentNullException" > path 为 null。</exception>
        ///  <exception cref="System.IO.PathTooLongException" > 指定的路径、文件名或者两者都超出了系统定义的最大长度。例如，在基于 Windows 的平台上，路径必须小于 248 个字符，文件名必须小于 260个字符。</exception>
        ///<exception cref="System.IO.DirectoryNotFoundException" > 指定的路径无效（例如，它位于未映射的驱动器上）。</exception>
        public static string[] GetFiles(this string path)
        {
            if (path.IsNullOrEmpty())
                throw new ArgumentNullException("path", "路径不合法!为空,或者不是路径信息!");
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// 从当前 路径读取文本。
        /// </summary>
        /// <param name="absolutePath">包含文件的绝对路径</param>
        /// <returns>String containing the content of the file.</returns>
        public static string GetFileText(this string absolutePath)
        {
            using (StreamReader sr = new StreamReader(absolutePath))
                return sr.ReadToEnd();
        }

        /// <summary>
        /// 在磁盘上创建当前路径的文件，并将文本写入文件中。
        /// </summary>
        /// <param name="absolutePath">文本文件绝对路径.</param>
        /// <param name="fileText">文件内容</param>
        public static void CreateToFile(this string fileText, string absolutePath)
        {
            using (StreamWriter sw = File.CreateText(absolutePath))
                sw.Write(fileText);
        }

        /// <summary>
        /// 将当前文本内容更新到文件路径指向的文件。
        /// </summary>
        /// <param name="absolutePath">文件绝对路径</param>
        /// <param name="lookFor">将被替换的原文本</param>
        /// <param name="replaceWith">用来代替的文本.</param>
        public static void UpdateFileText(this string absolutePath, string lookFor, string replaceWith)
        {
            string newText = GetFileText(absolutePath).Replace(lookFor, replaceWith);
            WriteToFile(absolutePath, newText);
        }

        /// <summary>
        /// 将文本写入当前文件路径中。
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="fileText">A String containing text to be written to the file.</param>
        public static void WriteToFile(this string absolutePath, string fileText)
        {
            using (StreamWriter sw = new StreamWriter(absolutePath, false))
                sw.Write(fileText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath"></param>
        public static void CreateFolderIfNotExists(string dirPath)
        {
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
        }
    }
}
