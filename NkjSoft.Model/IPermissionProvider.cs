using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Model
{
    /// <summary>
    /// 表示提供权限信息的对象。
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        string ActionName { get; set; }
    }
}
