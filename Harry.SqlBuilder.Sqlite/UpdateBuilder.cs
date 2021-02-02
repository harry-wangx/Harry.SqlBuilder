using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;

#if SQLITE
namespace Harry.SqlBuilder.Sqlite
#elif SQLSERVER
namespace Harry.SqlBuilder.SqlServer
#elif MYSQL
namespace Harry.SqlBuilder.MySql
#endif
{
    public sealed class UpdateBuilder : IUpdateBuilder
    {
        private readonly string table;
        private readonly string schema;
        private readonly List<string> statics = new List<string>();
        private readonly List<SqlBuilderParameter> columns = new List<SqlBuilderParameter>();
        private readonly List<string> wheres = new List<string>();
        private readonly List<SqlBuilderParameter> whereParameters = new List<SqlBuilderParameter>();
        private readonly List<object> whereDynamicParams = new List<object>();
        private readonly int capacity;

        public ISqlBuilderFactory Factory { get; private set; }

        internal UpdateBuilder(ISqlBuilderFactory factory, string table, string schema, int capacity)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentException($"{nameof(table)}不能为空", nameof(table));
            this.table = table;
            this.schema = schema;
            this.capacity = capacity;

            this.Factory = factory;
        }

        public IUpdateBuilder Column(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));

            statics.Add(sql);
            return this;
        }

        public IUpdateBuilder Column(SqlBuilderParameter parameter)
        {
            //if (parameter == null)
            //    throw new ArgumentException($"{nameof(parameter)}不能为空", nameof(parameter));
            columns.Add(parameter);
            return this;
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
            if (statics.Count + columns.Count < 1)
            {
                throw new ArgumentException("更新字段数量不能为0");
            }
            if (wheres.Count == 0)
            {
                throw new ArgumentException("未提供 where 更新限定条件");
            }


            var sb = new StringBuilder("UPDATE ", capacity);
            this.Factory.SqlGenerationHelper.DelimitIdentifier(sb, table, schema);
            sb.Append(" SET ");

            var i = 0;
            foreach (var o in statics)
            {
                sb.AppendFormat(i == 0 ? "{0}\n" : "    ,{0}\n", o);
                i++;
            }
            foreach (var o in columns)
            {
                if (i != 0)
                {
                    sb.Append(",");
                }
                this.Factory.SqlGenerationHelper.DelimitIdentifier(sb, o.Name);
                sb.Append("=");
                this.Factory.SqlGenerationHelper.GenerateParameterName(sb, o.Name);
                i++;
            }

            sb.Append(" WHERE ");

            i = 0;
            foreach (var o in wheres)
            {
                sb.AppendFormat(i == 0 ? "{0}\n" : "    AND {0}\n", o);
                i++;
            }

            columns.AddRange(whereParameters);

            return new SqlBuilderCommand(sb.ToString(), columns, whereDynamicParams, transaction, commandTimeout, commandType);
        }

    }
}
