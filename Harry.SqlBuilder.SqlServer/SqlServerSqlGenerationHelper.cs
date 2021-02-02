using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder.SqlServer
{
    public class SqlServerSqlGenerationHelper : SqlGenerationHelper
    {
        public override string BatchTerminator => "GO" + Environment.NewLine + Environment.NewLine;


        public override string EscapeIdentifier(string identifier)
            => Check.NotEmpty(identifier, nameof(identifier)).Replace("]", "]]");


        public override void EscapeIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));

            var initialLength = builder.Length;
            builder.Append(identifier);
            builder.Replace("]", "]]", initialLength, identifier.Length);
        }


        public override string DelimitIdentifier(string identifier)
            => $"[{EscapeIdentifier(Check.NotEmpty(identifier, nameof(identifier)))}]"; // Interpolation okay; strings


        public override void DelimitIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));

            builder.Append('[');
            EscapeIdentifier(builder, identifier);
            builder.Append(']');
        }

        public override string GeneratePaginationSql()
            => $" OFFSET {GenerateParameterName("offset")} rows fetch next {GenerateParameterName("limit")} rows only ";

        public override void GeneratePaginationSql(StringBuilder builder)
        {
            builder.Append(" OFFSET ");
            GenerateParameterName(builder, "offset");
            builder.Append(" rows fetch next ");
            GenerateParameterName(builder, "limit");
            builder.Append(" rows only ");
        }
    }
}
