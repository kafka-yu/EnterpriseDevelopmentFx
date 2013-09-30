//--------------------------文档信息----------------------------
//       
//                 文件名: CommonEnums                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.CommonTypes
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/3 21:09:00
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.CommonTypes
{
    #region --- 枚举 ---
    /// <summary>
    /// 定义一组数,用于表示性别。
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 男性(0)
        /// </summary>
        Female = 0,
        /// <summary>
        /// 女性(1)
        /// </summary>
        Male = 1,

        /// <summary>
        /// 未记录.(或表示调用者没有填写)(-1)
        /// </summary>
        NREC = -1,
    }

    /// <summary>
    /// 定义一组值,表示与数据库操作相关的方法的返回结果信息。
    /// </summary>
    public enum MethodResult
    {
        /// <summary>
        /// 表示当前操作返回未知结果。（-1）
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 表示当前操作失败。(0)默认
        /// </summary>
        Failed = 0,

        /// <summary>
        /// 表示当前执行的操作成功完成。(1)
        /// </summary>
        Successfully = 1,


        /// <summary>
        /// 表示当前向数据库操作的记录已经存在。(3)
        /// </summary>
        RecordExists = 3,

        /// <summary>
        /// 表示在登录操作的时候输入的密码错误。(4)
        /// </summary>
        InputPwdError = 4,

        /// <summary>
        /// 表示相关的记录不存在。（5）
        /// </summary>
        RecordNone = 5,
    }


    /// <summary>
    /// 定义一组表示用于指定排序方式的值。
    /// </summary>
    public enum OrderBy
    {
        /// <summary>
        /// 升序.
        /// </summary>
        Asc = 1,

        /// <summary>
        /// 降序
        /// </summary>
        Desc = 2,

        /// <summary>
        /// 默认排序 = 升序
        /// </summary>
        Default = Asc,
    }
    #endregion

    #region --- 自定义辅助类型 ---
    /// <summary>
    /// 表示简单方法返回的参数信息。
    /// </summary>
    /// <typeparam name="T">返回的简单信息的类型</typeparam>
    public class MethodReturnInfo<T>
    {
        /// <summary>
        /// 获取或设置方法操作的返回类型。
        /// </summary>
        public MethodResult ReturnType { get; set; }

        /// <summary>
        /// 获取或设置附加的简单信息。
        /// </summary>
        public T SimpleInfo { get; set; }

        /// <summary>
        /// 获取或设置影响的整数信息。
        /// </summary>
        public int? Effects { get; set; }


        /// <summary>
        /// 实例化一个新的 <see cref="MethodReturnInfo&lt;T&gt;"/>。
        /// </summary>
        public MethodReturnInfo()
            : this(MethodResult.Unknown,
                -1, default(T))
        {
        }


        /// <summary>
        /// 实例化一个只定义方法操作返回类型信息的 <see cref="MethodReturnInfo&lt;T&gt;"/> 实例。
        /// </summary>
        /// <param name="rtnType">方法操作返回类型</param>
        public MethodReturnInfo(MethodResult rtnType)
            : this(rtnType, 0, default(T))
        {

        }

        /// <summary>
        /// 定义一个只定义了方法操作返回类型、影响的整数信息的 <see cref="MethodReturnInfo&lt;T&gt;"/> 实例.
        /// </summary>
        /// <param name="rtnType">方法操作返回类型.</param>
        /// <param name="efts">影响的整数信息</param>
        public MethodReturnInfo(MethodResult rtnType, int? efts)
            : this(rtnType, efts, default(T))
        {

        }


        /// <summary>
        /// 实例化一个新的 <see cref="MethodReturnInfo&lt;T&gt;"/> 完整实例。
        /// </summary>
        /// <param name="rtnType">方法操作返回类型</param>
        /// <param name="efts">影响的整数信息</param>
        /// <param name="smpInfo">附加信息.</param>
        public MethodReturnInfo(MethodResult rtnType, int? efts, T smpInfo)
        {
            this.Effects = efts;
            this.ReturnType = rtnType;
            this.SimpleInfo = smpInfo;
        }
    }



    #endregion
}


