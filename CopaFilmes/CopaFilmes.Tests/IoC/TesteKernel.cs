using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using CopaFilmes.IoC;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RepositorioAPI;

namespace CopaFilmes.Tests.IoC
{
    [TestFixture]
    public class TesteKernel
    {
        public IServiceCollection services;
        [OneTimeSetUp]
        public void SetUp()
        {
            services = new ServiceCollection();
        }
        [Test]
        public void DeveRetornarSourceAddressCorretamente()
        {
            services.AddSingleton<SourceAddress>("algumacoisa");
            var serviceProvider = services.BuildServiceProvider();
            var source = serviceProvider.GetService<SourceAddress>();
            Assert.AreEqual((string)source, "algumacoisa");
            Assert.AreEqual(source.GetSourceString(), "algumacoisa");
        }

        [Test]
        public void DeveRetornarRepositorioImplementado()
        {
            services.AddRepositorioApi("algumacoisa");
            var serviceProvider = services.BuildServiceProvider();
            var repositorio = serviceProvider.GetService<IRepository<Filme>>();
            Assert.IsNotNull(repositorio);
            Assert.AreEqual(typeof(RepositorioAPI<Filme>), repositorio.GetType());
        }
    }
}
