// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NkjSoft.ORM.Core;

namespace NkjSoft.ORM.Data.Common
{
    /// <summary>
    /// Defines query execution & materialization policies. 
    /// </summary>
    public class QueryTranslator
    {
        QueryLinguist linguist;
        QueryMapper mapper;
        QueryPolice police;

        public QueryTranslator(QueryLanguage language, QueryMapping mapping, QueryPolicy policy)
        {
            this.linguist = language.CreateLinguist(this);
            this.mapper = mapping.CreateMapper(this);
            this.police = policy.CreatePolice(this);
        }

        public QueryLinguist Linguist
        {
            get { return this.linguist; }
        }

        public QueryMapper Mapper
        {
            get { return this.mapper; }
        }

        public QueryPolice Police
        {
            get { return this.police; }
        }

        /// <summary>
        /// Translates the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual Expression Translate(Expression expression)
        {
            // 准备翻译Lambda 表达式到 到表达式树,进而进行解析
            expression = PartialEvaluator.Eval(expression, this.mapper.Mapping.CanBeEvaluatedLocally);

            // 进行实体映射,获取操作的字段等..
            expression = this.mapper.Translate(expression);

            // 
            //TODO:优化查询
            // 进行参数化查询 设置
            expression = this.police.Translate(expression);

            // 使用具体的数据库查询语言格式化查询语句结果..
            expression = this.linguist.Translate(expression);

            return expression;
        }
    }
}