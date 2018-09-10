using System.Collections.Generic;
using System.Linq;
using ARCommerce.Feature.Catalog.Models;
using Sitecore.Commerce.XA.Feature.Catalog.Models;
using Sitecore.Commerce.XA.Feature.Catalog.Models.JsonResults;
using Sitecore.Commerce.XA.Feature.Catalog.Repositories;
using Sitecore.Commerce.XA.Foundation.Catalog.Managers;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.Models;
using Sitecore.Commerce.XA.Foundation.Common.Search;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Commerce.XA.Foundation.Connect.Entities;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using Sitecore.Diagnostics;

namespace ARCommerce.Feature.Catalog.Repositories
{
	public class ARCatalogRepository : ProductListRepository, IARCatalogRepository
	{
		public ARCatalogRepository(IModelProvider modelProvider, 
			IStorefrontContext storefrontContext, 
			ISiteContext siteContext, 
			ISearchInformation searchInformation, 
			ISearchManager searchManager, 
			ICatalogManager catalogManager, 
			IInventoryManager inventoryManager, 
			ICatalogUrlManager catalogUrlManager, 
			IContext context)
			: base(modelProvider, storefrontContext, siteContext, searchInformation, 
				  searchManager, catalogManager, inventoryManager, catalogUrlManager, context)
		{
		}

		public List<ProductScan> GetProductScans(IVisitorContext visitorContext, string currentItemId, string currentCatalogItemId)
		{
			Assert.ArgumentNotNull(visitorContext, "visitorContext");
			var model = ModelProvider.GetModel<ProductListJsonResult>();
			var curentCatalogItem = !string.IsNullOrEmpty(currentCatalogItemId) ? Context.Database.GetItem(currentCatalogItemId) : null;
			if (string.IsNullOrEmpty(currentCatalogItemId))
				curentCatalogItem = StorefrontContext.CurrentStorefront.CatalogItem;
			if (curentCatalogItem == null)
			{
				return null;
			}

			var currentItem = Context.Database.GetItem(currentItemId);
			SiteContext.CurrentCatalogItem = curentCatalogItem;
			SiteContext.CurrentItem = currentItem;

			CategorySearchInformation searchInformation = SearchInformation.GetCategorySearchInformation(curentCatalogItem);
			CommerceSearchOptions commerceSearchOptions = new CommerceSearchOptions(GetDefaultItemsPerPage(null, searchInformation),0);
			
			SearchResults childProducts = GetChildProducts(commerceSearchOptions, curentCatalogItem);
			List<ProductEntity> productEntityList = AdjustProductPriceAndStockStatus(visitorContext, childProducts, curentCatalogItem);

			productEntityList.Select(p => p.ProductId);

			return productEntityList.Select(p => new ProductScan { ProductId = p.ProductId, ScanFilePath = "TODO" }).ToList();
		}

		public CatalogItemRenderingModel GetProductInformation(IVisitorContext visitorContext, string productId)
		{
			var currentStorefront = StorefrontContext.CurrentStorefront;
			var productItem = SearchManager.GetProduct(productId, currentStorefront.Catalog);
			if (productItem == null)
			{
				return null;
			}
			return GetCatalogItemRenderingModel(visitorContext, productItem);
		}
	}
}