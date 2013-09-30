using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Model.Common
{
    public class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ActionName { get; set; }

        public string Controller { get; set; }

        public string PageUrl { get; set; }

        public string PermissionId { get; set; }
    }
}
