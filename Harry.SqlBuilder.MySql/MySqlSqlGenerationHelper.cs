using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder.MySql
{
    public class MySqlSqlGenerationHelper : SqlGenerationHelper
    {
        public override string GenerateParameterName(string name)
            => "?" + name;

        public override void GenerateParameterName(StringBuilder builder, string name)
            => builder.Append("?").Append(name);

        public override string EscapeIdentifier(string identifier)
            => Check.NotEmpty(identifier, nameof(identifier)).Replace("`", "``");


        public override void EscapeIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));

            var initialLength = builder.Length;
            builder.Append(identifier);
            builder.Replace("`", "``", initialLength, identifier.Length);
        }


        public override string DelimitIdentifier(string identifier)
            => $"`{EscapeIdentifier(Check.NotEmpty(identifier, nameof(identifier)))}`";


        public override void DelimitIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));
            builder.Append('`');
            EscapeIdentifier(builder, identifier);
            builder.Append('`');
        }
    }
}
