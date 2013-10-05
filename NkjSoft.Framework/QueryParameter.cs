using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NkjSoft.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryParameter
    {
        public string Field1 { get; set; }

        public object Value1 { get; set; }
        
        public string Field2 { get; set; }
        
        public object Value2 { get; set; }

        public string Operation { get; set; }
    }
}
