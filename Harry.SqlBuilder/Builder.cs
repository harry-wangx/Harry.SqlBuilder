using Microsoft.Extensions.DependencyInjection;

namespace Harry.SqlBuilder
{
    public class Builder : IBuilder
    {
        public Builder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
