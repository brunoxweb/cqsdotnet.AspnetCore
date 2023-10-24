using Microsoft.Extensions.DependencyInjection;

namespace CQSDotnet.AspnetCore
{
    public class ServiceProviderTypeResolver : ITypeResolver
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderTypeResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object Resolve(Type type)
        {
            return serviceProvider.GetRequiredService(type);
        }
    }
}
