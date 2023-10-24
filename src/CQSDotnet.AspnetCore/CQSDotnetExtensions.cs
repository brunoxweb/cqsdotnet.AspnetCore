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
                   .ScanAssemblies(assembly, typeof(IQuery), typeof(IQueryHandler<,>))
                   .ScanAssemblies(assembly, typeof(IQuery), typeof(IQueryValidator<>))
                   .ScanAssemblies(assembly, typeof(ICommand), typeof(ICommandHandler<>))
                   .ScanAssemblies(assembly, typeof(ICommand), typeof(ICommandValidator<>));
            }

            services.AddSingleton<IQueryHandlerFactory, QueryHandlerFactory>();
            services.AddSingleton<IQueryValidatorFactory, QueryValidatorFactory>();

            services.AddSingleton<ICommandHandlerFactory, CommandHandlerFactory>();
            services.AddSingleton<ICommandValidatorFactory, CommandValidatorFactory>();

            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

            return services;
        }

        private static IServiceCollection ScanAssemblies(this IServiceCollection services, Assembly assembly, Type serviceType, Type handlerType)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(type => serviceType.IsAssignableFrom(type))
                .ToList();

            for (int i = 0; i < serviceTypes.Count; i++)
            {
                var typeToRegister = serviceTypes[i].Assembly.GetTypes()
                  .FirstOrDefault(vt => vt.GetInterfaces().Any(a => a.IsGenericType &&
                             a.GetGenericTypeDefinition() == handlerType &&
                             a.GetGenericArguments()[0] == serviceTypes[i]));

                if (typeToRegister != null)
                {
                    services.AddTransient(serviceTypes[i], typeToRegister);
                }
            }

            return services;
        }
    }
}