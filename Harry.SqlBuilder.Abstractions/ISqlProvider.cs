using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    public interface ISqlProvider : IDisposable
    {
        /// <summary>
        /// 创建一个新的 <see cref="ISqlBuilder"/>.
        /// </summary>
        /// <param name="sqlType">数据库类型</param>
        /// <returns>返回<see cref="ISqlBuilder"/>.</returns>
        ISqlBuilder Create(string sqlType);
    }
}
