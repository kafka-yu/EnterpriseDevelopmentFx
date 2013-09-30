//--------------------------版权信息----------------------------
//       
//                 文件名: XmlReaderWriter.cs                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: NkjSoft.Common.IO
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2009/7/10 18:01:24
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------

using System;
using System.Data;
using System.Xml;
using System.IO;

namespace NkjSoft.Common.IO
{
    /// <summary>
    /// 封装 了 操作 XML 文件 的方法。无法继承此类。
    /// </summary>
    public sealed class XmlReaderWriter
    {
        #region --- 构造函数 ---

        /// <summary>
        ///构造新的 <see cref="XmlReaderWriter"/> 对象
        /// </summary>
        public XmlReaderWriter()
        {
            //实例 xml
            doc = new XmlDocument();
        }

        /// <summary>
        /// 构造新的 <see cref="XmlReaderWriter"/> 对象
        /// </summary>
        /// <param name="xmlPath">The XML path.</param>
        public XmlReaderWriter(string xmlPath)
        {
            _xmlPath = xmlPath;

            //实例 xml
            doc = new XmlDocument();
        }

        #endregion

        #region --- 私有变量 ---
        /// <summary>
        /// 类操作的 xml文件地址
        /// </summary>
        private string _xmlPath;

        //实例 xml
        XmlDocument doc;
        #endregion ---

        #region --- 成员属性 ---
        /// <summary>
        /// 获取或者设置当前实例 的 ConfigFile 文件 
        /// </summary>
        public string ConfigFilePath
        {
            get { return _xmlPath; }
            set { _xmlPath = value; }
        }
        #endregion

        #region --- 私有方法 ---

        #endregion ----

        #region --- 公共方法 ----
        // 使用 程序 新  字符串  代替旧的 .
        /// <summary>
        /// 使用 程序 新  字符串  代替旧的[只更新第二个属性的值].
        /// </summary>
        /// <param name="key">需要更新值的 配置节</param>
        /// <param name="_configerSession">需要更新值的 配置节 的第一个属性名</param>
        /// <param name="_SecondAttName">第二个属性名</param>
        /// <param name="newValue">第二个属性 的新值</param>
        /// <exception cref="System.NullReferenceException">没有指定要操作的XML文件路径XmlPath</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件不存在</exception>
        /// <exception cref="System.Xml.XmlException">XML文件操作异常</exception>
        /// <returns></returns>
        public bool SaveConfig(string key, string _configerSession, string _SecondAttName, string newValue)
        {
            if (string.IsNullOrEmpty(_xmlPath))
                throw new NullReferenceException("没有指定要操作的XML文件路径XmlPath");
            if (!File.Exists(_xmlPath))
                throw new FileNotFoundException("文件不存在", _xmlPath);
            //返回值
            bool __returnValue = false;

            //实例 xml
            doc.Load(_xmlPath);
            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性
                XmlAttribute att = nodes[i].Attributes[key];
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att != null && att.Value == _configerSession)
                {
                    //对目标元素中的第二个属性赋值
                    att = nodes[i].Attributes[_SecondAttName];
                    att.Value = newValue;
                    break;
                }
                else
                    continue;
            }
            try
            {
                //保存上面的修改
                doc.Save(_xmlPath);
                __returnValue = true;
            }
            catch (XmlException ex)
            {
                __returnValue = false;
                throw ex;//("读取文件出错", "试图保存 " + _xmlPath + " 文件出错!\r\n\r\n请重新启动 程序 !");
            }

            return __returnValue;
        }

        #region --- 读取ＸML 到 DataSet中
        /// <summary>
        /// 读取ＸML 到 DataSet中
        /// </summary>
        /// <param name="_xmlPath">ＸML 路径</param>
        /// <returns></returns>
        public DataSet GetXmlDataView()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(_xmlPath);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                else
                    return null;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取ＸML 到 DataSet中
        /// </summary>
        /// <param name="xmlPath">ＸML 路径</param>
        /// <returns></returns>
        public DataSet GetXmlDataView(string xmlPath)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlPath);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //读取 xml 文件 某个配置节 的 属性值 
        /// <summary>
        /// 读取 xml 文件 某个配置节 的 属性值 
        /// </summary>
        /// <param name="key">需要读取值的 配置节</param>
        /// <param name="_configerSession">需要读取值的 配置节 的第一个属性名</param>
        /// <param name="_SecondAttName">第二个属性名</param>
        /// <returns></returns>
        public string GetXmlAttributeValueByName(string key, string _configerSession, string _SecondAttName)
        {
            //返回值
            string __returnString = string.Empty;

            //加载 xml 文件
            doc.Load(_xmlPath);

            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性 第一属性名
                XmlAttribute att = nodes[i].Attributes[key];

                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att != null && att.Value == _configerSession)
                {
                    //对目标元素中的第二个属性赋值
                    att = nodes[i].Attributes[_SecondAttName];
                    __returnString = att.Value;
                    break;
                }
                else
                    continue;
            }
            return __returnString;
        }

        #endregion

        #region --- WriteXmlByDataSet
        /// <summary>
        /// 增加记录 到 XML 文件中
        /// </summary>
        /// <param name="Columns">字段名</param>
        /// <param name="ColumnValue">字段值</param>
        /// <returns></returns>
        public bool WriteXmlByDataSet(string Columns, string ColumnValue)
        {
            try
            {
                //根据传入的XML路径 文件
                DataSet ds = GetXmlDataView(_xmlPath);
                //读xml架构，关系到列的数据类型 
                if (ds != null)
                {
                    DataTable dt = ds.Tables[0];

                    //在原来的表格基础上创建新行 
                    DataRow newRow = dt.NewRow();

                    //循环给一行中的各个列赋值 
                    for (int i = 0; i < Columns.Length; i++)
                    {
                        newRow[0] = Columns;
                        newRow[1] = ColumnValue;
                    }
                    dt.Rows.Add(newRow);
                    dt.AcceptChanges();
                    ds.AcceptChanges();
                    ds.WriteXml(_xmlPath);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ----DeleteXmlRowByIndex

        /// <summary>
        /// 通过删除DataSet中iDeleteRow这一行，然后重写Xml以实现删除指定行 
        /// </summary>
        /// <param name="iDeleteRow">要删除的行在DataSet中的Index值 </param>
        /// <returns></returns>
        public bool DeleteXmlRowByIndex(int iDeleteRow)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(_xmlPath);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //删除符号条件的行 
                    ds.Tables[0].Rows[iDeleteRow].Delete();
                }
                ds.WriteXml(_xmlPath);
                return true;
            }
            catch (System.IO.IOException ex)
            {
                throw new System.IO.IOException(ex.Message);
            }
        }
        #endregion

        #region ---根据列值 删除Xml 记录DeleteXmlRows
        /// <summary>
        /// 根据列值 删除Xml 记录DeleteXmlRows 
        /// </summary>
        /// <param name="strColumn">列名 </param>
        /// <param name="ColumnValue">strColumn列中值为ColumnValue的行均会被删除 </param>
        /// <returns></returns>
        public bool DeleteXmlRows(string strColumn, string[] ColumnValue)
        {
            try
            {
                DataSet ds = new DataSet();

                ds.ReadXml(_xmlPath);
                //先判断行数 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //判断行多还是删除的值多，多的for循环放在里面 
                    if (ColumnValue.Length > ds.Tables[0].Rows.Count)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ColumnValue.Length; j++)
                            {
                                if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < ColumnValue.Length; j++)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                            }
                        }
                    }
                    ds.WriteXml(_xmlPath);
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  ---删除所有行 DeleteXmlAllRows
        /// <summary>
        /// 删除所有行 
        /// </summary>
        /// <param name="filter"> 需要保留的行 如:"系统模版[筛选[物理学与电子信息工程系,本科]";</param>
        /// <returns></returns>
        public bool DeleteXmlAllRows(params string[] filter)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(_xmlPath);

                DataRow dr = null;

                //判断是否 需要保留行 
                if (filter != null)
                {
                    dr = ds.Tables[0].NewRow();

                    //注意这里的行 结构必须与 ds 内 的 一致
                    for (int i = 0; i < filter.Length; i++)
                        dr[i] = filter[i].ToString();
                }
                //如果记录条数大于0 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //移除所有记录 
                    ds.Tables[0].Rows.Clear();
                }

                ds.WriteXml(_xmlPath);

                //重新写入，这时XML文件中就只剩根节点了 
                //保存一条系统默认模版
                if (dr != null)
                {
                    ds.Tables[0].Rows.Add(dr);

                    ds.AcceptChanges();

                    //重新 写回  XML
                    ds.WriteXml(_xmlPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #endregion
    }
}
