using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace NkjSoft.WCF
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NkjSoftWCFClientBase<T> : ClientBase<T> where T : class
    {
        private T channel;

        /// <summary>
        /// 
        /// </summary>
        public T Fn
        {
            get
            {
                return channel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NkjSoftWCFClientBase()
        {
            channel = new ChannelFactory<T>(System.Configuration
                .ConfigurationManager.AppSettings["ClientConfigName"])
                .CreateChannel();
        }
    }
}
