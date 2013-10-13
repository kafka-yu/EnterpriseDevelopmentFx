using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using NkjSoft.DAL;
using NkjSoft.BLL;
using Common;
using System.IO;
using System.Text;
using NkjSoft.IBLL;

namespace  Models
{
    /// <summary>
    /// 是否记录日志
    /// </summary>
    public enum LogOpration
    {
        /// <summary>
        /// 根据Web.config中的配置
        /// </summary>
        Default,
        /// <summary>
        /// 开启记录日志
        /// </summary>
        Start,
        /// <summary>
        /// 禁止记录日志
        /// </summary>
        Fobid
    }

    public class LogClassModels : System.Web.SessionState.IRequiresSessionState
    {
        public static void WriteServiceLog(string message, string logType, LogOpration logOpration = LogOpration.Default)
        {
            try
            {
                //logOpration设置优先级高于配置节logEnabled
                bool logEnabled = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["LogEnabled"]);
                if (logOpration == LogOpration.Fobid || (logOpration == LogOpration.Default && !logEnabled))
                {
                    return;
                }
                else if (logOpration == LogOpration.Start || (logOpration == LogOpration.Default && logEnabled))
                {
                    SysLog sysLog = new SysLog();
                    sysLog.Id = Result.GetNewId();
                    sysLog.CreateTime = DateTime.Now;
                    sysLog.Ip = Common.IP.GetIP();
                    sysLog.Message = message;
                    sysLog.CreatePerson = AccountModel.GetCurrentPerson();
                    sysLog.MenuId = logType;//哪个模块生成的日志

                    ISysLogBLL sysLogRepository = new SysLogBLL();

                    ValidationErrors validationErrors = new ValidationErrors();
                    sysLogRepository.Create(ref validationErrors, sysLog);
                    return;

                }
            }
            catch (Exception ep)
            {
                try
                {
                    string path = @"mylog.txt";
                    string txtPath = System.Web.HttpContext.Current.Server.MapPath(path);//获取绝对路径
                    using (StreamWriter sw = new StreamWriter(txtPath, true, Encoding.Default))
                    {
                        sw.WriteLine((ep.Message + "|" + message + "|" + Common.IP.GetIP() + DateTime.Now.ToString()).ToString());
                        sw.Close();
                    }
                    return;
                }
                catch { return; }
            }

        }
     

        public void Dispose()
        {

        }
    }
}
