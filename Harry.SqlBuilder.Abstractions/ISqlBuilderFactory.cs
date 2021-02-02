
namespace Harry.SqlBuilder
{
    public interface ISqlBuilderFactory
    {
        IInsertBuilder Insert(string table, string schema = null, int capacity = 32);

        IUpdateBuilder Update(string table, string schema = null, int capacity = 32);

        IDeleteBuilder Delete(string table, string schema = null, int capacity = 32);

        ISelectBuilder Select(string field, int capacity = 32);

        IRawBuilder Raw(int capacity = 32);

        ISqlGenerationHelper SqlGenerationHelper { get; }
    }
}
