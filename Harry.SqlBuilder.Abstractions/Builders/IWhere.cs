using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    //public interface IWhere<T> where T:class, IWhere<T>
    //{
    //    T Where(string sql);

    //    T Where(string sql, SqlBuilderParameter parameter);

    //    T Where(string sql, params SqlBuilderParameter[] parameters);

    //    T Where(string sql, object dynamicParams);
    //}

    public interface IWhere:IToCommand
    {
        IWhere Where(string sql);

        IWhere Where(string sql, SqlBuilderParameter parameter);

        IWhere Where(string sql, params SqlBuilderParameter[] parameters);

        IWhere Where(string sql, object dynamicParams);
    }
}
