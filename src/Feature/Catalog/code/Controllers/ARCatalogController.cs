namespace ARCommerce.Feature.Catalog.Controllers
{
	using Microsoft.Extensions.DependencyInjection;
	using System.Web.Mvc;
	using ARCommerce.Feature.Catalog.Repositories;
	using Sitecore.Commerce.XA.Feature.Catalog.Repositories;
	using Sitecore.Commerce.XA.Foundation.Common.Context;
	using Sitecore.Commerce.XA.Foundation.Common.Models;
	using Sitecore.Commerce.XA.Foundation.Connect;
	using Sitecore.DependencyInjection;
	using Sitecore.Commerce.XA.Feature.Catalog.Controllers;

	public class ARCatalogController : CatalogController
	{
		private readonly IARCatalogRepository _arRepository;

		public ARCatalogController(IModelProvider modelProvider, 
			IProductListHeaderRepository productListHeaderRepository, 
			IProductListRepository productListRepository, 
			IPromotedProductsRepository promotedProductsRepository, 
			IProductInformationRepository productInformationRepository, 
			IProductImagesRepository productImagesRepository, 
			IProductInventoryRepository productInventoryRepository, 
			IProductPriceRepository productPriceRepository, 
			IProductVariantsRepository productVariantsRepository, 
			IProductListPagerRepository productListPagerRepository, 
			IProductFacetsRepository productFacetsRepository, 
			IProductListSortingRepository productListSortingRepository, 
			IProductListPageInfoRepository productListPageInfoRepository, 
			IProductListItemsPerPageRepository productListItemsPerPageRepository, 
			ICatalogItemContainerRepository catalogItemContainerRepository, 
			IVisitedCategoryPageRepository visitedCategoryPageRepository, 
			IVisitedProductDetailsPageRepository visitedProductDetailsPageRepository, 
			ISearchInitiatedRepository searchInitiatedRepository, 
			IStorefrontContext storefrontContext, 
			ISiteContext siteContext, 
			IContext sitecoreContext,
			IARCatalogRepository arRepository) : base(modelProvider, productListHeaderRepository, 
				productListRepository, promotedProductsRepository, productInformationRepository, 
				productImagesRepository, productInventoryRepository, productPriceRepository, 
				productVariantsRepository, productListPagerRepository, productFacetsRepository, 
				productListSortingRepository, productListPageInfoRepository, 
				productListItemsPerPageRepository, catalogItemContainerRepository, 
				visitedCategoryPageRepository, visitedProductDetailsPageRepository, 
				searchInitiatedRepository, storefrontContext, siteContext, sitecoreContext)
		{
			_arRepository = arRepository;
		}

		[HttpGet]
		public JsonResult GetProductScans([Bind(Prefix = "cci")] string currentCatalogItemId, [Bind(Prefix = "ci")] string currentItemId)
		{
			var service = ServiceLocator.ServiceProvider.GetService<IVisitorContext>();
			var products = _arRepository.GetProductScans(service, currentItemId, currentCatalogItemId);
			return Json(products, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult ProductInformation([Bind(Prefix = "id")] string productId)
		{
			var service = ServiceProviderServiceExtensions.GetService<IVisitorContext>(ServiceLocator.ServiceProvider);
			var product = _arRepository.GetProductInformation(service, productId);
			return Json(product, JsonRequestBehavior.AllowGet);
		}
	}
}