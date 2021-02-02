using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    public interface IHasSqlBuilderFactory
    {
        ISqlBuilderFactory Factory { get; }
    }
}
