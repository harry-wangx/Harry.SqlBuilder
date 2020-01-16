using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace Harry.SqlBuilder
{
    public class ExtSqls : IExtSql
    {
        private readonly ConcurrentDictionary<string, string> dicSqls;
        private readonly ExtSqls parent;

        private ExtSqls(ConcurrentDictionary<string, string> dicSqls)
        {
            this.dicSqls = dicSqls;
        }

        public ExtSqls()
        {
            dicSqls = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public ExtSqls(ExtSqls parent) : this()
        {
            this.parent = parent;
        }

        public string GetSql(string name)
        {
            dicSqls.TryGetValue(name, out string value);
            if (value != null) return value;

            if (parent != null)
            {
                value = parent.GetSql(name);
            }
            return value;
        }



        public class ExtSqlsBuilder
        {
            private readonly ConcurrentDictionary<string, string> dicSqls = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public ExtSqlsBuilder AddSql(string name, string sql)
            {
                Check.NotEmpty(name, nameof(name));
                Check.NotEmpty(sql, nameof(sql));

                dicSqls[name] = sql;

                return this;
            }

            public ExtSqls Build()
            {
                return new ExtSqls(dicSqls);
            }
        }
    }
}
