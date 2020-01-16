using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder.SqlServer
{
    public class SqlServerProvider : ISqlProvider
    {
        public ISqlBuilder Create(string sqlType)
        {
            if (String.Equals(sqlType, "SqlServer", StringComparison.OrdinalIgnoreCase))
                return new Harry.SqlBuilder.SqlServer.SqlBuilder();
            else
                return null;
        }

        public void Dispose()
        {

        }
    }
}
