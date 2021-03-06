using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Data;
using Library.Web.Models.Catalog;
using Library.Web.Models.CheckoutModels;


namespace Library.Web.Controllers {
    public class CatalogController : Controller {
            private ILibraryAsset _assets;
            private ICheckout _checkouts;
            public CatalogController (ILibraryAsset assets, ICheckout checkouts) {
                _assets = assets;
                _checkouts = checkouts;
            }

            public IActionResult Index() {
                var assetModels = _assets.GetAll();
                var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                    Title = result.Title,
                    Type = _assets.GetType(result.Id),
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id)
                });
            
                var model = new AssetIndexModel() {
                    Assets = listingResult
                };
                return View(model);
            }

            public IActionResult Detail(int id) {
                var asset = _assets.GetById(id);
                var currentHolds = _checkouts.GetCurrentHolds(id)
                    .Select(a => new AssetHoldModel 
                    {
                        HoldPlaced = _checkouts.GetCurrentHoldPlaced(a.Id).ToString("d"),
                        PatronName =_checkouts.GetCurrecntHoldPatronName(a.Id) 
                    });

                var model = new AssetDetailModel {
                    AssetId = id,
                    Title = asset.Title,
                    Year = asset.Year,
                    Cost = asset.Cost,
                    Status = asset.Status.Name,
                    ImageUrl = asset.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                    CurrentLocation = _assets.GetCurrentLocation(id).Name,
                    DeweyCallNumber = _assets.GetDeweyIndex(id),
                    ISBN = _assets.GetIsbn(id),
                    LatestCheckout =_checkouts.GetLatestCheckout(id),
                    CurrentHolds = currentHolds
                };

                return View(model);
            }
    
        public IActionResult Checkout(int id) {
                
                var asset = _assets.GetById(id);
                
                var model = new CheckoutModel {
                    AssetId = id,
                    ImageUrl = asset.Title,
                    LibraryCardId = "",
                    IsCheckedOut = _checkouts.IsCheckedOut(id)
                };
                return View(model);
            }
            
    }
}
        
