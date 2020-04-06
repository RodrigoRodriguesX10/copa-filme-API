using CopaFilmes.Dominio.Repositorio;
using Microsoft.Extensions.DependencyInjection;
using RepositorioAPI;
using System;

namespace CopaFilmes.IoC
{
    public static class Kernel
    {
        public static IServiceCollection AddRepositorioApi(this IServiceCollection services, string sourceAddress)
        {
            services.AddSingleton<SourceAddress>(sourceAddress);
            services.AddTransient(typeof(IRepository<>), typeof(RepositorioAPI<>));
            return services;
        }
    }
}
