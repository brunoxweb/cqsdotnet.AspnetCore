using System.Reflection;
using CQSDotnet.AspnetCore.Tests.Stubs;
using CQSDotnet.Commands;
using CQSDotnet.Commands.Interfaces;
using CQSDotnet.Queries;
using CQSDotnet.Queries.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CQSDotnet.AspnetCore.Tests
{
    [TestFixture]
    public class CQSDotnetExtensionsTests
    {
        [Test]
        public void CQSDotnetRegister_RegistersTypes_RegisteredSuccessfully()
        {
            // Arrange
            var services = new ServiceCollection();
            var assemblies = new Assembly[] { typeof(CQSDotnetExtensionsTests).Assembly };

            // Act
            services.CQSDotnetRegister(assemblies);

            // Assert
            AssertIsRegistered<ITypeResolver, ServiceProviderTypeResolver>(services);
            AssertIsRegistered<IQueryHandlerFactory, QueryHandlerFactory>(services);
            AssertIsRegistered<IQueryValidatorFactory, QueryValidatorFactory>(services);
            AssertIsRegistered<ICommandHandlerFactory, CommandHandlerFactory>(services);
            AssertIsRegistered<ICommandValidatorFactory, CommandValidatorFactory>(services);
            AssertIsRegistered<ICommandDispatcher, CommandDispatcher>(services);
            AssertIsRegistered<IQueryDispatcher, QueryDispatcher>(services);
        }

        [Test]
        public void CQSDotnetRegister_ScansAssemblies_RegisteredSuccessfully()
        {
            // Arrange
            var services = new ServiceCollection();
            var assemblies = new Assembly[] { typeof(CQSDotnetExtensionsTests).Assembly };

            // Act
            services.CQSDotnetRegister(assemblies);

            // Assert
            AssertIsRegistered<IQueryHandler<MyQuery, MyDto>, MyQueryHandler>(services);
        }

        private static void AssertIsRegistered<TInterface, TImplementation>(ServiceCollection services)
        {
            var descriptor = services
                .FirstOrDefault(d => d.ServiceType == typeof(TInterface) && d.ImplementationType == typeof(TImplementation));
        }
    }
}
