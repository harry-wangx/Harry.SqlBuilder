using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#if SQLITE
namespace Harry.SqlBuilder.Sqlite
#elif SQLSERVER
namespace Harry.SqlBuilder.SqlServer
#elif MYSQL
namespace Harry.SqlBuilder.MySql
#endif
{
    public class RawBuilder : IRawBuilder
    {
        private readonly List<string> lstSql = new List<string>();
        private readonly List<SqlBuilderParameter> parameters = new List<SqlBuilderParameter>();
        private readonly List<object> lstDynamicParams = new List<object>();
        private readonly int capacity;

        public ISqlBuilder SqlBuilder { get; private set; }
        internal RawBuilder(ISqlBuilder factory, int capacity)
        {
            this.capacity = capacity;

            this.SqlBuilder = factory;
        }

        public IRawBuilder Append(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));

            lstSql.Add(sql);
            return this;
        }

        public IRawBuilder Append(string sql, SqlBuilderParameter parameter)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            //if (parameter == null)
            //    throw new ArgumentException($"{nameof(parameter)}不能为空", nameof(parameter));

            lstSql.Add(sql);
            parameters.Add(parameter);

            return this;
        }

        public IRawBuilder Append(string sql, params SqlBuilderParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            if (parameters == null)
                throw new ArgumentException($"{nameof(parameters)}不能为空", nameof(parameters));

            lstSql.Add(sql);
            this.parameters.AddRange(parameters);

            return this;
        }

        public IRawBuilder Append(string sql, object dynamicParams)
        {
#if SQLITE
            throw new NotSupportedException("Sqlite 不支持 DynamicParams");
#else 
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            if (dynamicParams == null)
                throw new ArgumentException($"{nameof(dynamicParams)}不能为空", nameof(dynamicParams));

            lstSql.Add(sql);
            lstDynamicParams.Add(dynamicParams);
            return this;
#endif
        }

        public SqlBuilderCommand ToCommand(IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var sb = new StringBuilder(capacity);

            foreach (var o in lstSql)
            {
                sb.AppendLine(o);
            }

            return new SqlBuilderCommand(sb.ToString(), parameters, lstDynamicParams, transaction, commandTimeout, commandType);
        }
    }
}
