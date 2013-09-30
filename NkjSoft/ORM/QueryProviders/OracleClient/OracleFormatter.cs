//--------------------------文档信息----------------------------
//       
//                 文件名: OracleFormatter                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.ORM.QueryProviders.MySqlClient
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/19 17:44:10
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.ORM.Data.Common;

namespace NkjSoft.ORM.QueryProviders.OracleClient
{
    /// <summary>
    /// 
    /// </summary>
    public class OracleFormatter : SqlFormatter
    {
        OracleLanguage oracleLanguage;
        /// <summary>
        /// Initializes a new instance of the <see cref="OracleFormatter"/> class.
        /// </summary>
        /// <param name="oracleLanguage">The oracle language.</param>
        public OracleFormatter(OracleLanguage oracleLanguage)
            : base(oracleLanguage)
        {

        }
    }
}
