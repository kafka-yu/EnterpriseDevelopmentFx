//--------------------------文档信息----------------------------
//       
//                 文件名: WinRar                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Common.IO
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/5 23:44:56
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace NkjSoft.Common.IO
{
    //源自:博客园
    /// <summary>
    /// 
    /// </summary>
    public class WinRar
    {
        /// <summary>
        /// RAs the rsave.
        /// </summary>
        /// <param name="patch">The patch.</param>
        /// <param name="rarPatch">The rar patch.</param>
        /// <param name="rarName">Name of the rar.</param>
        public void RARsave(string patch, string rarPatch, string rarName)
        {
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                the_Reg = Registry.ClassesRoot.OpenSubKey(@"ApplicationsWinRAR.exeShellOpenCommand");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                Directory.CreateDirectory(patch);
                //命令参数
                //the_Info = " a    " + rarName + "  " + @"C:Test70821.txt"; //文件压缩
                the_Info = " a    " + rarName + "  " + patch + "  -r"; ;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //打包文件存放目录
                the_StartInfo.WorkingDirectory = rarPatch;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Uns the RAR.
        /// </summary>
        /// <param name="unRarPatch">The un rar patch.</param>
        /// <param name="rarPatch">The rar patch.</param>
        /// <param name="rarName">Name of the rar.</param>
        /// <returns></returns>
        public string unRAR(string unRarPatch, string rarPatch, string rarName)
        {
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                the_Reg = Registry.ClassesRoot.OpenSubKey(@"ApplicationsWinRAR.exeShellOpenCommand");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                Directory.CreateDirectory(unRarPatch);
                the_Info = "e   " + rarName + "  " + unRarPatch + " -y";
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = rarPatch;//获取压缩包路径
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return unRarPatch;
        }

    }
}
