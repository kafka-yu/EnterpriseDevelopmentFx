using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 表示生成器从数据获取的表映射，用于生成器生成。
    /// </summary>
    public sealed class Table
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }


        /// <summary>
        /// 获取或设置所属表的所有成员。
        /// </summary>
        public Column[] Columns { get; set; }
    }
}
