using Harry.SqlBuilder.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
namespace Harry.SqlBuilder
{
    public static class MySqlBuilderExtensions
    {
        public static IBuilder AddMySql(this IBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISqlProvider, MySqlProvider>());
            return builder;
        }
    }
}
