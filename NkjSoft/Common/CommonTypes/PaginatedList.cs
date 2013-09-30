//--------------------------文档信息----------------------------
//       
//                 文件名: PaginatedList                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Common.CommonTypes
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/16 11:17:38
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Common.CommonTypes
{
    /// <summary>
    /// 提供进行分页的强类型序列。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {

        /// <summary>
        /// 获取或设置当前页索引。
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; private set; }
        /// <summary>
        /// 获取或设置每页记录数。
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; private set; }
        /// <summary>
        /// 获取或设置记录项总数。
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; private set; }
        /// <summary>
        /// 获取或设置页总数。
        /// </summary>
        /// <value>The total pages.</value>
        public int TotalPages { get; private set; }

        /// <summary>
        /// 实例化新的 <see cref="PaginatedList&lt;T&gt;"/> 实例.
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pageIndex">当前页索引.</param>
        /// <param name="pageSize">每页记录项数.</param>
        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }

        /// <summary>
        /// 获取一个值，表示是否还有上一页。
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        /// <summary>
        /// 获取一个值，表示是否还有下一页。
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }

        /// <summary>
        /// 转到下一页.
        /// </summary>
        public void MoveNext()
        { 
        }

        /// <summary>
        /// 转到向下的指定页.
        /// </summary>
        /// <param name="pageIndex">新页索引</param>
        public void MoveNext(int pageIndex)
        { 
        }

        /// <summary>
        /// 转到上一页.
        /// </summary>
        public void MovePrevious() { }
        /// <summary>
        /// 转到向上的指定页.
        /// </summary>
        /// <param name="pageIndex">新页索引.</param>
        public void MovePrevious(int  pageIndex) { }

        /// <summary>
        /// 转到首页.
        /// </summary>
        public void MoveTop() { }
        /// <summary>
        /// 转到尾页.
        /// </summary>
        public void MoveEnd() { }
    }
}
