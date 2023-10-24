using Microsoft.Extensions.DependencyInjection;

namespace CQSDotnet.AspnetCore.Tests
{
    [TestFixture]
    public class ServiceProviderTypeResolverTests
    {
        private IServiceProvider serviceProvider;

        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ISomeService, SomeService>();
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public void Resolve_ValidType_ReturnsInstance()
        {
            // Arrange
            var typeResolver = new ServiceProviderTypeResolver(serviceProvider);

            // Act
            var resolvedInstance = typeResolver.Resolve(typeof(ISomeService));

            // Assert
            Assert.That(resolvedInstance, Is.Not.Null);
            Assert.That(resolvedInstance, Is.InstanceOf<ISomeService>());
        }

        [Test]
        public void Resolve_InvalidType_ThrowsException()
        {
            // Arrange
            var typeResolver = new ServiceProviderTypeResolver(serviceProvider);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                typeResolver.Resolve(typeof(INonExistentService));
            });
        }

        public interface ISomeService { }

        public class SomeService : ISomeService { }

        public interface INonExistentService { }
    }
}
