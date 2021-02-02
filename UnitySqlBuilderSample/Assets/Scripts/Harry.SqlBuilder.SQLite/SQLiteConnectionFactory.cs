using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Harry.SqlBuilder.SQLite
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;
        public SQLiteConnectionFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }
        public IDbConnection CreateDbConnection()
        {
            return new SqliteConnection(connectionString);

        }
    }
}
