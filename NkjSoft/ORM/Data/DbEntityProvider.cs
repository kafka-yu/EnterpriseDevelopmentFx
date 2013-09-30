// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
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
    /// 表示数据库实体查询提供程序。
    /// </summary>
    public class DbEntityProvider : EntityProvider
    {
        //TODO:2010-8-22 IEntitySession 
        IEntitySession session = null;

        /// <summary>
        /// 获取查询上下文会话状态。该会话状态管理器提供查询的会话管理,支持延迟执行、延迟加载功能.
        /// </summary>
        /// <value>The session.</value>
        public IEntitySession Session
        {
            get
            {
                if (session == null)
                    session = new EntitySession(this);
                return session;
            }
            private set { session = value; }
        }
        DbConnection connection;
        DbTransaction transaction;
        IsolationLevel isolation = IsolationLevel.ReadCommitted;

        int nConnectedActions = 0;
        bool actionOpenedConnection = false;

        /// <summary>
        /// 根据指定参数，初始化一个新 <see cref="DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="connection">数据库连接对象。</param>
        /// <param name="language">数据查询语言类型。</param>
        /// <param name="mapping">映射方式信息</param>
        /// <param name="policy">查询策略对象信息。</param>
        public DbEntityProvider(DbConnection connection, QueryLanguage language, QueryMapping mapping, QueryPolicy policy)
            : base(language, mapping, policy)
        {
            if (connection == null)
                throw new InvalidOperationException("Connection not specified");
            this.connection = connection;
        }

        /// <summary>
        /// 获取当前使用的 <see cref="System.Data.Common.DbConnection"/> 连接对象。
        /// </summary>
        /// <value>The connection.</value>
        public virtual DbConnection Connection
        {
            get { return this.connection; }
        }

        /// <summary>
        /// 获取当前使用的提供事务处理能力的 <see cref="System.Data.Common.DbTransaction"/> 事务对象。
        /// </summary>
        /// <value>The transaction.</value>
        public virtual DbTransaction Transaction
        {
            get { return this.transaction; }
            set
            {
                if (value != null && value.Connection != this.connection)
                    throw new InvalidOperationException("Transaction does not match connection.");
                this.transaction = value;
            }
        }

        /// <summary>
        /// 获取或设置事务锁行为级别。
        /// </summary>
        /// <value>The isolation.</value>
        public IsolationLevel Isolation
        {
            get { return this.isolation; }
            set { this.isolation = value; }
        }

        /// <summary>
        /// 根据连接对象、映射信息、查询策略新建一个 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public virtual DbEntityProvider New(DbConnection connection, QueryMapping mapping, QueryPolicy policy)
        {
            return (DbEntityProvider)Activator.CreateInstance(this.GetType(), new object[] { connection, mapping, policy });
        }

        /// <summary>
        /// 根据连接对象新建一个 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public virtual DbEntityProvider New(DbConnection connection)
        {
            var n = New(connection, this.Mapping, this.Policy);
            n.Log = this.Log;
            return n;
        }

        /// <summary>
        /// 根据映射信息对象新建一个 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public virtual DbEntityProvider New(QueryMapping mapping)
        {
            var n = New(this.Connection, mapping, this.Policy);
            n.Log = this.Log;
            return n;
        }

        /// <summary>
        /// 根据查询策略对象新建一个 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。使用属性指定的其他两个对象信息。
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public virtual DbEntityProvider New(QueryPolicy policy)
        {
            var n = New(this.Connection, this.Mapping, policy);
            n.Log = this.Log;
            return n;
        }
        /// <summary> 
        /// 获取一个值,该值表示当前连接是否处于打开状态。
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [action opened connection]; otherwise, <c>false</c>.
        /// </value>
        protected bool ActionOpenedConnection
        {
            get { return this.actionOpenedConnection; }
        }

        /// <summary>
        /// 打开 DbConnection 连接
        /// </summary>
        protected void StartUsingConnection()
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
                this.actionOpenedConnection = true;
            }
            this.nConnectedActions++;
        }

        /// <summary>
        /// 停止使用一个数据库连接。
        /// </summary>
        protected void StopUsingConnection()
        {
            System.Diagnostics.Debug.Assert(this.nConnectedActions > 0);
            this.nConnectedActions--;
            if (this.nConnectedActions == 0 && this.actionOpenedConnection)
            {
                this.connection.Close();
                this.actionOpenedConnection = false;
            }
        }

        /// <summary>
        /// 在打开数据库连接之后执行指定的对数据库的操作。
        /// </summary>
        /// <param name="action">指定的对数据库的操作</param>
        public override void DoConnected(Action action)
        {
            if (action == null)
                return;
            this.StartUsingConnection();
            try
            {
                action();
            }
            finally
            {
                this.StopUsingConnection();
            }
        }

        /// <summary>
        /// 在打开数据库连接之后，执行事务级别的查询操作。
        /// </summary>
        /// <param name="action">查询操作</param>
        public override void DoTransacted(Action action)
        {
            if (action == null) return;

            this.StartUsingConnection();
            try
            {
                if (this.Transaction == null)
                {
                    var trans = this.Connection.BeginTransaction(this.Isolation);
                    try
                    {
                        this.Transaction = trans;
                        action();
                        trans.Commit();
                    }
                    catch { trans.Rollback(); }
                    finally
                    {
                        this.Transaction = null;
                        trans.Dispose();
                    }
                }
                else
                {
                    action();
                }
            }
            finally
            {
                this.StopUsingConnection();
            }
        }
        /// <summary>
        /// 在打开数据库连接之后，执行事务级别的查询操作。
        /// </summary>
        /// <param name="action">查询操作</param>
        public virtual void DoTransacted(Action<DbTransaction> action)
        {
            if (action == null) return;

            this.StartUsingConnection();
            try
            {
                if (this.Transaction == null)
                {
                    var trans = this.Connection.BeginTransaction(this.Isolation);
                    try
                    {
                        this.Transaction = trans;
                        action(Transaction);
                        trans.Commit();
                    }
                    finally
                    {
                        this.Transaction = null;
                        trans.Dispose();
                    }
                }
                else
                {
                    action(Transaction);
                }
            }
            finally
            {
                this.StopUsingConnection();
            }
        }

        /// <summary>
        /// 执行简单的T-SQL查询操作。
        /// </summary>
        /// <param name="commandText">T-SQL查询操作命令</param>
        /// <returns></returns>
        public override int ExecuteCommand(string commandText)
        {
            if (this.Log != null)
            {
                this.Log.WriteLine(commandText);
            }
            this.StartUsingConnection();
            try
            {
                DbCommand cmd = this.Connection.CreateCommand();
                cmd.CommandText = commandText;
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                this.StopUsingConnection();
            }
        }

        /// <summary>
        /// 创建一个 <see cref="NkjSoft.ORM.Data.Common.QueryExecutor"/> 查询器对象。
        /// </summary>
        /// <returns></returns>
        protected override QueryExecutor CreateExecutor()
        {
            return new Executor(this);
        }

        /// <summary>
        ///  表示一个查询执行者。
        /// </summary>
        public class Executor : QueryExecutor
        {
            DbEntityProvider provider;
            int rowsAffected;

            /// <summary>
            /// 初始化一个新的 <see cref="Executor"/> .
            /// </summary>
            /// <param name="provider">查询提供程序.</param>
            public Executor(DbEntityProvider provider)
            {
                this.provider = provider;
            }

            /// <summary>
            /// Gets the provider.
            /// </summary>
            /// <value>The provider.</value>
            public DbEntityProvider Provider
            {
                get { return this.provider; }
            }

            /// <summary>
            /// Gets the rows affected.
            /// </summary>
            /// <value>The rows affected.</value>
            public override int RowsAffected
            {
                get { return this.rowsAffected; }
            }

            /// <summary>
            /// Gets a value indicating whether [buffer result rows].
            /// </summary>
            /// <value><c>true</c> if [buffer result rows]; otherwise, <c>false</c>.</value>
            protected virtual bool BufferResultRows
            {
                get { return false; }
            }

            /// <summary>
            /// 获取
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [action opened connection]; otherwise, <c>false</c>.
            /// </value>
            protected bool ActionOpenedConnection
            {
                get { return this.provider.actionOpenedConnection; }
            }

            /// <summary>
            /// 打开 DbConnnection.
            /// </summary>
            protected void StartUsingConnection()
            {
                this.provider.StartUsingConnection();
            }

            /// <summary>
            /// 关闭 DbConnnection。
            /// </summary>
            protected void StopUsingConnection()
            {
                this.provider.StopUsingConnection();
            }

            /// <summary>
            /// 转换指定类型的对象.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            public override object Convert(object value, Type type)
            {
                if (value == null)
                {
                    return TypeHelper.GetDefault(type);
                }
                type = TypeHelper.GetNonNullableType(type);
                Type vtype = value.GetType();
                if (type != vtype)
                {
                    if (type.IsEnum)
                    {
                        if (vtype == typeof(string))
                        {
                            return Enum.Parse(type, (string)value);
                        }
                        else
                        {
                            Type utype = Enum.GetUnderlyingType(type);
                            if (utype != vtype)
                            {
                                value = System.Convert.ChangeType(value, utype);
                            }
                            return Enum.ToObject(type, value);
                        }
                    }
                    return System.Convert.ChangeType(value, type);
                }
                return value;
            }

            /// <summary>
            /// 执行一条指定的命令并得到查询结果序列.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="command">The command.</param>
            /// <param name="fnProjector">The fn projector.</param>
            /// <param name="entity">The entity.</param>
            /// <param name="paramValues">The param values.</param>
            /// <returns></returns>
            public override IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues)
            {
                this.LogCommand(command, paramValues);
                this.StartUsingConnection();
                try
                {
                    DbCommand cmd = this.GetCommand(command, paramValues);
                    DbDataReader reader = this.ExecuteReader(cmd);
                    var result = Project(reader, fnProjector, entity, true);

                    //    IEnumerable<T> result = null;
                    if (this.provider.ActionOpenedConnection)
                    {
                        //TODO: test cost
                        //result = NkjSoft.Extensions.Data.DataExtensions.ToList<T>(reader, true);
                        result = result.ToList();
                    }
                    else
                    {
                        result = new EnumerateOnce<T>(result);
                    }
                    return result;
                }
                finally
                {
                    this.StopUsingConnection();
                }
            }

            /// <summary>
            /// 执行DbCommand 并得到 DbDataReader 对象。
            /// </summary>
            /// <param name="command">The command.</param>
            /// <returns></returns>
            protected virtual DbDataReader ExecuteReader(DbCommand command)
            {
                //TODO:此处会引起异常

                var reader = command.ExecuteReader();
                if (this.BufferResultRows)
                {
                    // use data table to buffer results
                    var ds = new DataSet();
                    ds.EnforceConstraints = false;
                    var table = new DataTable();
                    ds.Tables.Add(table);
                    ds.EnforceConstraints = false;
                    table.Load(reader);
                    reader = table.CreateDataReader();
                }
                return reader;
            }

            /// <summary>
            /// 将 DbDataReader 的结果映射到指定的 <typeparamref name="T"/> 对象序列。
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="reader">DbDataReader 数据源</param>
            /// <param name="fnProjector">映射的方式</param>
            /// <param name="entity">映射的实体.</param>
            /// <param name="closeReader">指定是否在换行结束之后关闭 DbDataReader 。if set to <c>true</c> [close reader].</param>
            /// <returns></returns>
            protected virtual IEnumerable<T> Project<T>(DbDataReader reader, Func<FieldReader, T> fnProjector, MappingEntity entity, bool closeReader)
            {
                var freader = new DbFieldReader(this, reader);
                try
                {
                    while (reader.Read())
                    {
                        yield return fnProjector(freader);
                    }
                }
                finally
                {
                    if (closeReader)
                    {
                        reader.Close();
                    }
                }
            }

            /// <summary>
            /// 执行一条查询命令,并返回查询影响的行数。 
            /// </summary>
            /// <param name="query">需要执行的查询</param>
            /// <param name="paramValues">查询的参数集合。</param>
            /// <returns></returns>
            public override int ExecuteCommand(QueryCommand query, params object[] paramValues)
            {

                this.LogCommand(query, paramValues);
                this.StartUsingConnection();
                try
                {
                    DbCommand cmd = this.GetCommand(query, paramValues);
                    this.rowsAffected = cmd.ExecuteNonQuery();
                    return this.rowsAffected;
                }
                finally
                {
                    this.StopUsingConnection();
                }
            }

            /// <summary>
            ///  执行批量操作。
            /// </summary>
            /// <param name="query">查询.</param>
            /// <param name="paramSets">查询参数集</param>
            /// <param name="batchSize">批量语句大小.</param>
            /// <param name="stream">是否使用流,if set to <c>true</c> [stream].</param>
            /// <returns></returns>
            public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream)
            {
                this.StartUsingConnection();
                try
                {
                    var result = this.ExecuteBatch(query, paramSets);
                    if (!stream || this.ActionOpenedConnection)
                    {
                        return result.ToList();
                    }
                    else
                    {
                        return new EnumerateOnce<int>(result);
                    }
                }
                finally
                {
                    this.StopUsingConnection();
                }
            }

            /// <summary>
            /// 执行批量操作。
            /// </summary>
            /// <param name="query">查询</param>
            /// <param name="paramSets">查询参数集</param>
            /// <returns></returns>
            private IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets)
            {
                this.LogCommand(query, null);
                DbCommand cmd = this.GetCommand(query, null);
                foreach (var paramValues in paramSets)
                {
                    this.LogParameters(query, paramValues);
                    this.LogMessage("");
                    this.SetParameterValues(query, cmd, paramValues);
                    this.rowsAffected = cmd.ExecuteNonQuery();
                    yield return this.rowsAffected;
                }
            }

            /// <summary>
            /// 执行对 <typeparamref name="T"/> 类型的批量操作。
            /// </summary>
            /// <typeparam name="T">类型</typeparam>
            /// <param name="query">T查询命令</param>
            /// <param name="paramSets">参数集</param>
            /// <param name="fnProjector">映射方式.</param>
            /// <param name="entity">实体</param>
            /// <param name="batchSize">批量语句大小.</param>
            /// <param name="stream">是否使用流,if set to <c>true</c> [stream].</param>
            /// <returns></returns>
            public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, MappingEntity entity, int batchSize, bool stream)
            {
                this.StartUsingConnection();
                try
                {
                    var result = this.ExecuteBatch(query, paramSets, fnProjector, entity);
                    if (!stream || this.ActionOpenedConnection)
                    {
                        return result.ToList();
                    }
                    else
                    {
                        return new EnumerateOnce<T>(result);
                    }
                }
                finally
                {
                    this.StopUsingConnection();
                }
            }

            /// <summary>
            /// 执行批量查询操作。
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="query">The query.</param>
            /// <param name="paramSets">The param sets.</param>
            /// <param name="fnProjector">The fn projector.</param>
            /// <param name="entity">The entity.</param>
            /// <returns></returns>
            private IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, MappingEntity entity)
            {
                this.LogCommand(query, null);
                DbCommand cmd = this.GetCommand(query, null);
                cmd.Prepare();
                foreach (var paramValues in paramSets)
                {
                    this.LogParameters(query, paramValues);
                    this.LogMessage("");
                    this.SetParameterValues(query, cmd, paramValues);
                    var reader = this.ExecuteReader(cmd);
                    var freader = new DbFieldReader(this, reader);
                    try
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            yield return fnProjector(freader);
                        }
                        else
                        {
                            yield return default(T);
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }

            /// <summary>
            /// 执行延迟加载。
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="query">The query.</param>
            /// <param name="fnProjector">The fn projector.</param>
            /// <param name="entity">The entity.</param>
            /// <param name="paramValues">The param values.</param>
            /// <returns></returns>
            public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues)
            {
                this.LogCommand(query, paramValues);
                this.StartUsingConnection();
                try
                {
                    DbCommand cmd = this.GetCommand(query, paramValues);
                    var reader = this.ExecuteReader(cmd);
                    var freader = new DbFieldReader(this, reader);
                    try
                    {
                        while (reader.Read())
                        {
                            yield return fnProjector(freader);
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    this.StopUsingConnection();
                }
            }

            /// <summary> 
            /// 通过查询命令已经参数获取此命令对应的 DbCommand 对象。
            /// </summary>
            /// <param name="query">The query.</param>
            /// <param name="paramValues">The param values.</param>
            /// <returns></returns>
            protected virtual DbCommand GetCommand(QueryCommand query, object[] paramValues)
            {
                // create command object (and fill in parameters)
                DbCommand cmd = this.provider.Connection.CreateCommand();
                cmd.CommandText = query.CommandText;
                if (this.provider.Transaction != null)
                    cmd.Transaction = this.provider.Transaction;
                this.SetParameterValues(query, cmd, paramValues);
                return cmd;
            }

            /// <summary>
            /// Sets the parameter values.
            /// </summary>
            /// <param name="query">The query.</param>
            /// <param name="command">The command.</param>
            /// <param name="paramValues">The param values.</param>
            protected virtual void SetParameterValues(QueryCommand query, DbCommand command, object[] paramValues)
            {
                if (query.Parameters.Count > 0 && command.Parameters.Count == 0)
                {
                    for (int i = 0, n = query.Parameters.Count; i < n; i++)
                    {
                        this.AddParameter(command, query.Parameters[i], paramValues != null ? paramValues[i] : null);
                    }
                }
                else if (paramValues != null)
                {
                    for (int i = 0, n = command.Parameters.Count; i < n; i++)
                    {
                        DbParameter p = command.Parameters[i];
                        if (p.Direction == System.Data.ParameterDirection.Input
                         || p.Direction == System.Data.ParameterDirection.InputOutput)
                        {
                            p.Value = paramValues[i] ?? DBNull.Value;
                        }
                    }
                }
            }

            /// <summary>
            /// Adds the parameter.
            /// </summary>
            /// <param name="command">The command.</param>
            /// <param name="parameter">The parameter.</param>
            /// <param name="value">The value.</param>
            protected virtual void AddParameter(DbCommand command, QueryParameter parameter, object value)
            {
                DbParameter p = command.CreateParameter();
                p.ParameterName = parameter.Name;
                p.Value = value ?? DBNull.Value;
                command.Parameters.Add(p);
            }

            /// <summary>
            /// Gets the parameter values.
            /// </summary>
            /// <param name="command">The command.</param>
            /// <param name="paramValues">The param values.</param>
            protected virtual void GetParameterValues(DbCommand command, object[] paramValues)
            {
                if (paramValues != null)
                {
                    for (int i = 0, n = command.Parameters.Count; i < n; i++)
                    {
                        if (command.Parameters[i].Direction != System.Data.ParameterDirection.Input)
                        {
                            object value = command.Parameters[i].Value;
                            if (value == DBNull.Value)
                                value = null;
                            paramValues[i] = value;
                        }
                    }
                }
            }

            /// <summary>
            /// Logs the message.
            /// </summary>
            /// <param name="message">The message.</param>
            protected virtual void LogMessage(string message)
            {
                if (this.provider.Log != null)
                {
                    this.provider.Log.WriteLine(message);
                }
            }

            /// <summary>
            /// Write a command & parameters to the log
            /// </summary>
            /// <param name="command"></param>
            /// <param name="paramValues"></param>
            protected virtual void LogCommand(QueryCommand command, object[] paramValues)
            {
                if (this.provider.Log != null)
                {
                    this.provider.Log.WriteLine(command.CommandText);
                    if (paramValues != null)
                    {
                        this.LogParameters(command, paramValues);
                    }
                    this.provider.Log.WriteLine();
                }
            }

            /// <summary>
            /// Logs the parameters.
            /// </summary>
            /// <param name="command">The command.</param>
            /// <param name="paramValues">The param values.</param>
            protected virtual void LogParameters(QueryCommand command, object[] paramValues)
            {
                //TODO: 2010-7-18 输出参数信息
                if (this.provider.Log != null && paramValues != null)
                {
                    for (int i = 0, n = command.Parameters.Count; i < n; i++)
                    {
                        var p = command.Parameters[i];
                        var v = paramValues[i];

                        if (v == null || v == DBNull.Value)
                        {
                            this.provider.Log.WriteLine("-- {0} = NULL", p.Name);//, p.QueryType.Length.ToString()p.QueryType.Length.ToString(),
                        }
                        else
                        {
                            this.provider.Log.WriteLine("-- {0}: Input {1} (Size = {2}; Prec = {3}; Scale = {4}) [{5}]",
                                p.Name, p.QueryType.DataType, v.ToString().Length.ToString(), p.QueryType.Precision.ToString(), p.QueryType.Scale.ToString(), v);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 数据阅读器。
        /// </summary>
        protected class DbFieldReader : FieldReader
        {
            QueryExecutor executor;
            DbDataReader reader;

            /// <summary>
            /// Initializes a new instance of the <see cref="DbFieldReader"/> class.
            /// </summary>
            /// <param name="executor">The executor.</param>
            /// <param name="reader">The reader.</param>
            public DbFieldReader(QueryExecutor executor, DbDataReader reader)
            {
                this.executor = executor;
                this.reader = reader;
                this.Init();
            }

            /// <summary>
            /// Gets the field count.
            /// </summary>
            /// <value>The field count.</value>
            protected override int FieldCount
            {
                get { return this.reader.FieldCount; }
            }

            /// <summary>
            /// Gets the type of the field.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Type GetFieldType(int ordinal)
            {
                return this.reader.GetFieldType(ordinal);
            }

            /// <summary>
            /// Determines whether [is DB null] [the specified ordinal].
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns>
            /// 	<c>true</c> if [is DB null] [the specified ordinal]; otherwise, <c>false</c>.
            /// </returns>
            protected override bool IsDBNull(int ordinal)
            {
                return this.reader.IsDBNull(ordinal);
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override T GetValue<T>(int ordinal)
            {
                return (T)this.executor.Convert(this.reader.GetValue(ordinal), typeof(T));
            }

            /// <summary>
            /// Gets the byte.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Byte GetByte(int ordinal)
            {
                return this.reader.GetByte(ordinal);
            }

            /// <summary>
            /// Gets the char.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Char GetChar(int ordinal)
            {
                return this.reader.GetChar(ordinal);
            }

            /// <summary>
            /// Gets the date time.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override DateTime GetDateTime(int ordinal)
            {
                return this.reader.GetDateTime(ordinal);
            }

            /// <summary>
            /// Gets the decimal.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Decimal GetDecimal(int ordinal)
            {
                return this.reader.GetDecimal(ordinal);
            }

            /// <summary>
            /// Gets the double.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Double GetDouble(int ordinal)
            {
                return this.reader.GetDouble(ordinal);
            }

            /// <summary>
            /// Gets the single.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Single GetSingle(int ordinal)
            {
                return this.reader.GetFloat(ordinal);
            }

            /// <summary>
            /// Gets the GUID.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Guid GetGuid(int ordinal)
            {
                return this.reader.GetGuid(ordinal);
            }

            /// <summary>
            /// Gets the int16.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Int16 GetInt16(int ordinal)
            {
                return this.reader.GetInt16(ordinal);
            }

            /// <summary>
            /// Gets the int32.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Int32 GetInt32(int ordinal)
            {
                return this.reader.GetInt32(ordinal);
            }

            /// <summary>
            /// Gets the int64.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override Int64 GetInt64(int ordinal)
            {
                return this.reader.GetInt64(ordinal);
            }

            /// <summary>
            /// Gets the string.
            /// </summary>
            /// <param name="ordinal">The ordinal.</param>
            /// <returns></returns>
            protected override String GetString(int ordinal)
            {
                return this.reader.GetString(ordinal);
            }
        }



        //public override event EventHandler QueryLog;
    }
}
