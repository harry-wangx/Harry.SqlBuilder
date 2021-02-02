using Harry.SqlBuilder;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Harry.SqlBuilder.SQLite
{
    public static class SqlBuilderCommandExtensions
    {
        public static T ExecuteReader<T>(this SqlBuilderCommand sqlCmd, IDbConnectionFactory connFactory, Func<IDataReader, T> act)
        {
            using (var conn = connFactory.CreateDbConnection() as SqliteConnection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    initDbCommand(cmd, sqlCmd);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (act != null)
                            return act(reader);
                        else
                            return default(T);
                    }
                }
            }
        }

        public static T ExecuteReader<T>(this SqlBuilderCommand sqlCmd, SqliteConnection conn, Func<IDataReader, T> act)
        {
            using (var cmd = conn.CreateCommand())
            {
                initDbCommand(cmd, sqlCmd);

                using (var reader = cmd.ExecuteReader())
                {
                    if (act != null)
                        return act(reader);
                    else
                        return default(T);
                }
            }
        }

        public static object ExecuteScalar(this SqlBuilderCommand sqlCmd, IDbConnectionFactory connFactory)
        {
            using (var conn = connFactory.CreateDbConnection() as SqliteConnection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    initDbCommand(cmd, sqlCmd);

                    return cmd.ExecuteScalar();
                }
            }
        }

        public static object ExecuteScalar(this SqlBuilderCommand sqlCmd, SqliteConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                initDbCommand(cmd, sqlCmd);

                return cmd.ExecuteScalar();
            }
        }

        public static int ExecuteNonQuery(this SqlBuilderCommand sqlCmd, IDbConnectionFactory connFactory)
        {
            using (var conn = connFactory.CreateDbConnection() as SqliteConnection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    initDbCommand(cmd, sqlCmd);

                    return cmd.ExecuteNonQuery();
                }
            }
        }


        public static int ExecuteNonQuery(this SqlBuilderCommand sqlCmd, SqliteConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                initDbCommand(cmd, sqlCmd);

                return cmd.ExecuteNonQuery();
            }
        }


        private static void initDbCommand(SqliteCommand cmd, SqlBuilderCommand sqlCmd)
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlCmd.Sql;
            if (sqlCmd.Transaction != null) cmd.Transaction = sqlCmd.Transaction as SqliteTransaction;
            if (sqlCmd.Parameters != null)
            {
                foreach (var item in sqlCmd.Parameters)
                {
                    SqliteParameter p = new SqliteParameter(item.Name);
                    if (item.DbType != null) p.DbType = item.DbType.Value;
                    if (item.Size != null) p.Size = item.Size.Value;
                    if (item.Value != null) p.Value = item.Value;
                    if (item.Direction != null) p.Direction = item.Direction.Value;
                    cmd.Parameters.Add(p);
                }
            }
        }
    }
}
