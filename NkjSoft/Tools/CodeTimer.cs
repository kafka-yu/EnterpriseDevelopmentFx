//--------------------------版权信息----------------------------
//       
//                 文件名: CodeTimers                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: MyToolLib
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/3/12 9:41:27
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace NkjSoft.Tools
{
    /// <summary>
    /// 从老赵那边学习到的一个.net 程序执行耗时测试器
    /// </summary>
    public static class CodeTimer
    {
        private static TestPlatform _testPlatform;

        /// <summary>
        /// 获取或设置测试平台区分。
        /// </summary>
        /// <value>The test platform.</value>
        public static TestPlatform TestPlatform
        {
            get { return _testPlatform; }
            set { _testPlatform = value; }
        }
        /// <summary>
        /// 初始化,并预热测试器
        /// </summary>
        public static void Initialize(TestPlatform testplatform)
        {
            _testPlatform = testplatform;
            //将当前程序的CPU权限设置为最高
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            //预热
            Test(string.Empty, 1, () => { });
        }

        /// <summary>
        /// 将需要进行测试的方法传递进入测试（面向控制台的测试环境）
        /// </summary>
        /// <param name="name">测试名称:进行空测试请传入String.Empty or Null</param>
        /// <param name="iterationTimes"> 
        /// 设置 反复执行的次数<para>注意当此数值>>100万时,运行测试的时候请注意需要等待时间</para>
        /// </param>
        /// <param name="action">测试方法</param>
        public static void Test(string name, int iterationTimes, Action action)
        {
            Func<ulong> getCycleCount = null;
            getCycleCount = () => _testPlatform == TestPlatform.Vista7 ? GetCycleCount() : GetCurrentThreadTimes();
            //判断空 ,返回
            if (String.IsNullOrEmpty(name)) return;

            Console.WriteLine("测试名称: {0}", name);
            Console.WriteLine("重复次数: {0}\r\n-------------------------\r\n", iterationTimes.ToString());

            // 1.
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            //测试 第一代
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced); //强制先执行垃圾回收.排除其他干扰
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = getCycleCount();
            //反复执行 被测试方法
            for (int i = 0; i < iterationTimes; i++)
                action();
            ulong cpuCycles = getCycleCount() - cycleCount;
            watch.Stop();

            // 4.
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));

            // 5.
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();

        }

        /// <summary>
        /// 将需要进行测试的方法传递进入测试（自定义处理测试结果）
        /// </summary>
        /// <param name="name">测试名称:进行空测试请传入String.Empty or Null</param>
        /// <param name="iterationTimes">设置 反复执行的次数<para>注意当此数值&gt;&gt;100万时,运行测试的时候请注意需要等待时间</para></param>
        /// <param name="action">测试方法</param>
        /// <param name="timeInfo">输出信息....</param>
        public static void Test(string name, int iterationTimes, Action action, Action<StringBuilder> timeInfo)
        {
            Func<ulong> getCycleCount = null;
            getCycleCount = () => _testPlatform == TestPlatform.Vista7 ? GetCycleCount() : GetCurrentThreadTimes();
            //判断空 ,返回
            if (String.IsNullOrEmpty(name)) return;

            StringBuilder sb = new StringBuilder("");
            sb.AppendFormat("测试名称: {0}\r\n", name);
            sb.AppendFormat("重复次数: {0}\r\n-------------------------\r\n", iterationTimes.ToString());

            // 1. 

            //测试 第一代
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced); //强制先执行垃圾回收.排除其他干扰
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = getCycleCount();
            //反复执行 被测试方法
            for (int i = 0; i < iterationTimes; i++)
                action();
            ulong cpuCycles = getCycleCount() - cycleCount;
            watch.Stop();

            // 4. 
            sb.AppendLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms\r\n");
            sb.AppendLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0") + "\r\n");

            // 5.
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                sb.AppendLine("\tGen " + i + ": \t\t" + count + "\r\n");
            }
            if (timeInfo != null)
                timeInfo(sb);

        }

        /// <summary>
        /// 进行一次默认的基于 Windows Vista/7 的 指定次数的测试。
        /// </summary>
        /// <param name="iterationTimes">指定测试的次数</param>
        /// <param name="action">测试活动</param>
        public static void DefaultTest(int iterationTimes, Action action)
        {
            Test("Default Test", iterationTimes, action);
        }


        #region --- Win 7 / Vista ---
        /// <summary>
        /// 获取一个周期 回收器回收的数量
        /// </summary>
        /// <returns></returns>
        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        // 平台调用
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();
        #endregion

        #region --- Win XP / 2003  ---
        private static ulong GetCurrentThreadTimes()
        {
            long l;
            long kernelTime, userTimer;
            GetThreadTimes(GetCurrentThread(), out l, out l, out kernelTime,
               out userTimer);
            return (ulong)(kernelTime + userTimer);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime,
           out long lpExitTime, out long lpKernelTime, out long lpUserTime);
        #endregion



    }

    /// <summary>
    /// 定义一组表示 <see cref="CodeTimer"/> 支持的的测试平台。
    /// </summary>
    public enum TestPlatform
    {
        /// <summary>
        /// 在 Win Vista / 7 /2008 平台(默认)
        /// </summary>
        Vista7,
        /// <summary>
        /// 在 Win Xp/2003 平台
        /// </summary>
        WindowdXP2003,


    }
}
