namespace Harry.SqlBuilder
{
    public interface IUpdateBuilder : IWhere, IToCommand, IHasSqlBuilder
    {
        IUpdateBuilder Column(string sql);

        IUpdateBuilder Column(SqlBuilderParameter parameter);
    }
}
