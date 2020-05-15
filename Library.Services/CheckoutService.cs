using System;
using System.Collections.Generic;
using Library.Data;
using Library.Data.Models;
using Library.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Library.Data.Models;

namespace Library.Services {
    public class CheckoutService : ICheckout
    {
        public void Add(Checkout newCheckout)
        {
            throw new NotImplementedException();
        }

        public void CheckInItem(int assetId, int libraryCardId)
        {
            throw new NotImplementedException();
        }

        public void CheckOutItem(int assetId, int libraryCardId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Checkout> GetAll()
        {
            throw new NotImplementedException();
        }

        public Checkout GetById(int checkoutId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CheckoutHistory> GetCHeckoutHistory(int id)
        {
            throw new NotImplementedException();
        }

        public string GetCurrecntHoldPatronName(int id)
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentHoldPlaced(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            throw new NotImplementedException();
        }

        public void MarkFound(int assetId)
        {
            throw new NotImplementedException();
        }

        public void MarkLost(int assetId)
        {
            throw new NotImplementedException();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            throw new NotImplementedException();
        }
    }

}