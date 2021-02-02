using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#if SQLITE
namespace Harry.SqlBuilder.Sqlite
#elif SQLSERVER
namespace Harry.SqlBuilder.SqlServer
#elif MYSQL
namespace Harry.SqlBuilder.MySql
#endif
{
    public sealed class DeleteBuilder : IDeleteBuilder
    {
        private readonly string table;
        private readonly string schema;
        private readonly List<string> wheres = new List<string>();
        private readonly List<SqlBuilderParameter> whereParameters = new List<SqlBuilderParameter>();
        private readonly List<object> whereDynamicParams = new List<object>();
        private readonly int capacity;

        public ISqlBuilderFactory Factory { get; private set; }

        internal DeleteBuilder(ISqlBuilderFactory factory, string table, string schema, int capacity)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentException($"{nameof(table)}不能为空", nameof(table));

            this.Factory = factory;
            this.table = table;
            this.schema = schema;

            this.capacity = capacity;
        }

        public IWhere Where(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            wheres.Add(sql);
            return this;
        }

        public IWhere Where(string sql, SqlBuilderParameter parameter)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            //if (parameter == null)
            //    throw new ArgumentException($"{nameof(parameter)}不能为空", nameof(parameter));
            wheres.Add(sql);
            whereParameters.Add(parameter);
            return this;
        }

        public IWhere Where(string sql, params SqlBuilderParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            if (parameters == null)
                throw new ArgumentException($"{nameof(parameters)}不能为空", nameof(parameters));
            wheres.Add(sql);
            whereParameters.AddRange(parameters);
            return this;
        }

        public IWhere Where(string sql, object dynamicParams)
        {
#if SQLITE
            throw new NotSupportedException("Sqlite 不支持 DynamicParams");
#else
            //todo:mysql是否支持没有经过测试
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            if (dynamicParams == null)
                throw new ArgumentException($"{nameof(dynamicParams)}不能为空", nameof(dynamicParams));

            wheres.Add(sql);
            whereDynamicParams.Add(dynamicParams);
            return this;
#endif
        }

        public SqlBuilderCommand ToCommand(IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (wheres.Count == 0)
            {
                throw new ArgumentException("未提供 where 删除限定条件");
            }
            var sb = new StringBuilder("DELETE FROM ", capacity);
            this.Factory.SqlGenerationHelper.DelimitIdentifier(sb,table, schema);
            sb.Append(" WHERE ");

            var i = 0;
            foreach (var o in wheres)
            {
                sb.AppendFormat(i == 0 ? "{0}\n" : " AND {0}\n", o);
                i++;
            }
            return new SqlBuilderCommand(sb.ToString(), whereParameters, whereDynamicParams, transaction, commandTimeout, commandType);
        }
    }
}
