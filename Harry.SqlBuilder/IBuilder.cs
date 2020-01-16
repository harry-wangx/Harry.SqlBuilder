using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    public interface IBuilder
    {
        /// <summary>
        /// 获取服务 <see cref="IServiceCollection"/>
        /// </summary>
        IServiceCollection Services { get; }
    }
}
