using System;
using Harry.SqlBuilder;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加SqlBuilder相关服务到 <see cref="IServiceCollection" />.
        /// </summary>
        public static IServiceCollection AddSqlBuilder(this IServiceCollection services)
        {
            return AddSqlBuilder(services, builder => { });
        }

        /// <summary>
        /// 添加SqlBuilder相关服务到 <see cref="IServiceCollection" />.
        /// </summary>
        public static IServiceCollection AddSqlBuilder(this IServiceCollection services, Action<IBuilder> builder)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton<ISqlFactory, SqlFactory>());

            if (builder != null)
            {
                builder.Invoke(new Builder(services));
            }

            return services;
        }
    }
}
