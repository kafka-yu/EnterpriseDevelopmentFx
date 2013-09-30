// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReadOnlyExtensions
    {
        /// <summary>
        /// 将当前 <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> 转换成只读的 <see cref="System.Collections.ObjectModel.ReadOnlyCollection&lt;T&gt;"/> 版本。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> collection)
        {
            ReadOnlyCollection<T> roc = collection as ReadOnlyCollection<T>;
            if (roc == null)
            {
                if (collection == null)
                {
                    roc = EmptyReadOnlyCollection<T>.Empty;
                }
                else
                {
                    roc = new List<T>(collection).AsReadOnly();
                }
            }
            return roc;
        }

        class EmptyReadOnlyCollection<T>
        {
            internal static readonly ReadOnlyCollection<T> Empty = new List<T>().AsReadOnly();
        }
    }
}
