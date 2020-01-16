using Harry.SqlBuilder.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
namespace Harry.SqlBuilder
{
    public static class SqlServerBuilderExtensions
    {
        public static IBuilder AddSqlServer(this IBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISqlProvider, SqlServerProvider>());
            return builder;
        }
    }
}
