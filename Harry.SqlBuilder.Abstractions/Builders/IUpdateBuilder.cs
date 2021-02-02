namespace Harry.SqlBuilder
{
    public interface IUpdateBuilder : IWhere, IToCommand, IHasSqlBuilderFactory
    {
        IUpdateBuilder Column(string sql);

        IUpdateBuilder Column(SqlBuilderParameter parameter);
    }
}
