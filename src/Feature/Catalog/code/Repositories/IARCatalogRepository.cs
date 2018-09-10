using ARCommerce.Feature.Catalog.Models;
using Sitecore.Commerce.XA.Feature.Catalog.Models;
using Sitecore.Commerce.XA.Foundation.Connect;
using System.Collections.Generic;

namespace ARCommerce.Feature.Catalog.Repositories
{
	public interface IARCatalogRepository
	{
		List<ProductScan> GetProductScans(IVisitorContext visitorContext, string currentItemId, string currentCatalogItemId);

		CatalogItemRenderingModel GetProductInformation(IVisitorContext visitorContext, string productId);
	}
}