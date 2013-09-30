//--------------------------版权信息----------------------------
//       
//                 文件名: ExceptionPolice                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: NkjSoft.Utility.ExceptionBlock
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/23 17:53:33
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;

namespace NkjSoft.Utility
{
    namespace ExceptionBlock
    {
        /// <summary>
        /// 描述一个异常处理模块，提供统一的异常处理机制。无法继承此类。
        /// </summary>
        public sealed class ExceptionPolice
        {
            /// <summary>
            /// 在执行的操作发生异常的时候发生。
            /// </summary>
            public event EventHandler<ExceptionEventArgs> Exceptioning;


            /// <summary>
            /// 在异常处理环境下执行指定的操作。
            /// </summary>
            /// <param name="action">执行指定的操作</param>
            public void Run(Action action)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
            }

            /// <summary>
            /// 在异常处理环境下执行指定的操作。
            /// </summary>
            /// <param name="action">执行指定的操</param>
            /// <param name="finallyAction">异常发生之后 finally 部分需要执行的操作.</param>
            public void Run(Action action, Action finallyAction)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                finally
                {
                    if (finallyAction != null)
                        finallyAction();
                }
            }

            /// <summary>
            /// 执行一个具有一个返回值的方法的操作。
            /// </summary>
            /// <typeparam name="TResult">返回值类型.</typeparam>
            /// <param name="action">需要执行的方法.</param>
            /// <returns></returns>
            public TResult Run<TResult>(Func<TResult> action)
            {
                TResult t = default(TResult);
                try
                {
                    t = action();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }

                return t;
            }


            /// <summary>
            /// 执行一个具有一个返回值的方法的操作。方法接收一个  <typeparamref name="TResult"/> 类型参数作为当执行的操作发生异常之后的返回值。
            /// </summary>
            /// <typeparam name="TResult">The type of the result.</typeparam>
            /// <param name="action">The action.</param>
            /// <param name="defaultIfException">The default if exception.</param>
            /// <returns></returns>
            public TResult Run<TResult>(Func<TResult> action, TResult defaultIfException)
            {
                TResult t = default(TResult);
                try
                {
                    t = action();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                finally
                {
                    t = defaultIfException;
                }
                return t;
            }

            /// <summary>
            /// 对异常进行处理的函数。
            /// </summary>
            /// <param name="ex">发生的异常信息。</param>
            private void OnException(Exception ex)
            {
                if (Exceptioning != null)
                    Exceptioning(this, new ExceptionEventArgs(ex));
            }

            /// <summary>
            ///  在Try-Catch的环境下执行指定的方法，该方法返回一个 <typeparam name="TResult">TResult</typeparam> 值。方法接收一个 System.Action&lt;<typeparam name="TResult">TResult</typeparam>,System.Exception&gt; callBack ,返回操作结果和捕捉到的异常。
            /// </summary>
            /// <typeparam name="TResult">The type of the result.</typeparam>
            /// <param name="actionReturnInt">定义指定要执行的操作。该方法必须返回一个确定类型的值的 Func&lt;&gt; 签名的委托引用.</param>
            /// <param name="callBack">回调.</param>
            public static void RunWithCatch<TResult>(Func<TResult> actionReturnInt, Action<TResult, Exception> callBack)
            {
                TResult rtv = default(TResult);
                Exception _ex = null;
                try
                {
                    rtv = actionReturnInt();
                }
                catch (Exception ex)
                {
                    _ex = ex;
                }

                callBack(rtv, _ex);

            }
        }

        /// <summary>
        /// 一个继承自 <see cref="System.EventArgs"/> 的类，对 <see cref="System.EventHandler&lt;TEventArgs&gt;"/> 事件处理函数提供数据。
        /// </summary>
        public class ExceptionEventArgs : EventArgs
        {
            /// <summary>
            /// 获取执行操作之后捕获的 <see cref="System.Exception"/> 信息。
            /// </summary>
            public Exception Exception { get; private set; }


            /// <summary>
            /// 实例化一个新的 <see cref="ExceptionEventArgs"/> 对象。
            /// </summary>
            /// <param name="e">表示执行操作时捕获的异常。.</param>
            public ExceptionEventArgs(Exception e)
            {
                this.Exception = e;
            }

            /// <summary>
            /// 获取当前异常事件参数的 <see cref="System.String"/> 信息。
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (this.Exception != null)
                    return this.Exception.ToString();
                return base.ToString();
            }
        }
    }
}
