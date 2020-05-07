using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Data;
using Library.Web.Models.Catalog;


namespace Library.Web.Controllers {
    public class CatalogController : Controller {
            private ILibraryAsset _assets;
            public CatalogController (ILibraryAsset assets) {
                _assets = assets;
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
                var model = new AssetDetailModel {
                    AssetIndexListingModel = id,
                    TypeFilterAttribute = asset.Title,
                    Year = asset.Year,
                    Cost = asset.Cost,
                    StatusCode = asset.Status.Name,
                }
            }
    }
}
