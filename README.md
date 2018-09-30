# ARCommerce.ProductAPI

[Sitecore Commerce](https://dev.sitecore.net/Downloads/Sitecore_Commerce.aspx) product API to use in Augmented Reality application. Developed with [Apple ARKit](https://developer.apple.com/arkit/)

## Usage

Project adds two API endpoints for accessing products data:
* Get product scans for given product category item id: <br/>
 GET `/api/cxa/ARCatalog/GetProductScans?cci={catalog item id}`
 Return: 
  
* Get product details for given product id: <br/>
 GET `/api/cxa/ARCatalog/ProductInformation?id={product id}`

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
