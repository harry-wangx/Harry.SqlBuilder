using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harry.SqlBuilder
{
    public class SqlFactory : ISqlFactory
    {
        private readonly Dictionary<string, ISqlBuilder> _builderFactories = new Dictionary<string, ISqlBuilder>(StringComparer.Ordinal);
        private List<ISqlProvider> _provider = new List<ISqlProvider>();
        private readonly object _sync = new object();
        private volatile bool _disposed;

        public SqlFactory() : this(Enumerable.Empty<ISqlProvider>())
        {

        }

        /// <summary>
        /// 创建一个新的 <see cref="SqlFactory"/>.
        /// </summary>
        /// <param name="providers">适配器集合,用于创建<see cref="ISqlBuilder"/>.</param>
        public SqlFactory(IEnumerable<ISqlProvider> providers)
        {
            _provider.AddRange(providers);
        }

        /// <summary>
        /// 创建一个新的 <see cref="ISqlBuilder"/>.
        /// </summary>
        /// <param name="sqlType">数据库类型</param>
        /// <returns>返回<see cref="ISqlBuilder"/>.</returns>
        public ISqlBuilder Create(string sqlType)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(SqlFactory));
            }

            lock (_sync)
            {
                if (!_builderFactories.TryGetValue(sqlType, out var result))
                {
                    for (int i = 0; i < _provider.Count; i++)
                    {
                        result = _provider[i].Create(sqlType);
                        if (result != null) break;
                    }

                    if (result != null)
                        _builderFactories[sqlType] = result;
                }

                return result;
            }
        }

        /// <summary>
        /// 添加 <see cref="ISqlProvider"/>.
        /// </summary>
        /// <param name="provider"> <see cref="ISqlProvider"/>.</param>
        public void AddProvider(ISqlProvider provider)
        {
            lock (_sync)
            {
                _provider.Add(provider);
            }
        }

        /// <summary>
        /// 获取资源是否被适放
        /// </summary>
        /// <returns>当调用 <see cref="Dispose()"/>后,返回True</returns>
        protected virtual bool CheckDisposed() => _disposed;

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                foreach (var provider in _provider)
                {
                    try
                    {
                        provider.Dispose();
                    }
                    catch
                    {
                        // Swallow exceptions on dispose
                    }
                }
            }
        }
    }
}
