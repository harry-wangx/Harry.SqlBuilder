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
    public class SelectBuilder : ISelectBuilder
    {
        private readonly IDictionary<string, Clauses> data = new Dictionary<string, Clauses>();
        private readonly List<object> whereDynamicParams = new List<object>();
        private OrderType orderType = OrderType.Asc;
        private readonly int capacity;

        private class Clause
        {
            public string Sql { get; set; }

            public IEnumerable<SqlBuilderParameter> Parameters { get; set; }
        }

        private enum OrderType { Asc, Desc }

        private class Clauses : List<Clause>
        {
            private readonly string joiner;
            private readonly string prefix;
            private readonly string postfix;

            public Clauses(string joiner, string prefix = "", string postfix = "")
            {
                this.joiner = joiner;
                this.prefix = prefix;
                this.postfix = postfix;
            }

            public override string ToString()
            {
#if NET35
                return prefix + string.Join(joiner, this.Select(c => c.Sql).ToArray()) + postfix;
#else
                return prefix + string.Join(joiner, this.Select(c => c.Sql)) + postfix;
#endif
            }
        }

        private void AddClause(string name, string sql, IEnumerable<SqlBuilderParameter> parameters, string joiner, string prefix = "", string postfix = "")
        {
            Clauses clauses;
            if (!data.TryGetValue(name, out clauses))
            {
                clauses = new Clauses(joiner, prefix, postfix);
                data[name] = clauses;
            }
            clauses.Add(new Clause { Sql = sql, Parameters = parameters });
        }

        public ISqlBuilder SqlBuilder { get; private set; }

        internal SelectBuilder(ISqlBuilder factory, string field, int capacity)
        {
            this.capacity = capacity;

            this.SqlBuilder = factory;

            AddClause("SELECT", field, null, " , ", prefix: "", postfix: "\n");
        }


        public ISelectBuilder From(string table, string schema = null)
        {
            AddClause("FROM", this.SqlBuilder.SqlGenerationHelper.DelimitIdentifier(table, schema), null, " , ", "", "\n");
            return this;
        }

        public IWhere Where(string sql)
        {
            AddClause("WHERE", sql, null, " AND ", prefix: "WHERE ", postfix: "\n");
            return this;
        }

        public IWhere Where(string sql, SqlBuilderParameter parameter)
        {
            //if (parameter == null)
            //    throw new ArgumentException($"{nameof(parameter)}不能为空", nameof(parameter));

            return Where(sql, new SqlBuilderParameter[] { parameter });
        }

        public IWhere Where(string sql, params SqlBuilderParameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentException($"{nameof(parameters)}不能为空", nameof(parameters));

            AddClause("WHERE", sql, parameters, " AND ", prefix: "WHERE ", postfix: "\n");
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

            AddClause("WHERE", sql, null, " AND ", prefix: "WHERE ", postfix: "\n");
            whereDynamicParams.Add(dynamicParams);
            return this;
#endif
        }

        public ISelectBuilder OrderBy(string sql)
        {
            AddClause("ORDERBY", sql, null, " , ", prefix: "ORDER BY ", postfix: " ");
            return this;
        }

        public ISelectBuilder Asc()
        {
            orderType = OrderType.Asc;
            return this;
        }

        public ISelectBuilder Desc()
        {
            orderType = OrderType.Desc;
            return this;
        }

        public ISelectBuilder GroupBy(string sql)
        {
            AddClause("GROUPBY", sql, null, joiner: " , ", prefix: "GROUP BY ", postfix: "\n");
            return this;
        }

        public ISelectBuilder Having(string sql, params SqlBuilderParameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentException($"{nameof(parameters)}不能为空", nameof(parameters));

            AddClause("HAVING", sql, parameters, joiner: "\nAND ", prefix: "HAVING ", postfix: "\n");
            return this;
        }

        public SqlBuilderCommand ToCommand(IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return ToCommand(-1, -1, transaction, commandTimeout, commandType);
        }

        public SqlBuilderCommand ToCommand(int take, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (data.ContainsKey("HAVING") && !data.ContainsKey("groupby"))
            {
                throw new ArgumentException("group by 参数未提供");
            }

            return ToCommand(1, take, transaction, commandTimeout, commandType);
        }

        public SqlBuilderCommand ToCommand(int page, int size, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (data.ContainsKey("HAVING") && !data.ContainsKey("groupby"))
            {
                throw new ArgumentException("group by 参数未提供");
            }

            if (size < 0) size = 0;

            var sb = new StringBuilder(capacity);
            var parameters = new List<SqlBuilderParameter>();

            //生成sql
            generateSql(sb, parameters);

            //access不允许同时执行多条SQL语句
            ////返回总数量
            //sb.AppendFormat("SELECT COUNT(0) FROM {0}", data["FROM"]);
            //if (data.ContainsKey("WHERE"))
            //{
            //    sb.Append(data["WHERE"]);
            //    parameters.AddRange(data["WHERE"].Where(p => p != null && p.Parameters != null).SelectMany(p => p.Parameters));
            //}
            //if (data.ContainsKey("GROUPBY"))
            //    sb.Append(data["GROUPBY"]);
            //if (data.ContainsKey("HAVING"))
            //    sb.Append(data["HAVING"]);
            //sb.Append(";\n");

            if (page > 0)
            {
                this.SqlBuilder.SqlGenerationHelper.GeneratePaginationSql(sb);
                parameters.Add(new SqlBuilderParameter("offset", (page - 1) * size));
                parameters.Add(new SqlBuilderParameter("limit", size));
            }

            return new SqlBuilderCommand(sb.ToString(), parameters, whereDynamicParams, transaction, commandTimeout, commandType);
        }

        private void generateSql(StringBuilder sb, List<SqlBuilderParameter> parameters)
        {
            sb.AppendFormat("SELECT {0}", data["SELECT"]);
            sb.AppendFormat(" FROM {0}", data["FROM"]);
            if (data.ContainsKey("WHERE"))
            {
                sb.Append(" ");
                sb.Append(data["WHERE"]);
                parameters.AddRange(data["WHERE"].Where(p => p != null && p.Parameters != null).SelectMany(p => p.Parameters));
            }
            if (data.ContainsKey("GROUPBY"))
            {
                sb.Append(" ");
                sb.Append(data["GROUPBY"]);
            }
            if (data.ContainsKey("HAVING"))
            {
                sb.Append(" ");
                sb.Append(data["HAVING"]);
                parameters.AddRange(data["WHERE"].Where(p => p != null && p.Parameters != null).SelectMany(p => p.Parameters));
            }
            if (data.ContainsKey("ORDERBY"))
            {
                sb.Append(" ");
                sb.Append(data["ORDERBY"]);
                sb.Append(orderType == OrderType.Asc ? " ASC " : " DESC ");
            }
        }
    }
}
