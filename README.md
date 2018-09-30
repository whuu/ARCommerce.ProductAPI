# ARCommerce.ProductAPI

[Sitecore Commerce](https://dev.sitecore.net/Downloads/Sitecore_Commerce.aspx) product API to use in Augmented Reality application. Developed with [Apple ARKit](https://developer.apple.com/arkit/)

## Usage

Project adds two API endpoints for accessing products data:
* Get product scans for given product category item id: <br/>
 GET `/api/cxa/ARCatalog/GetProductScans?cci={catalog item id}`<br/>
 Returned JSON: 
	```
	[
			{
			  "ProductId":"123456",
			  "ScanFilePath":"/-/media/AR-Maps/123456/iron.arobject"
			},
			...
	]
	```
  
* Get product details for given product id: <br/>
 GET `/api/cxa/ARCatalog/ProductInformation?id={product id}` <br/>
	Returned JSON:
	```
	{
	     "DisplayName":"Azur 10/02",
	     "Description":"Text Description.",
         "SummaryImageUrl":"/-/media/Images/Habitat/123456.png?h=220\u0026w=263\u0026hash=5C258CA2D56764DF5E0ACCEBDEB186BA",
         "Link":"/shop/Connectedhome%3dhabitat_master-connected%20home/PhilipsAzurGC4410%5B%5BSS%5D%5D02%3d123456",
         "ProductId":"123456",
         "AdjustedPriceWithCurrency":"7.00 USD",
         "ListPriceWithCurrency":"45.00 USD",
         "StockStatusLabel":"Out of Stock",
         ...
	}
	```

Sample iOS client code for this API can be found [here](https://github.com/whuu/ARCommerce.ClientApp)

## Requirements

Sitecore XC 9.0 Update-2 with SXA

## Installation

### Put files into `\lib\Modules\Commerce` folder: 
* `Sitecore.Commerce.XA.Feature.Catalog.dll`
* `Sitecore.Commerce.XA.Foundation.Catalog.dll`
* `Sitecore.Commerce.XA.Foundation.Common.dll`
* `Sitecore.Commerce.XA.Foundation.Connect.dll`

### Deploy `ARCommerce.Feature.Catalog.csproj` to your Sitecore Commerce website.
