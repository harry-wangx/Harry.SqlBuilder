using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder.Sqlite
{
    public class SqliteSqlGenerationHelper : SqlGenerationHelper
    {
        public override string DelimitIdentifier(string name, string schema)
            => base.DelimitIdentifier(name);

        public override void DelimitIdentifier(StringBuilder builder, string name, string schema)
            => base.DelimitIdentifier(builder, name);
    }
}
