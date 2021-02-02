using Dapper;

namespace Harry.SqlBuilder.Dapper
{
    public static class DapperHelper
    {
        //生成参数
        public static SqlMapper.IDynamicParameters CreateDynamicParameters(SqlBuilderCommand cmd)
        {
            if (cmd.Parameters == null && cmd.DynamicParameters == null)
                return null;

            var result = new DynamicParameters();
            if (cmd.Parameters != null)
            {
                foreach (var item in cmd.Parameters)
                {
                    result.Add(item.Name, item.Value, item.DbType, item.Direction, item.Size);
                }
            }
            if (cmd.DynamicParameters != null)
            {
                foreach (var item in cmd.DynamicParameters)
                {
                    result.AddDynamicParams(item);
                }
            }

            return result;
        }
    }
}
