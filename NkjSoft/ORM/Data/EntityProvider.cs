// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NkjSoft.ORM.Data
{
    using Common;
    using Mapping;
    using NkjSoft.ORM.Core;

    /// <summary>
    /// 表示一个建立在 <see cref="System.Data.Common.DbConnection"/> 之上，实现 LINQ IQueryable 接口的查询提供程序，提供对特定对象集进行查询的能力的类。
    /// </summary>
    public abstract class EntityProvider : QueryProvider, IEntityProvider, ICreateExecutor
    {
        QueryLanguage language;
        QueryMapping mapping;
        QueryPolicy policy;
        TextWriter log;
        Dictionary<MappingEntity, IEntityTable> tables;
        QueryCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityProvider"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="policy">The policy.</param>
        public EntityProvider(QueryLanguage language, QueryMapping mapping, QueryPolicy policy)
        {
            if (language == null)
                throw new InvalidOperationException("Language not specified");
            if (mapping == null)
                throw new InvalidOperationException("Mapping not specified");
            if (policy == null)
                throw new InvalidOperationException("Policy not specified");
            this.language = language;
            this.mapping = mapping;
            this.policy = policy;
            this.tables = new Dictionary<MappingEntity, IEntityTable>();
        }

        /// <summary>
        /// 获取对象的 <see cref="NkjSoft.ORM.Data.Common.QueryMapping"/> 映射信息。
        /// </summary>
        /// <value>The mapping.</value>
        public QueryMapping Mapping
        {
            get { return this.mapping; }
        }

        /// <summary>
        /// 获取数据提供程序使用的 <see cref="NkjSoft.ORM.Data.Common.QueryLanguage"/> 查询语言对象。
        /// </summary>
        /// <value>The language.</value>
        public QueryLanguage Language
        {
            get { return this.language; }
        }

        /// <summary>
        /// 获取或设置使用的 <see cref="NkjSoft.ORM.Data.Common.QueryPolicy"/> 查询策略对象。
        /// </summary>
        /// <value>The policy.</value>
        public QueryPolicy Policy
        {
            get { return this.policy; }

            set
            {
                if (value == null)
                {
                    this.policy = QueryPolicy.Default;
                }
                else
                {
                    this.policy = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置提供程序在执行查询时提供的日志文本输出内容。
        /// </summary>
        /// <value>The log.</value>
        public TextWriter Log
        {
            get { return this.log; }
            set { this.log = value; }
        }

        /// <summary>
        /// 获取或设置查询缓存。
        /// </summary>
        /// <value>The cache.</value>
        public QueryCache Cache
        {
            get { return this.cache; }
            set { this.cache = value; }
        }

        /// <summary>
        /// 获取实体所映射的表。
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public IEntityTable GetTable(MappingEntity entity)
        {
            IEntityTable table;
            if (!this.tables.TryGetValue(entity, out table))//缓存
            {
                table = this.CreateTable(entity);
                this.tables.Add(entity, table);
            }
            return table;
        }

        /// <summary>
        /// 创建映射实体表。
        /// </summary>
        /// <param name="entity">映射的实体类型</param>
        /// <returns></returns>
        protected virtual IEntityTable CreateTable(MappingEntity entity)
        {
            return (IEntityTable)Activator.CreateInstance(
                typeof(EntityTable<>).MakeGenericType(entity.ElementType),
                new object[] { this, entity }
                );
        }


        /// <summary>
        /// 获取 强类型 <typeparamref name="T"/> 的 <see cref="NkjSoft.ORM.Core.IEntityTable"/>对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEntityTable<T> GetTable<T>()
        {
            return GetTable<T>(null);
        }

        /// <summary>
        /// 根据表ID获取映射的 <typeparamref name="T"/> 强类型的 <see cref="NkjSoft.ORM.Core.IEntityTable"/>对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">The table id.</param>
        /// <returns></returns>
        public virtual IEntityTable<T> GetTable<T>(string tableId)
        {
            return (IEntityTable<T>)this.GetTable(typeof(T), tableId);
        }

        /// <summary>
        /// 根据 CLR 类型获取 <typeparamref name="T"/> 强类型的 <see cref="NkjSoft.ORM.Core.IEntityTable"/>对象。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public virtual IEntityTable GetTable(Type type)
        {
            return GetTable(type, null);
        }

        /// <summary>
        /// 根据 CLR 类型和表 ID 获取 <typeparamref name="T"/> 强类型的 <see cref="NkjSoft.ORM.Core.IEntityTable"/>对象。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tableId">The table id.</param>
        /// <returns></returns>
        public virtual IEntityTable GetTable(Type type, string tableId)
        {
            return this.GetTable(this.Mapping.GetEntity(type, tableId));
        }

        /// <summary> 
        /// 判断一个表达式实例的参数对象是否能被映射到当前特定的表达式树种。
        /// </summary>
        /// <param name="expression">表达式实例.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can be evaluated locally] the specified expression; otherwise, <c>false</c>.
        /// </returns>
        public bool CanBeEvaluatedLocally(Expression expression)
        {
            return this.Mapping.CanBeEvaluatedLocally(expression);
        }

        /// <summary>
        /// Determines whether this instance [can be parameter] the specified expression.
        /// 判断一个表达式实例的参数对象是否能被实现为参数化，进而生成实现参数化查询表达式。
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can be parameter] the specified expression; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanBeParameter(Expression expression)
        {
            Type type = TypeHelper.GetNonNullableType(expression.Type);
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    if (expression.Type == typeof(Byte[]) ||
                        expression.Type == typeof(Char[]))
                        return true;
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Creates the executor.
        /// </summary>
        /// <returns></returns>
        protected abstract QueryExecutor CreateExecutor();

        /// <summary>
        /// 创建查询执行器。
        /// </summary>
        /// <returns></returns>
        QueryExecutor ICreateExecutor.CreateExecutor()
        {
            return this.CreateExecutor();
        }

        /// <summary>
        /// 表示数据库对象到 CLR 对象的<typeparamref name="T"/> 映射类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class EntityTable<T> : Query<T>, IEntityTable<T>, IHaveMappingEntity
        {
            MappingEntity entity;
            EntityProvider provider;

            /// <summary>
            /// 实例化一个新的 <see cref="EntityTable&lt;T&gt;"/> 类对象。
            /// </summary>
            /// <param name="provider">数据查询提供程序</param>
            /// <param name="entity">实体</param>
            public EntityTable(EntityProvider provider, MappingEntity entity)
                : base(provider, typeof(IEntityTable<T>))
            {
                this.provider = provider;
                this.entity = entity;

            }

            /// <summary>
            /// 获取当前映射的 <see cref="NkjSoft.ORM.Data.Common.MappingEntity"/> 实体对象。
            /// </summary>
            /// <value>The entity.</value>
            public MappingEntity Entity
            {
                get { return this.entity; }
            }

            /// <summary>
            /// 获取当前数据查询提供程序类型。
            /// </summary>
            /// <value>The provider.</value>
            new public IEntityProvider Provider
            {
                get { return this.provider; }
            }

            /// <summary>
            /// 获取表映射得到的 <see cref="System.String"/> ID值。
            /// </summary>
            /// <value>The table id.</value>
            public string TableId
            {
                get { return this.entity.TableId; }
            }

            /// <summary>
            /// 获取当前映射的 CLR 类型。
            /// </summary>
            /// <value>The type of the entity.</value>
            public Type EntityType
            {
                get { return this.entity.EntityType; }
            }

            /// <summary>
            /// 通过ID获取 <typeparamref name="T"/> 对象。
            /// </summary>
            /// <param name="id">The id.</param>
            /// <returns></returns>
            public T GetById(object id)
            {
                var dbProvider = this.Provider;
                if (dbProvider != null)
                {
                    IEnumerable<object> keys = id as IEnumerable<object>;
                    if (keys == null)
                        keys = new object[] { id };
                    Expression query = ((EntityProvider)dbProvider).Mapping.GetPrimaryKeyQuery(this.entity, this.Expression, keys.Select(v => Expression.Constant(v)).ToArray());
                    return this.Provider.Execute<T>(query);
                }
                return default(T);
            }

            /// <summary>
            /// Gets the by id.
            /// </summary>
            /// <param name="id">The id.</param>
            /// <returns></returns>
            object IEntityTable.GetById(object id)
            {
                return this.GetById(id);
            }

            /// <summary>
            /// 向数据库插入一个经过从 CLR 到 数据库转换的实体对象。返回一个值，表示受影响的行数。
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <returns></returns>
            public int Insert(T instance)
            {
                return Updatable.Insert(this, instance);
            }

            /// <summary>
            ///  向数据库插入一个经过从 CLR 到 数据库转换的实体对象。返回一个值，表示受影响的行数。
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <returns></returns>
            int IEntityTable.Insert(object instance)
            {
                return this.Insert((T)instance);
            }

            /// <summary>
            /// 从数据库中删除一个映射的 CLR 到 数据库转换的实体对象。返回一个值，表示受影响的行数。
            /// </summary>
            /// <param name="instance">实体</param>
            /// <returns></returns>
            public int Delete(T instance)
            {
                return Updatable.Delete(this, instance);
            }

            ///// <summary>
            ///// 从数据库中删除一个映射的 CLR 到 数据库转换的实体对象。返回一个值，表示受影响的行数。
            ///// </summary>
            ///// <param name="instance">The instance.</param>
            ///// <returns></returns>
            //int IEntityTable.Delete(object instance)
            //{
            //    return this.Delete((T)instance);
            //}

            /// <summary>
            /// 对数据库中的一个映射的 CLR 到 数据库转换的实体对象信息进行更新。返回一个值，表示受影响的行数。
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <returns></returns>
            public int Update(T instance)
            {
                return Updatable.Update(this, instance);
            }

            /// <summary>
            /// 对数据库中的一个映射的 CLR 到 数据库转换的实体对象信息进行更新。返回一个值，表示受影响的行数。
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <returns></returns>
            int IEntityTable.Update(object instance)
            {
                return this.Update((T)instance);
            }

            /// <summary>
            /// 向数据库插入或者更新一个已经存在的记录的信息，提供程序自动判断操作。
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <returns></returns>
            public int InsertOrUpdate(T instance)
            {
                return Updatable.InsertOrUpdate(this, instance);
            }

            /// <summary>
            ///  向数据库插入或者更新一个已经存在的记录的信息，提供程序自动判断操作。
            /// </summary>
            /// <param name="instance">实体</param>
            /// <returns></returns>
            int IEntityTable.InsertOrUpdate(object instance)
            {
                return this.InsertOrUpdate((T)instance);
            }
        }

        /// <summary>
        /// 获取 Lambda 表达式查询的 <see cref="System.String"/> 的一份文本。
        /// </summary>
        /// <param name="expression">Lambda查询表达式</param>
        /// <returns></returns>
        public override string GetQueryText(Expression expression)
        {
            Expression plan = this.GetExecutionPlan(expression);
            var commands = CommandGatherer.Gather(plan).Select(c => c.CommandText).ToArray();
            return string.Join("\n\n", commands);
        }

        /// <summary>
        /// 命令收集器。
        /// </summary>
        class CommandGatherer : DbExpressionVisitor
        {
            List<QueryCommand> commands = new List<QueryCommand>();

            /// <summary>
            /// Gathers the specified expression.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static ReadOnlyCollection<QueryCommand> Gather(Expression expression)
            {
                var gatherer = new CommandGatherer();
                gatherer.Visit(expression);
                return gatherer.commands.AsReadOnly();
            }

            protected override Expression VisitConstant(ConstantExpression c)
            {
                QueryCommand qc = c.Value as QueryCommand;
                if (qc != null)
                {
                    this.commands.Add(qc);
                }
                return c;
            }
        }

        /// <summary>
        /// 从指定的 Lambda 表达式获取查询计划。
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public string GetQueryPlan(Expression expression)
        {
            Expression plan = this.GetExecutionPlan(expression);
            return DbExpressionWriter.WriteToString(this.Language, plan);
        }

        /// <summary>
        /// 创建翻译器。
        /// </summary>
        /// <returns></returns>
        protected virtual QueryTranslator CreateTranslator()
        {
            return new QueryTranslator(this.language, this.mapping, this.policy);
        }

        /// <summary>
        /// 对某个操作进行事务内的操作。
        /// </summary>
        /// <param name="action">操作</param>
        public abstract void DoTransacted(Action action);
        /// <summary>
        /// 对 DbConnnection 连接进行指定的操作。
        /// </summary>
        /// <param name="action">The action.</param>
        public abstract void DoConnected(Action action);
        /// <summary>
        /// 执行一段查询文本,返回受影响的行数。
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public abstract int ExecuteCommand(string commandText);

        /// <summary> 
        /// 执行一个 Lambda 表达式查询，系统会对表达式进行编译之后执行。
        /// </summary>
        /// <param name="expression">需要执行的 Lambda 表达式查询</param>
        /// <returns></returns>
        public override object Execute(Expression expression)
        {
            LambdaExpression lambda = expression as LambdaExpression;

            if (lambda == null && this.cache != null && expression.NodeType != ExpressionType.Constant)
            {
                return this.cache.Execute(expression);
            }

            Expression plan = this.GetExecutionPlan(expression);

            if (lambda != null)
            {
                // compile & return the execution plan so it can be used multiple times
                LambdaExpression fn = Expression.Lambda(lambda.Type, plan, lambda.Parameters);
#if NOREFEMIT
                    return ExpressionEvaluator.CreateDelegate(fn);
#else
                return fn.Compile();
#endif
            }
            else
            {
                // compile the execution plan and invoke it
                Expression<Func<object>> efn = Expression.Lambda<Func<object>>(Expression.Convert(plan, typeof(object)));
#if NOREFEMIT
                    return ExpressionEvaluator.Eval(efn, new object[] { });
#else
                Func<object> fn = efn.Compile();
                return fn();
#endif
            }
        }

        /// <summary> 
        /// 将指定的 Lambda 查询表达式转换成一个执行计划。
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Expression GetExecutionPlan(Expression expression)
        {
            // strip off lambda for now
            LambdaExpression lambda = expression as LambdaExpression;
            if (lambda != null)
                expression = lambda.Body;

            QueryTranslator translator = this.CreateTranslator();

            // 将查询翻译成具体数据库适用的表达式
            Expression translation = translator.Translate(expression);

            var parameters = lambda != null ? lambda.Parameters : null;
            Expression provider = this.Find(expression, parameters, typeof(EntityProvider));
            if (provider == null)
            {
                Expression rootQueryable = this.Find(expression, parameters, typeof(IQueryable));
                provider = Expression.Property(rootQueryable, typeof(IQueryable).GetProperty("Provider"));
            }
            //return translation;
            return translator.Police.BuildExecutionPlan(translation, provider);
        }

        /// <summary>
        /// 在参数表达式列表中查找特定类型的查询表达式。
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private Expression Find(Expression expression, IList<ParameterExpression> parameters, Type type)
        {
            if (parameters != null)
            {
                Expression found = parameters.FirstOrDefault(p => type.IsAssignableFrom(p.Type));
                if (found != null)
                    return found;
            }
            return TypedSubtreeFinder.Find(expression, type);
        }

        /// <summary>
        /// 获取映射信息。
        /// </summary>
        /// <param name="mappingId">The mapping id.</param>
        /// <returns></returns>
        public static QueryMapping GetMapping(string mappingId)
        {
            if (mappingId != null)
            {
                Type type = FindLoadedType(mappingId);
                if (type != null)
                {
                    return new AttributeMapping(type);
                }
            }
            return new ImplicitMapping();
        }

        /// <summary>
        /// 通过对象查询提供程序的 <see cref="System.String"/> 名称获取对象查询提供程序的 <see cref="System.Type"/> 信息。
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        public static Type GetProviderType(string providerName)
        {
            if (!string.IsNullOrEmpty(providerName))
            {
                var type = FindInstancesIn(typeof(EntityProvider), providerName).FirstOrDefault();
                if (type != null)
                    return type;
            }
            return null;
        }

        /// <summary>
        /// 查找一个类型的反射类型信息。
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        private static Type FindLoadedType(string typeName)
        {
            foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assem.GetType(typeName, false, true);
                if (type != null)
                    return type;
            }
            return null;
        }

        /// <summary>
        /// 在指定的程序集中查找特定类型实例。
        /// </summary>
        /// <param name="type">查询的类型</param>
        /// <param name="assemblyName">程序集 <see cref="System.String"/> 名称.</param>
        /// <returns></returns>
        private static IEnumerable<Type> FindInstancesIn(Type type, string assemblyName)
        {
            Assembly assembly = GetAssemblyForNamespace(assemblyName);
            if (assembly != null)
            {
                foreach (var atype in assembly.GetTypes())
                {
                    if (string.Compare(atype.Namespace, assemblyName, true) == 0
                        && type.IsAssignableFrom(atype))
                    {
                        yield return atype;
                    }
                }
            }
        }

        /// <summary>
        /// 在指定的程序集中查找特定类型实例。
        /// </summary>
        /// <param name="typeFullName">Full name of the type.</param>
        /// <param name="asm">The asm.</param>
        /// <returns></returns>
        public static Type FindInstancesIn(string typeFullName, Assembly asm)
        {
            Assembly assembly = asm;
            if (assembly != null && !string.IsNullOrEmpty(typeFullName))
            {
                return assembly.GetType(typeFullName);
            }
            return null;
        }

        /// <summary>
        /// 从命名空间里加载程序集
        /// </summary>
        /// <param name="nspace">The nspace.</param>
        /// <returns></returns>
        private static Assembly GetAssemblyForNamespace(string nspace)
        {
            foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assem.FullName.Contains(nspace))
                {
                    return assem;
                }
            }
            return null;
        }


        /// <summary>
        /// 根据名字,动态加载得到一个 <see cref="System.Reflection.Assembly"/> 对象。
        /// </summary>
        /// <param name="name">指定需要的加载的程序集名称。</param>
        /// <exception cref="System.ArgumentNullException">assemblyFile 为 null。</exception>
        /// <exception cref="System.IO.FileNotFoundException">未找到 assemblyFile，或者试图加载的模块没有指定文件扩展名</exception>  
        /// <returns></returns>
        public static Assembly Load(string name)
        {
            // try to load it.
            try
            {
                //if (!File.Exists(name))
                //    return null;
                return Assembly.LoadFrom(name);
            }
            catch (Exception e)
            {
                throw e;
            }
            // return null;
        }
    }
}
