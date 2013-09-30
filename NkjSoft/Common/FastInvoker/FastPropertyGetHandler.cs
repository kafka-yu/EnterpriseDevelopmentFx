namespace NkjSoft.Common
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 快速反射调用获取属性信息
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <returns></returns>
    public delegate object FastPropertyGetHandler(object target);
}

