using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder.MySql
{
    public class MySqlProvider : ISqlProvider
    {
        public ISqlBuilder Create(string sqlType)
        {
            if (String.Equals(sqlType, "MySql", StringComparison.OrdinalIgnoreCase))
                return new Harry.SqlBuilder.MySql.SqlBuilder();
            else
                return null;
        }

        public void Dispose()
        {

        }
    }
}
