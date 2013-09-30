//--------------------------文档信息----------------------------
//       
//                 文件名: GZip                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Common.IO
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/19 2:43:52
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace NkjSoft.Common.IO
{
    //源自 博客园..
    /// <summary>
    /// 对文件和字符串压缩及解压缩的类。无法继承此类。
    /// </summary>
    public sealed class GZip
    {
        /// <summary>
        /// 对字符串进行压缩
        /// </summary>
        /// <param name="str">待压缩的字符串</param>
        /// <returns>压缩后的字符串</returns>
        public static string CompressString(string str)
        {
            string compressString = "";
            byte[] compressBeforeByte = Encoding.Default.GetBytes(str);
            byte[] compressAfterByte = GZip.Compress(compressBeforeByte);
            compressString = Encoding.Default.GetString(compressAfterByte);
            return compressString;
        }
        /// <summary>
        /// 对字符串进行解压缩
        /// </summary>
        /// <param name="str">待解压缩的字符串</param>
        /// <returns>解压缩后的字符串</returns>
        public static string DecompressString(string str)
        {
            string compressString = "";
            byte[] compressBeforeByte = Encoding.Default.GetBytes(str);
            byte[] compressAfterByte = GZip.Decompress(compressBeforeByte);
            compressString = Encoding.Default.GetString(compressAfterByte);
            return compressString;
        }
        /// <summary>
        /// 对文件进行压缩[TODO:暂未实现]
        /// </summary>
        /// <param name="sourceFile">待压缩的文件名</param>
        /// <param name="destinationFile">压缩后的文件名</param>
        public static void CompressFile(string sourceFile, string destinationFile)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 对文件进行解压缩[TODO:暂未实现]
        /// </summary>
        /// <param name="sourceFile">待解压缩的文件名</param>
        /// <param name="destinationFile">解压缩后的文件名</param>
        /// <returns></returns>
        public static void DecompressFile(string sourceFile, string destinationFile)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 对byte数组进行压缩
        /// </summary>
        /// <param name="data">待压缩的byte数组</param>
        /// <returns>压缩后的byte数组</returns>
        public static byte[] Compress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 对byte数组进行解压缩
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
