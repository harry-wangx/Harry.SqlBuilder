namespace Harry.SqlBuilder
{
    public interface IInsertBuilder : IToCommand, IHasSqlBuilderFactory
    {
        IInsertBuilder Column(SqlBuilderParameter parameter);
    }
}
