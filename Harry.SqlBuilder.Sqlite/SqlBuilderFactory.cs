#if SQLITE
namespace Harry.SqlBuilder.Sqlite
#elif SQLSERVER
namespace Harry.SqlBuilder.SqlServer
#elif MYSQL
namespace Harry.SqlBuilder.MySql
#endif
{
    public class SqlBuilderFactory : ISqlBuilderFactory
    {

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

#if SQLITE
        public ISqlGenerationHelper SqlGenerationHelper => new SqliteSqlGenerationHelper();
#elif SQLSERVER
        public ISqlGenerationHelper SqlGenerationHelper => new SqlServerSqlGenerationHelper();
#elif MYSQL
        public ISqlGenerationHelper SqlGenerationHelper => new MySqlSqlGenerationHelper();
#endif


    }
}
