//--------------------------文档信息----------------------------
//       
//                 文件名: AnonymityExtensions                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Extensions
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/9/11 1:38:51
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NkjSoft.Extensions.Special
{
    /// <summary>
    /// 封装对匿名类型对象的扩展方法。
    /// <list type="string">俞如凯</list>
    /// <list type="DateTime">2010/9/11</list>
    /// </summary>
    public static class AnonymityExtensions
    {
        /// <summary>
        /// 将当前类型强制转换成目标类型。目标类型由参数 <paramref name="getter"/> 推断得到，该方法可以用于将匿名类型从外部引入到当前执行上下文。该方法等同  IfNullDefault&lt;TResult&gt;方法的实现。
        /// </summary>
        /// <typeparam name="T">返回结果List的元素的类型</typeparam>
        /// <param name="obj">需要被转换的类型。</param>
        /// <param name="getter">类型推断器</param>
        /// <returns>目标类型&lt;T&gt;实例</returns>
        /// <exception cref="System.NullReferenceException">空引用异常</exception>
        /// <exception cref="System.InvalidCastException">无法进行的转换异常</exception> 
        public static T CastAsType<T>(this object obj, T getter) where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// 将指定的 object 类型(匿名类型,该类型必须是实现了 IEnumerable 接口的类型)，转换成指定的类型集合。
        /// </summary>
        /// <typeparam name="T">返回结果List的元素的类型</typeparam>
        /// <param name="obj">需要被转换的对象,</param>
        /// <param name="getter">类型推断器.</param>
        /// <returns>指定的&lt;T&gt;类型集合。</returns>
        /// <exception cref="System.NullReferenceException">空引用异常</exception>
        /// <exception cref="System.InvalidCastException">无法进行的转换异常</exception> 
        public static IEnumerable<T> CastAsIEnumerable<T>(this object obj, T getter) where T : class
        {
            return obj as IEnumerable<T>;
        }


        /// <summary>
        /// 将指定的 object 类型(匿名类型,该类型必须是实现了 IEnumerable 接口的类型)，转换成指定的 <typeparamref name="T"/> 类型数组。
        /// </summary>
        /// <typeparam name="T">返回结果数组的元素的类型</typeparam>
        /// <param name="obj">需要被转换的对象数组，比如一个匿名类型数组.</param>
        /// <param name="getter">类型推断器</param>
        /// <returns>指定的&lt;T&gt;类型数组。</returns>
        /// <exception cref="System.NullReferenceException">空引用异常</exception>
        /// <exception cref="System.InvalidCastException">无法进行的转换异常</exception> 
        public static T[] CastAsArray<T>(this object obj, T getter) where T : class
        {
            return obj as T[];

        }

        /// <summary>
        /// Casts as queryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="getter">The getter.</param>
        /// <returns></returns>
        public static IQueryable<T> CastAsQueryable<T>(this object obj, T getter) where T : class
        {
            return obj as IQueryable<T>;
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ConstructorDictionary<T>
    {
        /// <summary>
        /// Gets or sets the ctor info.
        /// </summary>
        /// <value>The ctor info.</value>
        public static ConstructorInfo CtorInfo { private set; get; }

        /// <summary>
        /// Initializes the <see cref="ConstructorDictionary&lt;T&gt;"/> class.
        /// </summary>
        static ConstructorDictionary()
        {
            CtorInfo = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                            null, new Type[0], null);
        }
    }

    /// <summary>
    /// 用于重置类对象值的扩展。
    /// </summary>
    public static class ResetExtension
    {
        /// <summary>
        /// 重置当前类对象（在当前对象的类中有显示构造函数的时候,重新调用构造函数,将全部字段、属性值重置为默认值）。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        public static void Reset<T>(this T obj) where T : new()
        {
            ConstructorDictionary<T>.CtorInfo.Invoke(obj, null);
        }
    }

}
