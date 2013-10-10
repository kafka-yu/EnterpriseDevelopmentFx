using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Framework.Web.Security
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class RequirePermission : ActionDefinitionAttrbute
    {
        public RequirePermission(string name, bool showAsMenu = false)
        {
            base.Name = name;
            base.ShowAsMenu = showAsMenu;
        }
    }
}
