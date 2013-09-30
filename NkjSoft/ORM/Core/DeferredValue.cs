// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// 表示延迟加载的泛型类型列表。
    /// </summary>
    /// <typeparam name="T">被延迟加载的类型。</typeparam>
    public struct DeferredValue<T> : IDeferLoadable
    {
        /// <summary>
        /// 表示延迟加载的泛型类型列表。
        /// </summary>
        IEnumerable<T> source;
        /// <summary>
        /// 获取一个值，该值表示列表是否已经加载了。
        /// </summary>
        bool loaded;
        /// <summary>
        /// 延迟加载的类型。
        /// </summary>
        T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeferredValue&lt;T&gt;"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public DeferredValue(T value)
        {
            this.value = value;
            this.source = null;
            this.loaded = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeferredValue&lt;T&gt;"/> struct.
        /// </summary>
        /// <param name="source">The source.</param>
        public DeferredValue(IEnumerable<T> source)
        {
            this.source = source;
            this.loaded = false;
            this.value = default(T);
        }

        /// <summary>
        /// 加载实例对象。
        /// </summary>
        public void Load()
        {
            if (this.source != null)
            {
                this.value = this.source.SingleOrDefault();
                this.loaded = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value><c>true</c> if this instance is loaded; otherwise, <c>false</c>.</value>
        public bool IsLoaded
        {
            get { return this.loaded; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is assigned.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is assigned; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssigned
        {
            get { return this.loaded && this.source == null; }
        }

        private void Check()
        {
            if (!this.IsLoaded)
            {
                this.Load();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get
            {
                this.Check();
                return this.value;
            }

            set
            {
                this.value = value;
                this.loaded = true;
                this.source = null;
            }
        }
    }
}