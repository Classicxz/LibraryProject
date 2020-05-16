using System;
using System.Collections.Generic;
using System.Text;
using Library.Data.Models;

namespace Library.Data {
    public interface ICheckout {
        IEnumerable<Checkout> GetAll();
        Checkout GetById(int checkoutId);
        void Add(Checkout newCheckout);
        void CheckOutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId, int libraryCardId);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);

        void PlaceHold(int assetId, int libraryCardId);
        string GetCurrecntHoldPatronName(int id);

        DateTime GetCurrentHoldPlaced(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);

        Checkout GetLatestCheckout(int assetId);

        void MarkLost(int assetId);
        void MarkFound(int assetId);
    }
}