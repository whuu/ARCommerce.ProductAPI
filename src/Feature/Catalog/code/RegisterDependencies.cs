using ARCommerce.Feature.Catalog.Controllers;
using ARCommerce.Feature.Catalog.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace ARCommerce.Feature.Catalog
{
	public class RegisterDependencies : IServicesConfigurator
	{

		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IARCatalogRepository, ARCatalogRepository>();
			serviceCollection.AddTransient<ARCatalogController>();
		}

	}
}