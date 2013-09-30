namespace NkjSoft.Common
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 快速反射调用。
    /// </summary>
    /// <param name="target">对象</param>
    /// <param name="parameter">参数</param>
    public delegate void FastPropertySetHandler(object target, object parameter);
}

