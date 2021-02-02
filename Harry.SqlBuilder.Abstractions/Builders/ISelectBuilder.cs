using System.Data;

namespace Harry.SqlBuilder
{
    public interface ISelectBuilder : IWhere, IToCommand, IHasSqlBuilderFactory
    {
        ISelectBuilder From(string table, string schema = null);

        ISelectBuilder GroupBy(string sql);

        ISelectBuilder Having(string sql, params SqlBuilderParameter[] parameters);

        ISelectBuilder OrderBy(string sql);

        ISelectBuilder Asc();

        ISelectBuilder Desc();

        SqlBuilderCommand ToCommand(int page, int size, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        SqlBuilderCommand ToCommand(int take, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
