using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    public interface IHasSqlBuilder
    {
        ISqlBuilder SqlBuilder { get; }
    }
}
