using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Framework.Web.Security
{
    public class ActionDefinitionAttrbute : Attribute
    {
        public string Name { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public bool ShowAsMenu { get; set; }
    }
}
