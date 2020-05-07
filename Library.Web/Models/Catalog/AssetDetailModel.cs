using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Models;


namespace Library.models.Catalog {
//Things to display on asset detail page
    public class AssetDetailModel {
        public int AssetId {get; set;}
        public string Title {get; set;}
        public string AuthorOrDirecotr {get; set;}
        public string Type {get; set;}
        public int Year {get; set;}
        public int ISBN {get; set;}

        public string DeweyCallNumber {get; set;}
        public string Status {get; set;}
        public decimal Cost  {get; set;}
        public string CurrentLocation  {get; set;}
        public string ImageUrl  {get; set;}

        public string PatronName  {get; set;}
        public Checkout LatestCheckout  {get; set;}
        public IEnumerable<CheckoutHistory> CHeckoutHistory {get; set;}
        public IEnumerable<AssetHoldModel> CurrentHolds {get; set;}

    }

    public class AssetHoldModel {
            public string PatronName {get; set;}
            public string HoldPlaced {get; set;}
    }
}