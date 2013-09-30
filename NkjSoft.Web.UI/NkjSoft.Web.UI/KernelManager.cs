using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Ninject;
using Ninject.Extensions.Xml;
using Ninject.Extensions.Xml.Processors;
using Ninject.Modules;
using Ninject.Syntax;

namespace NkjSoft.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class KernelManager
    {
        /// <summary>
        /// 
        /// </summary>
        private static IKernel _kernel = null;

        /// <summary>
        /// 
        /// </summary>
        public string ConfigFileName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static IKernel Kernel
        {
            get
            {
                return _kernel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFilename"></param>
        public KernelManager(string configFilename)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFilename"></param>
        public static void Initialize(string configFilename)
        {
            var _setting = new NinjectSettings();
            _setting.LoadExtensions = false;

            var _module = new XmlExtensionModule();
            _kernel = new StandardKernel(_setting, _module);

            if (_kernel != null)
            {
                _kernel.Load(configFilename);
            }
        }
    }


}