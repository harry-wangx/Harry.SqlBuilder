using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    public class SqlGenerationHelper : ISqlGenerationHelper
    {
        /// <summary>
        /// SQL语句分隔符
        /// </summary>
        public virtual string StatementTerminator => ";";

        /// <summary>
        /// 批量SQL语句的分隔符
        /// </summary>
        public virtual string BatchTerminator => string.Empty;

        /// <summary>
        /// 为给定的名称生成有效的参数名
        /// </summary>
        public virtual string GenerateParameterName(string name)
            => "@" + name;

        /// <summary>
        /// 将有效的参数名写入StringBuilder
        /// </summary>
        public virtual void GenerateParameterName(StringBuilder builder, string name)
            => builder.Append("@").Append(name);


        /// <summary>
        /// 生成标识符（列名、表名等）的转义SQL表示
        /// </summary>
        public virtual string EscapeIdentifier(string identifier)
            => Check.NotEmpty(identifier, nameof(identifier)).Replace("\"", "\"\"");

        /// <summary>
        /// 将有效的标识符（列名、表名等）的转义SQL写入StringBuilder
        /// </summary>
        public virtual void EscapeIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));

            var initialLength = builder.Length;
            builder.Append(identifier);
            builder.Replace("\"", "\"\"", initialLength, identifier.Length);
        }

        /// <summary>
        /// 生成标准标识符
        /// </summary>
        public virtual string DelimitIdentifier(string identifier)
            => $"\"{EscapeIdentifier(Check.NotEmpty(identifier, nameof(identifier)))}\""; 

        /// <summary>
        /// 将生成的标准标识符写入StringBuilder
        /// </summary>
        public virtual void DelimitIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));

            builder.Append('"');
            EscapeIdentifier(builder, identifier);
            builder.Append('"');
        }

        /// <summary>
        /// 生成带分隔符的标识符
        /// </summary>
        public virtual string DelimitIdentifier(string name, string schema)
            => (!string.IsNullOrEmpty(schema)
                   ? DelimitIdentifier(schema) + "."
                   : string.Empty)
               + DelimitIdentifier(Check.NotEmpty(name, nameof(name)));

        /// <summary>
        /// 将生成的带分隔符的标识符写入StringBuilder
        /// </summary>
        public virtual void DelimitIdentifier(StringBuilder builder, string name, string schema)
        {
            if (!string.IsNullOrEmpty(schema))
            {
                DelimitIdentifier(builder, schema);
                builder.Append(".");
            }

            DelimitIdentifier(builder, name);
        }

        /// <summary>
        /// 生成分页脚本
        /// </summary>
        /// <returns></returns>
        public virtual string GeneratePaginationSql()
            => $" LIMIT {GenerateParameterName("offset")}, {GenerateParameterName("limit")} ";

        public virtual void GeneratePaginationSql(StringBuilder builder)
        {
            builder.Append(" LIMIT ");
            GenerateParameterName(builder, "offset");
            builder.Append(",");
            GenerateParameterName(builder, "limit");
        }
    }
}
