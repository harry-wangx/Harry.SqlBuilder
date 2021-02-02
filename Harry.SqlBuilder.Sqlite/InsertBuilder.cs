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
    public sealed class InsertBuilder : IInsertBuilder
    {
        private readonly string table;
        private readonly string schema;
        private readonly IList<SqlBuilderParameter> columns = new List<SqlBuilderParameter>();

        private readonly int capacity;

        public ISqlBuilderFactory Factory { get; private set; }

        internal InsertBuilder(ISqlBuilderFactory factory, string table, string schema, int capacity)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentException($"{nameof(table)}不能为空", nameof(table));
            this.table = table;
            this.schema = schema;
            this.Factory = factory;

            this.capacity = capacity;
        }


        public IInsertBuilder Column(SqlBuilderParameter parameter)
        {
            //if (parameter == null)
            //    throw new ArgumentException($"{nameof(parameter)}不能为空", nameof(parameter));
            columns.Add(parameter);
            return this;
        }

        public SqlBuilderCommand ToCommand(IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (columns.Count <= 0)
            {
                throw new Exception("无需要插入的字段,请先调用Column()方法");
            }

            StringBuilder sbNames = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();

            for (int i = 0; i < columns.Count; i++)
            {
                if (i != 0)
                {
                    sbNames.Append(",");
                    sbParams.Append(",");
                }
                var item = columns[i];

                this.Factory.SqlGenerationHelper.DelimitIdentifier(sbNames, item.Name);

                this.Factory.SqlGenerationHelper.GenerateParameterName(sbParams, item.Name);
            }

            var sb = new StringBuilder("INSERT INTO ", capacity);
            this.Factory.SqlGenerationHelper.DelimitIdentifier(sb, table, schema);
            sb.AppendFormat(" ({0})VALUES({1})", sbNames.ToString(), sbParams.ToString());

            //if (identity)
            //{
            //    sb.AppendLine(";SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id");
            //}

            return new SqlBuilderCommand(sb.ToString(), columns, null, transaction, commandTimeout, commandType);
        }
    }
}
