using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder.Sqlite
{
    public class SqliteExtSql : IExtSql
    {
        public string GetAllTableNames()
        {
            return "select `name` from sqlite_master where type='table' and `name` != 'sqlite_sequence';";
        }
    }
}
