using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder.Sqlite
{
    public class SqliteProvider : ISqlProvider
    {
        public ISqlBuilder Create(string sqlType)
        {
            if (String.Equals(sqlType, "Sqlite", StringComparison.OrdinalIgnoreCase))
                return new Harry.SqlBuilder.Sqlite.SqlBuilder();
            else
                return null;
        }

        public void Dispose()
        {

        }
    }
}
