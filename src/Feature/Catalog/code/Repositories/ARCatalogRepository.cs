using System;
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
using Sitecore.Data.Items;
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

		public ProductSummaryViewModel GetProductInformation(IVisitorContext visitorContext, string productId)
		{
			var currentStorefront = StorefrontContext.CurrentStorefront;
			var productItem = SearchManager.GetProduct(productId, currentStorefront.Catalog);
			if (productItem == null)
			{
				return null;
			}
			return CatalogItemRenderingModel(visitorContext, productItem);
		}

		protected virtual ProductSummaryViewModel CatalogItemRenderingModel(IVisitorContext visitorContext, Item productItem)
		{
			Assert.ArgumentNotNull(visitorContext, "visitorContext");

			var currentStorefront = StorefrontContext.CurrentStorefront;
			var variantEntityList = new List<VariantEntity>();
			if (productItem != null && productItem.HasChildren)
			{
				foreach (Item variant in productItem.Children)
				{
					VariantEntity model = ModelProvider.GetModel<VariantEntity>();
					model.Initialize(variant);
					variantEntityList.Add(model);
				}
			}
			ProductEntity productEntity = ModelProvider.GetModel<ProductEntity>();
			productEntity.Initialize(currentStorefront, productItem, variantEntityList);
			var renderingModel = ModelProvider.GetModel<ProductSummaryViewModel>();
			if (renderingModel.ProductId == currentStorefront.GiftCardProductId)
			{
				renderingModel.GiftCardAmountOptions = GetGiftCardAmountOptions(visitorContext, currentStorefront, productEntity);
			}
			else
			{
				CatalogManager.GetProductPrice(currentStorefront, visitorContext, productEntity);
				renderingModel.CustomerAverageRating = CatalogManager.GetProductRating(productItem);
			}
			renderingModel.Initialize(productEntity, false);
			return renderingModel;
		}

		protected virtual List<ProductEntity> AdjustProductPriceAndStockStatus(IVisitorContext visitorContext, SearchResults searchResult, Item currentCategory)
		{
			CommerceStorefront currentStorefront = this.StorefrontContext.CurrentStorefront;
			List<ProductEntity> list = new List<ProductEntity>();
			string str = "Category/" + currentCategory.Name;
			if (this.SiteContext.Items[(object)str] != null)
				return (List<ProductEntity>)this.SiteContext.Items[(object)str];
			if (searchResult.SearchResultItems != null && searchResult.SearchResultItems.Count > 0)
			{
				foreach (Item obj in searchResult.SearchResultItems)
				{
					ProductEntity model = this.ModelProvider.GetModel<ProductEntity>();
					model.Initialize(currentStorefront, obj, (List<VariantEntity>)null);
					list.Add(model);
				}
				this.CatalogManager.GetProductBulkPrices(currentStorefront, visitorContext, list);
				this.InventoryManager.GetProductsStockStatus(currentStorefront, list, currentStorefront.UseIndexFileForProductStatusInLists);
				foreach (ProductEntity productEntity1 in list)
				{
					ProductEntity productEntity = productEntity1;
					Item productItem = Enumerable.FirstOrDefault<Item>((IEnumerable<Item>)searchResult.SearchResultItems, (Func<Item, bool>)(item =>
					{
						if (item.Name == productEntity.ProductId)
							return item.Language == Context.Language;
						return false;
					}));
					if (productItem != null)
						productEntity.CustomerAverageRating = this.CatalogManager.GetProductRating(productItem);
				}
			}
			this.SiteContext.Items[(object)str] = (object)list;
			return list;
		}
	}
}