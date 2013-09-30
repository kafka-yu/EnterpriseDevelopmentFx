//--------------------------文档信息----------------------------
//       
//                 文件名: Serializer.cs                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Common.IO
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/5/11 21:21:46
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
namespace NkjSoft.Common.IO
{
    /// <summary>
    /// 表示一个泛型序列化器。无法继承此类。
    /// </summary>
    public sealed class Serializer
    {
        /// <summary>
        /// 将指定的文件流通过 <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> 序列化器得到 <typeparamref name="TResult"/> 对象图。
        /// </summary>
        /// <typeparam name="TResult">需要的类型</typeparam>
        /// <param name="filePath">指定包含需要系列化的文件路径，提供文件流。</param>
        /// <exception cref="System.ArgumentNullException">Null</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception> 
        /// <returns>需要的对象类型。</returns> 
        public static TResult DeSerialize<TResult>(string filePath)
        {
            //   ///<exception cref="System.IO.FileNotFoundException">文件不存在</exception>
            if (!File.Exists(filePath))
            {
                //throw new FileNotFoundException("文件不存在。", filePath);
                return default(TResult);
            }
            BinaryFormatter bf = new BinaryFormatter();
            object o = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                try
                {
                    o = bf.Deserialize(fs);
                }
                catch (System.ArgumentNullException ae)
                { throw ae; }
                catch (System.Runtime.Serialization.SerializationException se)
                { throw se; }
                catch (System.Security.SecurityException sse)
                { throw sse; }
                fs.Close();
            }
            if (o == null)
                return default(TResult);
            return (TResult)o;
        }

        /// <summary>
        /// 将指定的 <typeparamref name="TSource"/> 类型对象通过 <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> 序列化器 ,保存到指定的文件路径文件中。
        /// </summary>
        /// <typeparam name="TSource">指定的需要被序列化的类型名称</typeparam>
        /// <param name="source">指定的需要被序列化的类型的对象</param>
        /// <param name="filePath">指定的文件路径</param>
        /// <exception cref="System.ArgumentNullException">Null</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public static void Serialize<TSource>(TSource source, string filePath)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                try
                {
                    bf.Serialize(fs, source);
                }
                catch (System.ArgumentNullException ae)
                { throw ae; }
                catch (System.Runtime.Serialization.SerializationException se)
                { throw se; }
                catch (System.Security.SecurityException sse)
                { throw sse; }
                fs.Close();
            }
        }

        /// <summary>
        /// 将指定的 <typeparamref name="TSource"/> 类型对象通过 <see cref=" System.Xml.Serialization.XmlSerializer"/> 序列化器 ,保存到指定的XML文件中。
        /// </summary>
        /// <typeparam name="TSource">指定的需要被系列化的对象的类型</typeparam>
        /// <param name="source">对象</param>
        /// <param name="filePath">文件路径</param>
        public static void XmlSerizlize<TSource>(TSource source, string filePath)
        {
            XmlSerializer xmler = new XmlSerializer(typeof(TSource));
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                try
                { 
                    xmler.Serialize(fs, source);
                }
                catch (System.ArgumentNullException ae)
                { throw ae; }
                catch (System.Runtime.Serialization.SerializationException se)
                { throw se; }
                catch (System.Security.SecurityException sse)
                { throw sse; }
                fs.Close();
            }
        }

        /// <summary>
        /// 将指定的文件流通过 <see cref=" System.Xml.Serialization.XmlSerializer"/> 序列化器得到 <typeparamref name="TResult"/> 对象图。
        /// </summary>
        /// <typeparam name="TResult">需要的类型</typeparam>
        /// <param name="filePath">指定包含需要系列化的文件路径，提供文件流。</param>
        /// <exception cref="System.ArgumentNullException">Null</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception> 
        /// <returns>需要的对象类型。</returns> 
        public static TResult  XmlDeSerialize<TResult>(string filePath)
        {
            //   ///<exception cref="System.IO.FileNotFoundException">文件不存在</exception>
            if (!File.Exists(filePath))
            {
                //throw new FileNotFoundException("文件不存在。", filePath);
                return default(TResult);
            }
            XmlSerializer xmler = new XmlSerializer(typeof(TResult));
            object o = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                try
                {
                    o = xmler.Deserialize(fs);
                }
                catch (System.ArgumentNullException ae)
                { throw ae; }
                catch (System.Runtime.Serialization.SerializationException se)
                { throw se; }
                catch (System.Security.SecurityException sse)
                { throw sse; }
                fs.Close();
            }
            if (o == null)
                return default(TResult);
            return (TResult)o;
        }
    }
}
