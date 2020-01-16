using Harry.SqlBuilder;
using Harry.SqlBuilder.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper
{
    public static class DapperExtensions
    {
        #region Execute

        public static int Execute(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.Execute(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }


        public static Task<int> ExecuteAsync(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.ExecuteAsync(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }

        #endregion

        #region ExecuteReader
        public static IDataReader ExecuteReader(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.ExecuteReader(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.ExecuteReaderAsync(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.ExecuteScalar(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }

        public static T ExecuteScalar<T>(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.ExecuteScalar<T>(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }

        public static Task<object> ExecuteScalarAsync(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.ExecuteScalarAsync(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }

        public static Task<T> ExecuteScalarAsync<T>(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            if (cnn == null)
                throw new ArgumentNullException(nameof(cnn));

            return cnn.ExecuteScalarAsync<T>(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }
        #endregion

        #region Query
#if NET35
        public static IEnumerable<IDictionary<string, object>> Query(this IDbConnection cnn, SqlBuilderCommand cmd, bool buffered = true)
        {
            return cnn.Query(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, buffered, cmd.CommandTimeout, cmd.CommandType);
        }
#else
        public static IEnumerable<dynamic> Query(this IDbConnection cnn, SqlBuilderCommand cmd, bool buffered = true)
        {
            return cnn.Query(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, buffered, cmd.CommandTimeout, cmd.CommandType);
        }
#endif
        public static IEnumerable<T> Query<T>(this IDbConnection cnn, SqlBuilderCommand cmd, bool buffered = true)
        {
            return cnn.Query<T>(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, buffered, cmd.CommandTimeout, cmd.CommandType);
        }

        public static Task<IEnumerable<dynamic>> QueryAsync(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            return cnn.QueryAsync(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }

        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, SqlBuilderCommand cmd)
        {
            return cnn.QueryAsync<T>(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType);
        }
        #endregion

        //todo:查询
        //public static IEnumerable<T> PagedList<T>(this IDbConnection cnn, SqlBuilderCommand cmd, out int count)
        //{
        //    using (var reader = cnn.QueryMultiple(cmd.Sql, DapperHelper.CreateDynamicParameters(cmd), cmd.Transaction, cmd.CommandTimeout, cmd.CommandType))
        //    {
        //        count = reader.Read<int>().FirstOrDefault();
        //        if (count > 0)
        //        {
        //            return reader.Read<T>();
        //        }
        //        return null;
        //    }
        //}

        ////生成参数
        //public static SqlMapper.IDynamicParameters CreateDynamicParameters(ref SqlBuilderCommand cmd)
        //{
        //    if (cmd.Parameters == null && cmd.DynamicParameters == null)
        //        return null;

        //    var result = new DynamicParameters();
        //    if (cmd.Parameters != null)
        //    {
        //        foreach (var item in cmd.Parameters)
        //        {
        //            result.Add(item.Name, item.Value, item.DbType, item.Direction, item.Size);
        //        }
        //    }
        //    if (cmd.DynamicParameters != null)
        //    {
        //        foreach (var item in cmd.DynamicParameters)
        //        {
        //            result.AddDynamicParams(item);
        //        }
        //    }

        //    return result;
        //}
    }
}
