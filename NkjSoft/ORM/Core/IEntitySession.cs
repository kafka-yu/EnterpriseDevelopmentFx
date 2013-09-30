 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// 表示提供一种会话状态信息保存的 IEntityTable 查询能力。
    /// </summary>
    public interface IEntitySession
    {
        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        IEntityProvider Provider { get; }
        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableId">The table id.</param>
        /// <returns></returns>
        ISessionTable<T> GetTable<T>(string tableId);
        ISessionTable<T> GetTable<T>();
        ISessionTable GetTable(Type elementType, string tableId);
        void SubmitChanges();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISessionTable : IQueryable
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        IEntitySession Session { get; }
        /// <summary>
        /// Gets the provider table.
        /// </summary>
        /// <value>The provider table.</value>
        IEntityTable ProviderTable { get; }
        object GetById(object id);
        void SetSubmitAction(object instance, SubmitAction action);
        SubmitAction GetSubmitAction(object instance);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISessionTable<T> : IQueryable<T>, ISessionTable
    {
        /// <summary>
        /// Gets the provider table.
        /// </summary>
        /// <value>The provider table.</value>
        new IEntityTable<T> ProviderTable { get; }
        new T GetById(object id);
        void SetSubmitAction(T instance, SubmitAction action);
        SubmitAction GetSubmitAction(T instance);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SubmitAction
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Update,
        /// <summary>
        /// 
        /// </summary>
        PossibleUpdate,
        /// <summary>
        /// 
        /// </summary>
        Insert,
        /// <summary>
        /// 
        /// </summary>
        InsertOrUpdate,
        /// <summary>
        /// 
        /// </summary>
        Delete
    }

    /// <summary>
    /// 
    /// </summary>
    public static class SessionTableExtensions
    {
        /// <summary>
        /// Inserts the on submit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void InsertOnSubmit<T>(this ISessionTable<T> table, T instance)
        {
            table.SetSubmitAction(instance, SubmitAction.Insert);
        }

        /// <summary>
        /// Inserts the on submit.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void InsertOnSubmit(this ISessionTable table, object instance)
        {
            table.SetSubmitAction(instance, SubmitAction.Insert);
        }

        /// <summary>
        /// Inserts the or update on submit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void InsertOrUpdateOnSubmit<T>(this ISessionTable<T> table, T instance)
        {
            table.SetSubmitAction(instance, SubmitAction.InsertOrUpdate);
        }

        /// <summary>
        /// Inserts the or update on submit.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void InsertOrUpdateOnSubmit(this ISessionTable table, object instance)
        {
            table.SetSubmitAction(instance, SubmitAction.InsertOrUpdate);
        }

        /// <summary>
        /// Updates the on submit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void UpdateOnSubmit<T>(this ISessionTable<T> table, T instance)
        {
            table.SetSubmitAction(instance, SubmitAction.Update);
        }

        /// <summary>
        /// Updates the on submit.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void UpdateOnSubmit(this ISessionTable table, object instance)
        {
            table.SetSubmitAction(instance, SubmitAction.Update);
        }

        /// <summary>
        /// Deletes the on submit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void DeleteOnSubmit<T>(this ISessionTable<T> table, T instance)
        {
            table.SetSubmitAction(instance, SubmitAction.Delete);
        }

        /// <summary>
        /// Deletes the on submit.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        public static void DeleteOnSubmit(this ISessionTable table, object instance)
        {
            table.SetSubmitAction(instance, SubmitAction.Delete);
        }
    }
}