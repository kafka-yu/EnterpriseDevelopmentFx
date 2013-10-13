using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Framework.IoC
{
    public interface IComponentContext
    {
        TResult Resolve<TResult>();
    }
}
