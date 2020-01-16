using System;
using System.Collections.Generic;
using System.Linq;

#if SQLITE
namespace Harry.SqlBuilder.Sqlite
#elif SQLSERVER
namespace Harry.SqlBuilder.SqlServer
#elif MYSQL
namespace Harry.SqlBuilder.MySql
#endif
{
    public class SqlBuilder : ISqlBuilder
    {
        private Dictionary<Type, IExtSql> dicExtSql = new Dictionary<Type, IExtSql>();
        private ExtSqls extSqls;
        public SqlBuilder() : this(Enumerable.Empty<IExtSql>())
        {

        }

        public SqlBuilder(IEnumerable<IExtSql> exts)
        {
            if (exts != null)
            {
                foreach (var item in exts)
                {
                    dicExtSql[item.GetType()] = item;
                }
            }

            if (dicExtSql.TryGetValue(typeof(ExtSqls), out IExtSql tmpExtSqls))
            {
                extSqls = tmpExtSqls as ExtSqls;
            }

            if (extSqls == null)
            {
                extSqls = new ExtSqls();
                dicExtSql[typeof(ExtSqls)] = extSqls;
            }
        }

        public IInsertBuilder Insert(string table, string schema = null, int capacity = 32)
        {
            if (capacity <= 0) capacity = 16;
            return new InsertBuilder(this, table, schema, capacity);
        }
        public IDeleteBuilder Delete(string table, string schema = null, int capacity = 32)
        {
            if (capacity <= 0) capacity = 16;
            return new DeleteBuilder(this, table, schema, capacity);
        }

        public IUpdateBuilder Update(string table, string schema = null, int capacity = 32)
        {
            if (capacity <= 0) capacity = 16;
            return new UpdateBuilder(this, table, schema, capacity);
        }

        public ISelectBuilder Select(string field, int capacity = 32)
        {
            if (capacity <= 0) capacity = 16;
            return new SelectBuilder(this, field, capacity);
        }

        public IRawBuilder Raw(int capacity = 32)
        {
            if (capacity <= 0) capacity = 16;
            return new RawBuilder(this, capacity);
        }

        public T GetExt<T>() where T : class, IExtSql
        {
            if (dicExtSql.TryGetValue(typeof(T), out IExtSql value))
            {
                return value as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取SQL脚本
        /// </summary>
        /// <param name="name">脚本名称</param>
        /// <returns></returns>
        public string GetSql(string name)
        {
            return extSqls.GetSql(name);
        }

#if SQLITE
        public ISqlGenerationHelper SqlGenerationHelper => new SqliteSqlGenerationHelper();
#elif SQLSERVER
        public ISqlGenerationHelper SqlGenerationHelper => new SqlServerSqlGenerationHelper();
#elif MYSQL
        public ISqlGenerationHelper SqlGenerationHelper => new MySqlSqlGenerationHelper();
#endif


    }
}
