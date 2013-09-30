//--------------------------文档信息----------------------------
//       
//                 文件名: BllBase                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.ORM
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/3 22:26:56
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;


using NkjSoft.Extensions;
using NkjSoft.CommonTypes;
using NkjSoft.ORM.Data;
namespace NkjSoft.ORM
{
    /// <summary>
    /// 默认定义的 BlllBase 类，提供缓存 <see cref="QueryContext"/> 的能力，提供子类调用。
    /// </summary>
    public class NkjSoftBllBase<T> where T : class ,new()
    {
        #region --- 基础设施 ---
        /// <summary>
        /// QueryContext 缓存集合。//UNDONE暂时没有使用..
        /// </summary>
        private static readonly Dictionary<string, QueryContext> dbCache = new Dictionary<string, QueryContext>();
        private static readonly string defKey = "Default_DB";
        private static readonly string keyFormatter = "db_{0}";
        /// <summary>
        /// 使用一个 <see cref="QueryContext"/> 子类作为默认的查询上下文对象，实例化 <see cref="NkjSoftBllBase"/> 。
        /// </summary>
        /// <param name="defDb"> 一个 <see cref="QueryContext"/> 子类作为默认的查询上下文对象</param>
        public NkjSoftBllBase(QueryContext defDb)
        {
            this[defKey] = defDb;
        }

        /// <summary>
        /// 获取当前连接的数据库映射的查询上下文。
        /// </summary>
        protected QueryContext DataBase
        {
            get { return this[defKey]; }
        }


        /// <summary>
        /// 通过数据库对应的名字获取或者缓存的 <see cref="NkjSoft.ORM.QueryContext"/> 实例。
        /// </summary>
        /// <value></value>
        protected QueryContext this[string dbName]
        {
            get
            {
                string t = keyFormatter.FormatWith(dbName);
                if (dbCache.ContainsKey(t))
                {
                    return dbCache[keyFormatter.FormatWith(dbName)];
                } return null;
            }
            set { dbCache[keyFormatter.FormatWith(dbName)] = value; }
        }
        #endregion

        #region --- 常用方法 ---
        /// <summary>
        /// 向当前连接的数据库的 <typeparamref name="T"/> 添加记录。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public MethodResult Add(T entity)
        {
            return DataBase.Provider.GetTable<T>().Insert(entity) > 0 ? MethodResult.Successfully : MethodResult.Failed;
        }

        /// <summary>
        /// 向当前连接的数据库的 <typeparamref name="T"/> 添加记录。方法接收一个 <see cref="Expression&lt;TDelegate&gt;"/>判断表中是否已经存在指定条件的记录，不存在则执行 Insert 操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">添加的记录的数据的实体映射.</param>
        /// <param name="existsCheck">用于判断是否已经存在特定记录的表达式。</param>
        /// <returns></returns>
        public MethodResult Add(T entity, Expression<Func<T, bool>> existsCheck)
        {
            if (Exists(existsCheck))
            { return MethodResult.RecordExists; }

            return Add(entity);
        }

        /// <summary>
        /// 在当前连接的数据库的 <see cref="T"/> 表中判断是否存在指定匹配条件的记录.
        /// </summary>
        /// <param name="predicate">定匹配条件</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return DataBase.Provider.GetTable<T>().Count(predicate) > 0;
        }

        /// <summary>
        /// 根据默认主键更新当前连接的数据库中 <typeparamref name="T"/> 的记录。
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public MethodResult Update(T entity)
        {
            return DataBase.Provider.GetTable<T>().Update(entity) > 0 ? MethodResult.Successfully : MethodResult.Failed;
        }


        /// <summary>
        /// 根据指定的筛选条件更新当前连接的数据库中 <typeparamref name="T"/> 的记录。
        /// </summary>
        /// <param name="entity">更新的信息。</param>
        /// <param name="predicate">匹配条件，只更新符合条件的记录</param>
        /// <returns></returns>
        public MethodResult Update(T entity, Expression<Func<T, bool>> predicate)
        {
            return DataBase.Provider.GetTable<T>().Update(entity, predicate) > 0 ? MethodResult.Successfully : MethodResult.Failed;
        }


        /// <summary>
        /// 从当前连接的数据库中的 <typeparamref name="T"/> 表中删除符合指定的条件的记录,返回删除的记录条数。
        /// </summary>
        /// <param name="predicate">指定的条件</param>
        /// <returns>删除的记录条数</returns>
        public int Delete(Expression<Func<T, bool>> predicate)
        {
            int r = DataBase.Provider.GetTable<T>().Delete(predicate);
            return r;
        }

        /// <summary>
        /// 从当前连接的数据库中的 <typeparamref name="T"/> 表中根据默认的主键信息删除记录。
        /// </summary>
        /// <param name="entity">包含对主键字段赋值了的实体.</param>
        /// <returns>删除的记录条数</returns>
        public MethodResult Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentException("entity");
            int r = DataBase.Provider.GetTable<T>().Delete(entity);
            return r > 0 ? MethodResult.Successfully : MethodResult.Failed;
        }

        /// <summary>
        /// 返回当前连接的数据库 <typeparamref name="T"/> 中符合条件的第一条记录对应的强类型对象。不存在，则返回Null。
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DataBase.Provider.GetTable<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 返回当前连接的数据库 <typeparamref name="T"/> 中符合条件的记录的强类型集合。不存在，则返回Null。
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual List<T> ToList(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
                return DataBase.Provider.GetTable<T>().Where(predicate).ToList();
            return DataBase.Provider.GetTable<T>().ToList();
        }
        /// <summary>
        /// 返回当前连接的数据库 <typeparamref name="T"/> 中符合条件的记录的强类型集合。不存在，则返回Null。
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual List<T> ToList()
        {
            return ToList(null);
        }

        /// <summary>
        /// 获取当前连接数据库的查询提供程序。
        /// </summary>
        public DbEntityProvider Provider { get { return DataBase.Provider; } }

        /// <summary>
        /// 向目标表执行添加记录的方法,并返回一个指定字段的值或多个值组成的对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">返回一个指定字段的值的类型.</typeparam>
        /// <param name="instance">记录映射实体.</param>
        /// <param name="resultSelector">回一个指定字段的值的表达式.</param>
        /// <returns></returns>
        public TResult Add<TResult>(T instance, Expression<Func<T, TResult>> resultSelector)
        {
            return Provider.GetTable<T>().Insert(instance, resultSelector);
        }


        /// <summary>
        /// 向目标表执行添加记录的方法,在添加的同时执行一个指定的判断,判断和当前记录存在的记录是否存在,方法返回一个指定字段的值或多个值组成的对象。
        /// </summary>
        /// <typeparam name="TResult">返回一个指定字段的值的类型..</typeparam>
        /// <param name="entity">记录映射实体.</param>
        /// <param name="existsCheck">一个指定的判断,判断和当前记录存在的记录是否存在.</param>
        /// <param name="resultSelector">回一个指定字段的值的表达式.</param>
        /// <returns></returns>
        ///<exception cref="System.ArgumentNullException">参数空</exception>
        public MethodReturnInfo<TResult> Add<TResult>(T entity, Expression<Func<T, bool>> existsCheck, Expression<Func<T, TResult>> resultSelector)
        {
            CheckPredicate(existsCheck, "existsCheck");
            CheckPredicate(resultSelector, "resultSelector");
            CheckPredicate(resultSelector, "entity");

            var table = DataBase.Provider.GetTable<T>();

            MethodReturnInfo<TResult> mri = new MethodReturnInfo<TResult>();
            mri.SimpleInfo = default(TResult);
            //判断是否存在
            if (table.Count(existsCheck) > 0)
            {
                mri.ReturnType = MethodResult.RecordExists;
                return mri;
            }
            //执行添加,并获取指定的返回类型。对象.
            var result = table.Insert(entity, resultSelector);

            mri.SimpleInfo = result;
            mri.ReturnType = result != null ? MethodResult.Successfully : MethodResult.Failed;
            return mri;
        }
        #endregion

        #region --- 辅助方法 ---
        /// <summary>
        /// Checks the predicate.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="pName">Name of the p.</param>
        private void CheckPredicate(Expression e, string pName)
        {
            if (e == null)
                throw new ArgumentNullException(pName);
        }

        #endregion

        #region --- 实例方法 ---
        /// <summary>
        ///  分页方法。
        /// </summary>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页大小.</param>
        /// <param name="searchArgs">查询参数</param>
        /// <returns></returns>
        public virtual object ToPage(int pageIndex, int pageSize, System.Collections.Specialized.HybridDictionary searchArgs)
        {
            return new object();
        }

        /// <summary>
        /// 分页方法。
        /// </summary>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页大小.</param>
        /// <param name="searchArgs">查询参数</param>
        /// <param name="recordCount">记录总数.</param>
        /// <returns></returns>
        public virtual object ToPage(int pageIndex, int pageSize, System.Collections.Specialized.HybridDictionary searchArgs, ref int recordCount)
        {
            recordCount = 0;
            return new object();
        }


        /// <summary>
        /// 返回指定条件筛选的最后一条记录。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultSelector">对T指定的条件筛选</param>
        /// <returns></returns>
        public virtual T LastOrDefault(Expression<Func<T, bool>> resultSelector)
        {
            if (resultSelector == null)
                return DataBase.Provider.GetTable<T>().LastOrDefault();
            return DataBase.Provider.GetTable<T>().LastOrDefault(resultSelector);
        }

        #endregion
    }
}
