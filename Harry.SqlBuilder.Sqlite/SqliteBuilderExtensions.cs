using Harry.SqlBuilder.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
namespace Harry.SqlBuilder
{
    public static class SqliteBuilderExtensions
    {
        public static IBuilder AddSqlite(this IBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISqlProvider, SqliteProvider>());
            return builder;
        }
    }
}
