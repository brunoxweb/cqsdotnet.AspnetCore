using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CQSDotnet.Commands;
using CQSDotnet.Commands.Interfaces;
using CQSDotnet.Queries;
using CQSDotnet.Queries.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CQSDotnet.AspnetCore
{
    [ExcludeFromCodeCoverage]
    public static class CQSDotnetExtensions
    {
        /// <summary>
        /// Enable the handlers and validator for queries and commands
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="assemblies">Assemblies in which handlers and validators reside.</param>
        /// <returns></returns>
        public static IServiceCollection CQSDotnetRegister(this IServiceCollection services, Assembly[] assemblies)
        {
            services.AddSingleton<ITypeResolver, ServiceProviderTypeResolver>();

            foreach (var assembly in assemblies)
            {
                services
                   .ScanAssemblies(assembly, typeof(IQueryHandler<,>))
                   .ScanAssemblies(assembly, typeof(IQueryValidator<>))
                   .ScanAssemblies(assembly, typeof(ICommandHandler<>))
                   .ScanAssemblies(assembly, typeof(ICommandValidator<>));
            }

            services.AddSingleton<IQueryHandlerFactory, QueryHandlerFactory>();
            services.AddSingleton<IQueryValidatorFactory, QueryValidatorFactory>();

            services.AddSingleton<ICommandHandlerFactory, CommandHandlerFactory>();
            services.AddSingleton<ICommandValidatorFactory, CommandValidatorFactory>();

            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

            return services;
        }

        private static IServiceCollection ScanAssemblies(this IServiceCollection services, Assembly assembly, Type typeInterface)
        {
            var implementationTypes = assembly.GetTypes()
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeInterface))
                .ToList();

            foreach (var implementationType in implementationTypes)
            {
                var interfaceType = implementationType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeInterface);

                services.AddTransient(interfaceType, implementationType);
            }

            return services;
        }
    }
}