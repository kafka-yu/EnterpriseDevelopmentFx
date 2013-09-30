// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// 表示一个提供对特定类型进行延迟加载的方法。
    /// </summary>
    public interface IDeferLoadable
    {
        /// <summary> 
        /// 获取一个 <see cref="System.Boolean"/> 值 ，该值表示是否对类型实例进行延迟加载。
        /// </summary>
        /// <value><c>true</c> if this instance is loaded; otherwise, <c>false</c>.</value>
        bool IsLoaded { get; }
        /// <summary>
        /// 加载实例对象。
        /// </summary>
        void Load();
    }

    /// <summary>
    /// 定义能够被用于延迟加载的集合。
    /// </summary>
    public interface IDeferredList : IList, IDeferLoadable
    {
    }

    /// <summary>
    ///  定义能够被用于延迟加载的的泛型集合。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDeferredList<T> : IList<T>, IDeferredList
    {
    }

    /// <summary> 
    /// 一个实现了运行进行延迟加载的泛型集合列表。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeferredList<T> : IDeferredList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, IDeferLoadable
    {
        IEnumerable<T> source;
        List<T> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeferredList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public DeferredList(IEnumerable<T> source)
        {
            this.source = source;
        }

        /// <summary>
        /// 加载实例对象。
        /// </summary>
        public void Load()
        {
            this.values = new List<T>(this.source);
        }

        /// <summary>
        /// 获取一个 <see cref="System.Boolean"/> 值 ，该值表示是否对类型实例进行延迟加载。
        /// </summary>
        /// <value><c>true</c> if this instance is loaded; otherwise, <c>false</c>.</value>
        public bool IsLoaded
        {
            get { return this.values != null; }
        }

        private void Check()
        {
            if (!this.IsLoaded)
            {
                this.Load();
            }
        }

        #region IList<T> Members

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            this.Check();
            return this.values.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, T item)
        {
            this.Check();
            this.values.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            this.Check();
            this.values.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                this.Check();
                return this.values[index];
            }
            set
            {
                this.Check();
                this.values[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            this.Check();
            this.values.Add(item);
        }

        public void Clear()
        {
            this.Check();
            this.values.Clear();
        }

        public bool Contains(T item)
        {
            this.Check();
            return this.values.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Check();
            this.values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { this.Check(); return this.values.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            this.Check();
            return this.values.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            this.Check();
            return this.values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IList Members

        /// <summary>
        /// 将某项添加到 <see cref="T:System.Collections.IList"/> 中。
        /// </summary>
        /// <param name="value">要添加到 <see cref="T:System.Collections.IList"/> 的 <see cref="T:System.Object"/>。</param>
        /// <returns>新元素的插入位置。</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="T:System.Collections.IList"/> 为只读。- 或 - <see cref="T:System.Collections.IList"/> 具有固定大小。</exception>
        public int Add(object value)
        {
            this.Check();
            return ((IList)this.values).Add(value);
        }

        public bool Contains(object value)
        {
            this.Check();
            return ((IList)this.values).Contains(value);
        }

        public int IndexOf(object value)
        {
            this.Check();
            return ((IList)this.values).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            this.Check();
            ((IList)this.values).Insert(index, value);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            this.Check();
            ((IList)this.values).Remove(value);
        }

        object IList.this[int index]
        {
            get
            {
                this.Check();
                return ((IList)this.values)[index];
            }
            set
            {
                this.Check();
                ((IList)this.values)[index] = value;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            this.Check();
            ((IList)this.values).CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        #endregion
    }
}