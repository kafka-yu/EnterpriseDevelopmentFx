//--------------------------文档信息----------------------------
//       
//                 文件名: OracleLanguage                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.ORM.QueryProviders.OracleClient
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/19 17:46:10
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.ORM.Data.Common;

namespace NkjSoft.ORM.QueryProviders.OracleClient
{
    public class OracleLanguage : QueryLanguage
    {
        public override QueryTypeSystem TypeSystem
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Linq.Expressions.Expression GetGeneratedIdExpression(System.Reflection.MemberInfo member)
        {
            throw new NotImplementedException();
        }
    }
}
