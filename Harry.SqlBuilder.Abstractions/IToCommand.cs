using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Harry.SqlBuilder
{
    public interface IToCommand
    {
        SqlBuilderCommand ToCommand(IDbTransaction transaction=null, int? commandTimeout=null, CommandType? commandType=null);
    }
}
