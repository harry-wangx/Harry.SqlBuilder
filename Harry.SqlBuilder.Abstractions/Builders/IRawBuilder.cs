namespace Harry.SqlBuilder
{
    public interface IRawBuilder : IToCommand, IHasSqlBuilderFactory
    {
        IRawBuilder Append(string sql);

        IRawBuilder Append(string sql, SqlBuilderParameter parameter);

        IRawBuilder Append(string sql, params SqlBuilderParameter[] parameters);

        IRawBuilder Append(string sql, object dynamicParams);
    }
}
