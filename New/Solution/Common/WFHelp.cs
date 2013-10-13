using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Runtime.DurableInstancing;
using System.Activities.XamlIntegration;

namespace Common
{
    /// <summary>
    /// 工作流辅助类
    /// </summary>
    public class WFHelp       //在1.2版本中修改
    {
        private string bookMark = string.Empty;
        private string path = string.Empty;
        private string connectionString = string.Empty;
        private InstanceStore _instanceStore;
        public WFHelp()
        {
            this.bookMark = "BookmarkName";
            this.path = System.Web.HttpContext.Current.Server.MapPath("~/WF/Activity1.xaml");
            this.connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }
        public WFHelp(string bookMark, string path, string connectionString)
        {
            this.bookMark = bookMark;
            this.path = path;
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 加载工作流
        /// </summary>
        /// <param name="id">工作流的唯一标示</param>
        /// <param name="bookMark">标签名称</param>
        /// <param name="ids">恢复指定名称的书签的时候，传入的参数</param>
        /// <returns>工作流的加载的状态</returns>
        public string Load(string id, object inputs = null)
        {
            _instanceStore = new SqlWorkflowInstanceStore(connectionString);
            InstanceView view = _instanceStore.Execute
                (_instanceStore.CreateInstanceHandle(),
                new CreateWorkflowOwnerCommand(),
                TimeSpan.FromSeconds(30));
            _instanceStore.DefaultInstanceOwner = view.InstanceOwner;

            WorkflowApplication i = new WorkflowApplication(ActivityXamlServices.Load(path));
            i.InstanceStore = _instanceStore;
            i.PersistableIdle = (waiea) => PersistableIdleAction.Unload;
            i.Load(new Guid(id));
            return i.ResumeBookmark(bookMark, inputs).GetString();

        }
        /// <summary>
        /// 创建工作流
        /// </summary>
        /// <param name="parameters">传入的参数</param>
        /// <returns>获取工作流实例的Id值</returns>
        public string Create(IDictionary<string, object> parameters)
        {
            _instanceStore = new SqlWorkflowInstanceStore(connectionString);
            InstanceView view = _instanceStore.Execute
                (_instanceStore.CreateInstanceHandle(),
                new CreateWorkflowOwnerCommand(),
                TimeSpan.FromSeconds(30));
            _instanceStore.DefaultInstanceOwner = view.InstanceOwner;

            WorkflowApplication i = new WorkflowApplication(ActivityXamlServices.Load(path), parameters);
            i.InstanceStore = _instanceStore;
            i.PersistableIdle = (waiea) => PersistableIdleAction.Unload;
            i.Run();
            return i.Id.ToString();

        }
    }
}