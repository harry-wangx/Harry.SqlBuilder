
namespace Harry.SqlBuilder
{
    public interface ISqlBuilder
    {
        IInsertBuilder Insert(string table, string schema = null, int capacity = 32);

        IUpdateBuilder Update(string table, string schema = null, int capacity = 32);

        IDeleteBuilder Delete(string table, string schema = null, int capacity = 32);

        ISelectBuilder Select(string field, int capacity = 32);

        IRawBuilder Raw(int capacity = 32);


        ISqlGenerationHelper SqlGenerationHelper { get; }

        /// <summary>
        /// 获取扩展接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetExt<T>() where T : class, IExtSql;

        /// <summary>
        /// 获取SQL脚本
        /// </summary>
        /// <param name="name">脚本名称</param>
        /// <returns></returns>
        string GetSql(string name);
    }
}
