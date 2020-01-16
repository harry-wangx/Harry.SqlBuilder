namespace Harry.SqlBuilder
{
    public interface IInsertBuilder : IToCommand, IHasSqlBuilder
    {
        IInsertBuilder Column(SqlBuilderParameter parameter);
    }
}
