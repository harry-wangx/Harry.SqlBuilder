using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    /// <summary>
    /// 通用SqlProvider
    /// </summary>
    public class SqlProvider : ISqlProvider
    {
        private ISqlBuilder _sqlBuilder;
        private readonly Func<ISqlBuilder> _func;

        public SqlProvider(string sqlType, ISqlBuilder sqlBuilder)
        {
            this.SqlType = sqlType;
            this._sqlBuilder = sqlBuilder;
        }

        public SqlProvider(string sqlType, Func<ISqlBuilder> func)
        {
            this.SqlType = sqlType;
            this._func = func;
        }

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public string SqlType { get; }

        /// <summary>
        /// 创建 <see cref="ISqlBuilder"/>
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public ISqlBuilder Create(string sqlType)
        {
            if (String.Equals(sqlType, this.SqlType, StringComparison.OrdinalIgnoreCase))
            {
                if (_sqlBuilder != null)
                    return _sqlBuilder;
                else
                    return _func?.Invoke();
            }
            else
                return null;

        }

        public void Dispose()
        {

        }
    }
}
