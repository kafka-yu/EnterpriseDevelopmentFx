// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumerateOnce<T> : IEnumerable<T>, IEnumerable
    {
        IEnumerable<T> enumerable;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerateOnce&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        public EnumerateOnce(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var en = Interlocked.Exchange(ref enumerable, null);
            if (en != null)
            {
                return en.GetEnumerator();
            }
            throw new Exception("Enumerated more than once.");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}