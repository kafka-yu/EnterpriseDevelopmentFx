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
    [DataContract]
    public class QueryParameter
    {
        [DataMember]
        public string Field1 { get; set; }
        [DataMember]
        public object Value1 { get; set; }
        [DataMember]
        public string Field2 { get; set; }
        [DataMember]
        public object Value2 { get; set; }

        [DataMember]
        public string Operation { get; set; }
    }
}
