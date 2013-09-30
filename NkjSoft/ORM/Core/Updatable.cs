
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NkjSoft.ORM
{
    using NkjSoft.Extensions;
    using NkjSoft.ORM.Core;
    /// <summary>
    /// 提供对未指定数据类型的特定数据源的查询进行计算的功能。
    /// </summary>
    public interface IUpdatable : IQueryable
    {
    }

    /// <summary>
    /// 提供对指定数据类型已知的特定数据源的查询进行计算的功能。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUpdatable<T> : IUpdatable, IQueryable<T>
    {
    }

    /// <summary>
    /// 对实现了 <see cref="NkjSoft.ORM.IUpdatable"/> 类型的扩展。
    /// </summary>
    public static class Updatable
    {

        #region --- Insert  ---

        /// <summary>
        /// 向目标数据源插入一个实体集。
        /// </summary>
        /// <param name="collection">实体集对象</param>
        /// <param name="instance">单个实体</param>
        /// <param name="resultSelector">结果选择器。</param>
        /// <returns></returns>
        /// <remarks>此方法不进行属性更新检测。</remarks>
        public static object Insert(IUpdatable collection, object instance, LambdaExpression resultSelector)
        {
            var callMyself = Expression.Call(
                null,
                (MethodInfo)MethodInfo.GetCurrentMethod(),
                collection.Expression,
                Expression.Constant(instance),
                resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(LambdaExpression))
                );
            return collection.Provider.Execute(callMyself);
        }

        /// <summary>
        /// 向目标数据源插入一份数据映射对象的拷贝，并产生成功执行影响的结果。
        /// </summary>
        /// <typeparam name="T">实例集的类型</typeparam>
        /// <typeparam name="R">返回的结果的类型</typeparam>
        /// <param name="collection">更新实体集</param>
        /// <param name="instance">需要被插入的实体</param>
        /// <param name="resultSelector">产生结果的方法.</param>
        /// <returns>
        /// The value of the result if the insert succeed, otherwise null.
        /// </returns>
        /// <remarks>此方法不进行属性更新检测。</remarks>
        public static R Insert<T, R>(this IUpdatable<T> collection, T instance, Expression<Func<T, R>> resultSelector)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                collection.Expression,
                Expression.Constant(instance),
                resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, R>>))
                );
            return (R)collection.Provider.Execute(callMyself);
        }

        /// <summary>
        /// 向目标数据源插入一份数据映射对象的拷贝，并产生成功执行影响的结果。
        /// </summary>
        /// <typeparam name="T">实例集的类型</typeparam>
        /// <typeparam name="R">返回的结果的类型</typeparam>
        /// <param name="collection">更新实体集</param>
        /// <param name="instance">需要被插入的实体</param>
        /// <param name="resultSelector">产生结果的方法.</param>
        /// <param name="fieldsToUpdate">指定需要更新的字段，会自动过滤不存在的字段。如果全部不存在，则会抛出异常</param>
        /// <returns>
        /// The value of the result if the insert succeed, otherwise null.
        /// </returns>
        /// <remarks>此方法不进行属性更新检测。</remarks>
        public static R Insert<T, R>(this IUpdatable<T> collection, T instance, Expression<Func<T, R>> resultSelector, ColumnsIndeed fieldsToUpdate)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                collection.Expression,
                Expression.Constant(instance),
                resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, R>>)),
                fieldsToUpdate != null ? Expression.Constant(fieldsToUpdate, typeof(ColumnsIndeed))
                           : Expression.Constant(null, typeof(LambdaExpression))
                );
            return (R)collection.Provider.Execute(callMyself);
        }

        /// <summary> 
        /// 向目标数据源插入一个实体记录，该实体通过 CLR 映射获取信息。
        /// </summary>
        /// <typeparam name="T">更新集类型</typeparam>
        /// <param name="collection">更新集.</param>
        /// <param name="instance">包含进行插入操作的数据实体对象。</param>
        /// <returns> 
        /// 1 表示成功，0表示失败。
        /// </returns>
        /// <remarks>此方法进行实体属性更新检测。</remarks>
        public static int Insert<T>(this IUpdatable<T> collection, T instance)
        {
            var fieldsToInsert = getInsertOrUpdateColumnsFrom(instance);
            return insert(collection, instance, fieldsToInsert);
        }

        /// <summary>
        /// 得到实体中属性值发生更改的成员名称.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string[] getInsertOrUpdateColumnsFrom(object obj)
        {
            NkjSoft.ORM.Data.IEntity temp = obj as NkjSoft.ORM.Data.IEntity;
            if (temp != null && temp.PropertysToInsertOrUpdate != null && temp.PropertysToInsertOrUpdate.Count > 0)
            {
                return temp.PropertysToInsertOrUpdate.Values.Cast<string>().ToArray();
            }
            return null;
        }


        /// <summary>
        /// 向目标数据源插入一个实体记录，该实体通过 CLR 映射获取信息。
        /// </summary>
        /// <typeparam name="T">更新集类型</typeparam>
        /// <param name="collection">更新集.</param>
        /// <param name="instance">包含进行插入操作的数据实体对象。</param>
        /// <param name="columnSelector">The column selector.</param>
        /// <returns>1 表示成功，0表示失败。</returns>
        /// <remarks>此方法不进行实体属性更新检测。指定的操作字段由参数 fieldsToInsert 决定。</remarks>
        public static int InsertToColumns<T, TC>(this IUpdatable<T> collection, T instance, Expression<Func<T, TC>> columnSelector)
        {
            var fieldsToInsert = getColumnsFromExpression(columnSelector as LambdaExpression);
            return insert(collection, instance, fieldsToInsert);
        }

        private static int insert<T>(this IUpdatable<T> collection, T instance, string[] fieldsToInsert)
        {
            if (fieldsToInsert == null || fieldsToInsert.Length == 0)
            {
                return Insert<T, int>(collection, instance, null);
            }
            return Insert<T, int>(collection, instance, null, new ColumnsIndeed() { ColumnsToBeHandled = new List<string>(fieldsToInsert).AsReadOnly() });
        }

        #endregion

        #region --- Update ---

        /// <summary>
        /// 对目标数据源的特定记录的指定列进行更新操作。
        /// </summary>
        /// <param name="collection">.</param>
        /// <param name="instance">更新的实体</param>
        /// <param name="updateCheck">更新检查</param>
        /// <param name="resultSelector">执行更新的筛选方法</param>
        /// <param name="fieldsToBeUpdated">指定需要更新的字段，会自动过滤不存在的字段。如果全部不存在，则会抛出异常。</param>
        /// <returns></returns>
        private static TR Update<T, TR>(IUpdatable collection, T instance, LambdaExpression updateCheck, LambdaExpression resultSelector, ColumnsIndeed fieldsToBeUpdated)
        {
            var callMyself = Expression.Call(
                           null,
                           ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TR)),
                           collection.Expression,
                           Expression.Constant(instance),
                           updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(LambdaExpression)),
                           resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, TR>>)),
                           fieldsToBeUpdated != null ? Expression.Constant(fieldsToBeUpdated, typeof(ColumnsIndeed))
                           : Expression.Constant(null, typeof(LambdaExpression))//new ColumnIndeedExpression(,fieldsToBeUpdated)
                           );
            return (TR)collection.Provider.Execute(callMyself);
        }


        /// <summary> 
        /// 对目标数据源的特定记录进行更新操作，该方法需要提供自定义的更新检查方法以及更新筛选方法。
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <typeparam name="TS">返回结果类型</typeparam>
        /// <param name="collection">更新集</param>
        /// <param name="instance">即将被推送到数据库更新的对象.</param>
        /// <param name="updateCheck">一个 <see cref="System.Linq.Expressions.Expression&lt;TDelegate&gt;"/> 的更新检查方法，只有通过了此方法检查的对象才会被更新。</param>
        /// <param name="resultSelector">返回更新结果的方法。.</param>
        /// <returns>更新成功，返回 S 类型的对象，否则返回Null</returns>
        /// <remarks>执行成功后，执行一个返回指定结果的 SELECT操作。此方法不进行属性更新检测。</remarks>
        public static TS Update<T, TS>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck, Expression<Func<T, TS>> resultSelector)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TS)),
                collection.Expression,
                Expression.Constant(instance),
                updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(Expression<Func<T, bool>>)),
                resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, TS>>))
                );
            return (TS)collection.Provider.Execute(callMyself);
        }


        /// <summary> 
        /// 对目标数据源的特定记录的所有字段进行更新操作(请确保对对象的每个属性进行赋值，否则可能会将Null更新到目标数据源)，方法接收一个 <see cref="System.Linq.Expressions.Expression&lt;TDelegate&gt;"/> 的更新检查。只要通过了此更新检查到对象才会被推送到数据源。
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="collection">更新集</param>
        /// <param name="instance">即将被推送到数据库更新的对象.</param>
        /// <param name="updateCheck">一个 <see cref="System.Linq.Expressions.Expression&lt;TDelegate&gt;"/> 的更新检查方法，只有通过了此方法检查的对象才会被更新。</param>
        /// <returns>1成功，0失败.</returns>
        /// <remarks>此方法进行实体属性更新检测。</remarks>
        public static int Update<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck)
        {
            var fieldsToBeUpdated = getInsertOrUpdateColumnsFrom(instance);
            return Update<T>(collection, instance, updateCheck, fieldsToBeUpdated);
        }

        /// <summary>
        /// 对目标数据源的特定记录的指定字段进行更新操作，方法接收一个 <see cref="System.Linq.Expressions.Expression&lt;TDelegate&gt;"/> 的更新检查。只要通过了此更新检查到对象才会被推送到数据源。
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="collection">更新集</param>
        /// <param name="instance">即将被推送到数据库更新的对象.</param>
        /// <param name="updateCheck">一个 <see cref="System.Linq.Expressions.Expression&lt;TDelegate&gt;"/> 的更新检查方法，只有通过了此方法检查的对象才会被更新。</param>
        /// <param name="fieldsToBeUpdated">指定需要更新的字段，会自动过滤不存在的字段。如果全部不存在，则会抛出异常。</param>
        /// <returns>1成功，0失败.</returns>
        /// <remarks>此方法进行实体属性更新检测。</remarks>
        public static int Update<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck, params string[] fieldsToBeUpdated)
        {
            var temp = fieldsToBeUpdated;
            if (temp != null)
                return Update<T, int>(collection, instance, updateCheck, null, new ColumnsIndeed()
                {
                    ColumnsToBeHandled = new System.Collections.ObjectModel.ReadOnlyCollection<string>(temp.ToList())
                });
            else
                return Update<T, int>(collection, instance, updateCheck, null);
        }

        /// <summary>
        /// 根据映射的实体执行数据库 Update 操作,执行成功之后执行返回指定结果的 Select 操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="collection">更新器</param>
        /// <param name="instance">映射的实体.</param>
        /// <param name="resultSelector">SELECT 操作返回的结果.</param>
        /// <returns></returns>
        /// <remarks>此方法进行实体属性更新检测。</remarks>
        public static TResult Update<T, TResult>(this IUpdatable<T> collection, T instance, Expression<Func<T, TResult>> resultSelector)
        {
            var fieldsToUpdate = getInsertOrUpdateColumnsFrom(instance);
            if (fieldsToUpdate != null)
                return update<T, TResult>(collection, instance, null, resultSelector, fieldsToUpdate);
            return Update<T, TResult>(collection, instance, null, resultSelector);
        }

        /// <summary> 
        /// 对目标数据源的特定记录进行更新操作。该方法通过映射实体指定的主键字段进行数据匹配。
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="collection">更新集</param>
        /// <param name="instance">即将被推送到数据库更新的对象.</param>
        /// <returns>1成功，0失败.</returns>
        /// <remarks>此方法进行实体属性更新检测。</remarks>
        public static int Update<T>(this IUpdatable<T> collection, T instance)
        {
            //TODO:2010-8-20 更新:检测属性值更新的字段  p3
            var fieldsToUpdate = getInsertOrUpdateColumnsFrom(instance);
            if (fieldsToUpdate != null && fieldsToUpdate.Length > 0)
                return update(collection, instance, null, fieldsToUpdate);
            return Update<T, int>(collection, instance, null, null);
        }

        /// <summary>
        /// 对目标数据源的记录进行指定字段的更新，更新的信息来自映射实体。
        /// </summary>
        /// <typeparam name="T">操作的映射实体类型</typeparam>
        /// <typeparam name="TS">The type of the S.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="instance">提供更新信息的实体实例</param>
        /// <param name="columnsToUpdate">指定被更新的列</param>
        /// <returns></returns>
        /// <remarks>此方法进行实体属性更新检测。</remarks>
        public static int UpdateColumns<T, TS>(this IUpdatable<T> collection, T instance, Expression<Func<T, TS>> columnsToUpdate)
        {
            //TODO:2010-8-20 更新:检测属性值更新的字段  p2
            var fieldsToBeUpdated = getColumnsFromExpression(columnsToUpdate as LambdaExpression);
            return update(collection, instance, null, fieldsToBeUpdated);

        } //UNDONE : this 


        //TODO:2010-8-20 更新:检测属性值更新的字段  p1
        private static int update<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck, string[] fieldsToBeUpdated)
        {
            if (fieldsToBeUpdated == null || fieldsToBeUpdated.Length == 0)
            {
                return Update<T, int>(collection, instance, updateCheck, null, null);
            }
            ColumnsIndeed ci = new ColumnsIndeed() { ColumnsToBeHandled = new List<string>(fieldsToBeUpdated).AsReadOnly() };
            return Update<T, int>(collection, instance, updateCheck, null, ci);
        }

        //TODO:2010-8-20 更新:检测属性值更新的字段  p1
        private static TS update<T, TS>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck, Expression<Func<T, TS>> resultSelector, string[] fieldsToBeUpdated)
        {
            ColumnsIndeed ci = null;
            if (fieldsToBeUpdated != null && fieldsToBeUpdated.Length > 0)
            {
                ci = new ColumnsIndeed() { ColumnsToBeHandled = new List<string>(fieldsToBeUpdated).AsReadOnly() };
            }
            return Update<T, TS>(collection, instance, updateCheck, resultSelector, ci);
        }

        #endregion

        #region --- Insert Or Update Columns  ---
        private static string[] getColumnsFromExpression(LambdaExpression expresssion)
        {
            var body = expresssion.Body;
            var p1 = body.Type;
            var bodyType = body.NodeType;

            if (bodyType == ExpressionType.MemberAccess)
                return new string[] { (body as MemberExpression).Member.Name };
            else if (bodyType == ExpressionType.New)
            {
                var n = body as NewExpression;
                if (n != null)
                    return n.Members.Select(p => p.Name).ToArray();
            }
            return null;
        }
        #endregion

        #region --- InsertOrUpdate ---
        /// 向目标数据源执行插入或者更新记录的操作，记录数据将有映射的实体对象实例提供。
        /// </summary>
        /// <param name="collection">更新集类型</param>
        /// <param name="instance">即将被推送到数据库更新的对象.</param>
        /// <param name="updateCheck">一个 <see cref="System.Linq.Expressions.LambdaExpression"/> 的更新检查方法，只有通过了此方法检查的对象才会被更新。</param>
        /// <param name="resultSelector">返回更新结果的方法。.</param>
        /// <returns>更新成功，返回 S 类型的对象，否则返回Null</returns>  
        public static object InsertOrUpdate(IUpdatable collection, object instance, LambdaExpression updateCheck, LambdaExpression resultSelector)
        {
            var callMyself = Expression.Call(
                null,
                (MethodInfo)MethodInfo.GetCurrentMethod(),
                collection.Expression,
                Expression.Constant(instance),
                updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(LambdaExpression)),
                resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(LambdaExpression))
                );
            return collection.Provider.Execute(callMyself);
        }
        /// <summary>
        /// 向目标数据源插入一个经过实体映射得到的对象，如果目标数据源已经存在与实体相同的值得记录则进行更新操作。该方法返回执行的结果。
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TS">结果的类型.</typeparam>
        /// <param name="collection">实体对象映射集合.</param>
        /// <param name="instance">需要被插入或者更新的对象。</param>
        /// <param name="updateCheck">一个 <see cref="System.Linq.Expressions.LambdaExpression"/> 的更新检查方法，只有通过了此方法检查的对象才会被更新。</param>
        /// <param name="resultSelector">返回更新结果的方法。.</param>
        /// <returns>更新成功，返回 S 类型的对象，否则返回Null</returns> 
        public static TS InsertOrUpdate<T, TS>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck, Expression<Func<T, TS>> resultSelector)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TS)),
                collection.Expression,
                Expression.Constant(instance),
                updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(Expression<Func<T, bool>>)),
                resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, TS>>))
                );
            return (TS)collection.Provider.Execute(callMyself);
        }


        /// <summary>
        /// 向目标数据源插入一个经过实体映射得到的对象，如果目标数据源已经存在与实体相同的值得记录则进行更新操作。该方法返回执行影响数据源的行数。
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="collection">实体对象映射集合.</param>
        /// <param name="instance">需要被插入或者更新的对象。</param>
        /// <param name="updateCheck">一个 <see cref="System.Linq.Expressions.LambdaExpression"/> 的更新检查方法，只有通过了此方法检查的对象才会被更新。</param>
        /// <returns>1成功，0失败.</returns> 
        public static int InsertOrUpdate<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck)
        {
            return InsertOrUpdate<T, int>(collection, instance, updateCheck, null);
        }


        /// <summary>
        /// 根据默认的映射方式向目标数据源插入一个经过实体映射得到的对象，（方法通过实体主键进行匹配更新）如果目标数据源已经存在与实体相同的值得记录则进行更新操作。该方法返回执行影响数据源的行数。
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="collection">实体对象映射集合.</param>
        /// <param name="instance">需要被插入或者更新的对象。</param>
        /// <returns>1成功，0失败.</returns> 
        public static int InsertOrUpdate<T>(this IUpdatable<T> collection, T instance)
        {
            return InsertOrUpdate<T, int>(collection, instance, null, null);
        }

        #endregion

        #region --- Delete ---
        /// <summary>
        /// 将目标数据源中匹配了删除条件的的数据记录删除，返回执行的结果。
        /// </summary>
        /// <param name="collection">.</param>
        /// <param name="instance">实体</param>
        /// <param name="deleteCheck">删除操作过滤方法。</param>
        /// <returns></returns>
        public static object Delete(IUpdatable collection, object instance, LambdaExpression deleteCheck)
        {
            var callMyself = Expression.Call(
                null,
                (MethodInfo)MethodInfo.GetCurrentMethod(),
                collection.Expression,
                Expression.Constant(instance),
                deleteCheck != null ? (Expression)Expression.Quote(deleteCheck) : Expression.Constant(null, typeof(LambdaExpression))
                );
            return collection.Provider.Execute(callMyself);
        }

        /// <summary> 
        /// 将目标数据源中匹配了删除条件的的数据记录删除，返回执行的结果。该方法接收一个删除筛选方法进行实体匹配。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">.</param>
        /// <param name="instance">需要被删除的实体映射.</param>
        /// <param name="deleteCheck">一个 <see cref="System.Linq.Expressions.Expression&lt;TDelegate&gt;"/>，用于筛选删除方法匹配。</param>
        /// <returns>1成功，0失败.</returns>
        public static int Delete<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> deleteCheck)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                collection.Expression,
                Expression.Constant(instance),
                deleteCheck != null ? (Expression)Expression.Quote(deleteCheck) : Expression.Constant(null, typeof(Expression<Func<T, bool>>))
                );
            return (int)collection.Provider.Execute(callMyself);
        }

        /// <summary>
        /// 将目标数据源中匹配了删除条件的的数据记录删除。该方法使用实体默认的映射主键进行删除匹配。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">.</param>
        /// <param name="instance">需要被删除的实体映射.</param>
        /// <returns>1成功，0失败.</returns>
        public static int Delete<T>(this IUpdatable<T> collection, T instance)
        {
            return Delete<T>(collection, instance, null);
        }

        /// <summary>
        /// 指定一个 <see cref="System.Linq.Expressions.LambdaExpression"/>，删除目标数据源中，匹配了此方法的的记录。
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">一个 <see cref="System.Linq.Expressions.LambdaExpression"/>匹配需要删除的记录</param>
        /// <returns>1成功，0失败.</returns>
        public static int Delete(IUpdatable collection, LambdaExpression predicate)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()),
                collection.Expression,
                predicate != null ? (Expression)Expression.Quote(predicate) : Expression.Constant(null, typeof(LambdaExpression))
                );
            return (int)collection.Provider.Execute(callMyself);
        }

        /// <summary>
        /// 根据一个已知类型的映射实体，向目标数据源删除匹配此实体映射相同的记录。
        /// </summary>
        /// <typeparam name="T">实体映射类型.</typeparam>
        /// <param name="collection">实体对象.</param>
        /// <param name="predicate">一个 <see cref="System.Linq.Expressions.LambdaExpression"/>匹配需要删除的记录</param>
        /// <returns>删除的记录总数</returns>
        public static int Delete<T>(this IUpdatable<T> collection, Expression<Func<T, bool>> predicate)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                collection.Expression,
                predicate != null ? (Expression)Expression.Quote(predicate) : Expression.Constant(null, typeof(Expression<Func<T, bool>>))
                );
            return (int)collection.Provider.Execute(callMyself);
        }

        #endregion

        #region --- Batch ---
        /// <summary>
        /// 对目标数据源中匹配实体映射对象的记录进行批量操作。
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="items">即将被批量操作的实体例序.</param>
        /// <param name="fnOperation">表示进行的操作方法。</param>
        /// <param name="batchSize">执行批量操作的数量。</param>
        /// <param name="stream">true，表示对操作进行流处理，否则 false</param>
        /// <returns></returns>
        public static IEnumerable Batch(IUpdatable collection, IEnumerable items, LambdaExpression fnOperation, int batchSize, bool stream)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()),
                collection.Expression,
                Expression.Constant(items),
                fnOperation != null ? (Expression)Expression.Quote(fnOperation) : Expression.Constant(null, typeof(LambdaExpression)),
                Expression.Constant(batchSize),
                Expression.Constant(stream)
                );
            return (IEnumerable)collection.Provider.Execute(callMyself);
        }

        /// <summary> 
        /// 应用一个 Insert 、Update、InsertOrUpdate 或者 Delete 操作，基于此方法，对目标数据源中匹配实体的每个记录进行批量的操作。
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="T">实例类型</typeparam>
        /// <typeparam name="S">方法返回结果的类型</typeparam>
        /// <param name="collection">实体集</param>
        /// <param name="instances">即将被批量操作的实体例序。 </param>
        /// <param name="fnOperation">将要进行的批量操作方法</param>
        /// <param name="batchSize">批量操作的初始可操作的数量</param>
        /// <param name="stream">true，表示该操作将进行延迟执行（在结果进行foreach、for 遍历的时候才会被执行），否则false。</param>
        /// <returns> 
        /// 返回每个操作执行的结果序列.
        /// </returns>
        public static IEnumerable<S> Batch<U, T, S>(this IUpdatable<U> collection, IEnumerable<T> instances, Expression<Func<IUpdatable<U>, T, S>> fnOperation, int batchSize, bool stream)
        {
            var callMyself = Expression.Call(
                null,
                ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(U), typeof(T), typeof(S)),
                collection.Expression,
                Expression.Constant(instances),
                fnOperation != null ? (Expression)Expression.Quote(fnOperation) : Expression.Constant(null, typeof(Expression<Func<IUpdatable<U>, T, S>>)),
                Expression.Constant(batchSize),
                Expression.Constant(stream)
                );
            return (IEnumerable<S>)collection.Provider.Execute(callMyself);
        }

        /// <summary>
        /// 应用一个 Insert 、Update、InsertOrUpdate 或者 Delete 操作，基于此方法，对目标数据源中匹配实体的每个记录进行批量的操作。
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="T">实例类型</typeparam>
        /// <typeparam name="S">方法返回结果的类型</typeparam>
        /// <param name="collection">实现了IUpdatable的表对象</param>
        /// <param name="instances">即将被批量操作的实体例序。</param>
        /// <param name="fnOperation">将要进行的批量操作方法</param>
        /// <returns> 
        /// 返回每个操作执行的结果序列.
        /// </returns>
        public static IEnumerable<S> Batch<U, T, S>(this IUpdatable<U> collection, IEnumerable<T> instances, Expression<Func<IUpdatable<U>, T, S>> fnOperation)
        {
            return Batch<U, T, S>(collection, instances, fnOperation, 50, false);
        }

        #endregion

        #region --- SessionOnSubmit() ---
        /// <summary>
        /// 在会话状态提交的时候进行 Insert 操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="instance">实体映射</param>
        public static void InsertOnSubmit<T>(this IUpdatable<T> collection, T instance)
        {
            (collection.Provider as NkjSoft.ORM.Data.DbEntityProvider).Session.GetTable<T>().InsertOnSubmit(instance);
        }
        /// <summary>
        /// 在会话状态提交的时候进行根据主键信息的 Update 操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="instance">The instance.</param>
        public static void UpdateOnSubmit<T>(this IUpdatable<T> collection, T instance)
        {
            (collection.Provider as NkjSoft.ORM.Data.DbEntityProvider).Session.GetTable<T>().UpdateOnSubmit(instance);
        }

        /// <summary>
        /// 将当前 <see cref="IEntityTable&lt;T&gt;"/> 转换成 <see cref="ISessionTable&lt;T&gt;"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityTable">The entity table.</param>
        /// <returns></returns>
        public static ISessionTable<T> AsSessionTable<T>(this IEntityTable<T> entityTable)
        {
            return (entityTable.Provider as NkjSoft.ORM.Data.DbEntityProvider).Session.GetTable<T>();
        }
        #endregion
    }
}