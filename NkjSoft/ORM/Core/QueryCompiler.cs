// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection; 

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// Creates a reusable, parameterized representation of a query that caches the execution plan
    /// </summary>
    public static class QueryCompiler
    {
        /// <summary>
        /// Compiles the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static Delegate Compile(LambdaExpression query)
        {
            CompiledQuery cq = new CompiledQuery(query);
            return StrongDelegate.CreateDelegate(query.Type, (Func<object[], object>)cq.Invoke);
        }

        /// <summary>
        /// 预编译一个指定的 Lambda 查询表达式。
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="query">指定的 Lambda 查询表达式</param>
        /// <returns></returns>
        public static D Compile<D>(Expression<D> query)
        {
            return (D)(object)Compile((LambdaExpression)query);
        }

        /// <summary>
        /// 预编译一个指定的 Lambda 查询表达式。并得到指定的结果类型。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型</typeparam>
        /// <param name="query">指定的 Lambda 查询表达式</param>
        /// <returns></returns>
        public static Func<TResult> Compile<TResult>(Expression<Func<TResult>> query)
        {
            return new CompiledQuery(query).Invoke<TResult>;
        }

        /// <summary>
        /// 提供参数并预编译一个指定的 Lambda 查询表达式。并得到指定的结果类型。
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static Func<T1, TResult> Compile<T1, TResult>(Expression<Func<T1, TResult>> query)
        {
            return new CompiledQuery(query).Invoke<T1, TResult>;
        }

        /// <summary>
        /// Compiles the specified query.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static Func<T1, T2, TResult> Compile<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> query)
        {
            return new CompiledQuery(query).Invoke<T1, T2, TResult>;
        }

        /// <summary>
        /// Compiles the specified query.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static Func<T1, T2, T3, TResult> Compile<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> query)
        {
            return new CompiledQuery(query).Invoke<T1, T2, T3, TResult>;
        }

        /// <summary>
        /// Compiles the specified query.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static Func<T1, T2, T3, T4, TResult> Compile<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> query)
        {
            return new CompiledQuery(query).Invoke<T1, T2, T3, T4, TResult>;
        }

        /// <summary>
        /// Compiles the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Func<IEnumerable<T>> Compile<T>(this IQueryable<T> source)
        {
            return Compile<IEnumerable<T>>(
                Expression.Lambda<Func<IEnumerable<T>>>(((IQueryable)source).Expression)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        public class CompiledQuery
        {
            LambdaExpression query;
            Delegate fnQuery;

            internal CompiledQuery(LambdaExpression query)
            {
                this.query = query;
            }

            /// <summary>
            /// Gets the query.
            /// </summary>
            /// <value>The query.</value>
            public LambdaExpression Query
            {
                get { return this.query; }
            }

            internal void Compile(params object[] args)
            {
                if (this.fnQuery == null)
                {
                    // first identify the query provider being used
                    Expression body = this.query.Body;

                    // ask the query provider to compile the query by 'executing' the lambda expression
                    IQueryProvider provider = this.FindProvider(body, args);
                    if (provider == null)
                    {
                        throw new InvalidOperationException("Could not find query provider");
                    }

                    Delegate result = (Delegate)provider.Execute(this.query);
                    System.Threading.Interlocked.CompareExchange(ref this.fnQuery, result, null);
                }
            }

            /// <summary>
            /// Finds the provider.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            internal IQueryProvider FindProvider(Expression expression, object[] args)
            {
                Expression root = this.FindProviderInExpression(expression) as ConstantExpression;
                if (root == null && args != null && args.Length > 0)
                {
                    Expression replaced = ExpressionReplacer.ReplaceAll(
                        expression,
                        this.query.Parameters.ToArray(),
                        args.Select((a, i) => Expression.Constant(a, this.query.Parameters[i].Type)).ToArray()
                        );
                    root = this.FindProviderInExpression(replaced);
                }
                if (root != null) 
                {
                    ConstantExpression cex = root as ConstantExpression;
                    if (cex == null)
                    {
                        cex = PartialEvaluator.Eval(root) as ConstantExpression;
                    }
                    if (cex != null)
                    {
                        IQueryProvider provider = cex.Value as IQueryProvider;
                        if (provider == null)
                        {
                            IQueryable query = cex.Value as IQueryable;
                            if (query != null)
                            {
                                provider = query.Provider;
                            }
                        }
                        return provider;
                    }
                }
                return null;
            }

            /// <summary>
            /// Finds the provider in expression.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            private Expression FindProviderInExpression(Expression expression)
            {
                Expression root = TypedSubtreeFinder.Find(expression, typeof(IQueryProvider));
                if (root == null)
                {
                    root = TypedSubtreeFinder.Find(expression, typeof(IQueryable));
                }
                return root;
            }

            /// <summary>
            /// Invokes the specified args.
            /// </summary>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public object Invoke(object[] args)
            {
                this.Compile(args);
                if (invoker == null)
                {
                    invoker = GetInvoker();
                }
                if (invoker != null)
                {
                    return invoker(args);
                }
                else
                {
                    try
                    {
                        return this.fnQuery.DynamicInvoke(args);
                    }
                    catch (TargetInvocationException tie)
                    {
                        throw tie.InnerException;
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            Func<object[], object> invoker;
            bool checkedForInvoker;

            /// <summary>
            /// Gets the invoker.
            /// </summary>
            /// <returns></returns>
            private Func<object[], object> GetInvoker()
            {
                if (this.fnQuery != null && this.invoker == null && !checkedForInvoker)
                {
                    this.checkedForInvoker = true;
                    Type fnType = this.fnQuery.GetType();
                    if (fnType.FullName.StartsWith("System.Func`"))
                    {
                        var typeArgs = fnType.GetGenericArguments();
                        MethodInfo method = this.GetType().GetMethod("FastInvoke"+typeArgs.Length, BindingFlags.Public|BindingFlags.Instance);
                        if (method != null)
                        {
                            this.invoker = (Func<object[], object>)Delegate.CreateDelegate(typeof(Func<object[], object>), this, method.MakeGenericMethod(typeArgs));
                        }
                    }
                }
                return this.invoker;
            }

            /// <summary>
            /// Fasts the invoke1.
            /// </summary>
            /// <typeparam name="R"></typeparam>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public object FastInvoke1<R>(object[] args)
            {
                return ((Func<R>)this.fnQuery)();
            }

            /// <summary>
            /// Fasts the invoke2.
            /// </summary>
            /// <typeparam name="A1">The type of the 1.</typeparam>
            /// <typeparam name="R"></typeparam>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public object FastInvoke2<A1, R>(object[] args)
            {
                return ((Func<A1, R>)this.fnQuery)((A1)args[0]);
            }

            /// <summary>
            /// Fasts the invoke3.
            /// </summary>
            /// <typeparam name="A1">The type of the 1.</typeparam>
            /// <typeparam name="A2">The type of the 2.</typeparam>
            /// <typeparam name="R"></typeparam>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public object FastInvoke3<A1, A2, R>(object[] args)
            {
                return ((Func<A1, A2, R>)this.fnQuery)((A1)args[0], (A2)args[1]);
            }

            /// <summary>
            /// Fasts the invoke4.
            /// </summary>
            /// <typeparam name="A1">The type of the 1.</typeparam>
            /// <typeparam name="A2">The type of the 2.</typeparam>
            /// <typeparam name="A3">The type of the 3.</typeparam>
            /// <typeparam name="R"></typeparam>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public object FastInvoke4<A1, A2, A3, R>(object[] args)
            {
                return ((Func<A1, A2, A3, R>)this.fnQuery)((A1)args[0], (A2)args[1], (A3)args[2]);
            }

            /// <summary>
            /// Fasts the invoke5.
            /// </summary>
            /// <typeparam name="A1">The type of the 1.</typeparam>
            /// <typeparam name="A2">The type of the 2.</typeparam>
            /// <typeparam name="A3">The type of the 3.</typeparam>
            /// <typeparam name="A4">The type of the 4.</typeparam>
            /// <typeparam name="R"></typeparam>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public object FastInvoke5<A1, A2, A3, A4, R>(object[] args)
            {
                return ((Func<A1, A2, A3, A4, R>)this.fnQuery)((A1)args[0], (A2)args[1], (A3)args[2], (A4)args[3]);
            }

            /// <summary>
            /// Invokes this instance.
            /// </summary>
            /// <typeparam name="TResult">The type of the result.</typeparam>
            /// <returns></returns>
            internal TResult Invoke<TResult>()
            {
                this.Compile(null);
                return ((Func<TResult>)this.fnQuery)();
            }

            internal TResult Invoke<T1, TResult>(T1 arg)
            {
                this.Compile(arg);
                return ((Func<T1, TResult>)this.fnQuery)(arg);
            }

            internal TResult Invoke<T1, T2, TResult>(T1 arg1, T2 arg2)
            {
                this.Compile(arg1, arg2);
                return ((Func<T1, T2, TResult>)this.fnQuery)(arg1, arg2);
            }

            internal TResult Invoke<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3)
            {
                this.Compile(arg1, arg2, arg3);
                return ((Func<T1, T2, T3, TResult>)this.fnQuery)(arg1, arg2, arg3);
            }

            /// <summary>
            /// Invokes the specified arg1.
            /// </summary>
            /// <typeparam name="T1">The type of the 1.</typeparam>
            /// <typeparam name="T2">The type of the 2.</typeparam>
            /// <typeparam name="T3">The type of the 3.</typeparam>
            /// <typeparam name="T4">The type of the 4.</typeparam>
            /// <typeparam name="TResult">The type of the result.</typeparam>
            /// <param name="arg1">The arg1.</param>
            /// <param name="arg2">The arg2.</param>
            /// <param name="arg3">The arg3.</param>
            /// <param name="arg4">The arg4.</param>
            /// <returns></returns>
            internal TResult Invoke<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                this.Compile(arg1, arg2, arg3, arg4);
                return ((Func<T1, T2, T3, T4, TResult>)this.fnQuery)(arg1, arg2, arg3, arg4);
            }
        }
    }
}