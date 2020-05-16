using System;
using System.Collections.Generic;
using Library.Data;
using Library.Data.Models;
using Library.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Library.Data.Models;

namespace Library.Services {
    public class CheckoutService : ICheckout {
        private LibraryContext _context;

        public CheckoutService(LibraryContext context) {
            _context = context;
        }
        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId, int libraryCardId) {
            //37:48 part 3
            var now = DateTime.Now;

            var item = _context.LibraryAssets
                .FirstOrDefault(a => a.Id == assetId);

            _context.Update(item);

            //remove existing checkouts on item
            //close all checkout history
            //look for existing holds on the item
            //if there exist holds, checkout the item to librarycard with earliest hold
            //else update item status to available
        }

        public void CheckOutItem(int assetId, int libraryCardId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return GetAll()
            .FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistories
                .Include(h =>h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(HashCode=>HashCode.LibraryAsset.Id == id);
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
            return _context.Holds
            .Include(h=>h.LibraryAsset)
            .Where(h => h.LibraryAsset.Id == id);
        }

        public Checkout GetLatestCheckout(int assetId)
        {
            //getall
            //order by descending
            //get fisrt
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == assetId)
                .OrderByDescending(c =>c.Since)
                .FirstOrDefault();
        }

        public void MarkFound(int assetId) {
            //update status of a itme to lost, implement "Available"
            
            var now = DateTime.Now;
            UpdateAssetStatus(assetId, "Available");
            //remove any existing checkouts (could have been checkout when lost)
            RemoveExistingCheckouts(assetId);

            CloseExistingCheckoutHistory(assetId, now);
            //close any checkout histories as well
            

            _context.SaveChanges();
        }

        

        public void MarkLost(int assetId) {
            //update status of a itme to lost, implement "Lost"
            
             UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            throw new NotImplementedException();
        }
        
        
        //HELPER fucntions
        private void RemoveExistingCheckouts(int assetId) {
            var checkout = _context.Checkouts
                .FirstOrDefault(co => co.LibraryAsset.Id == assetId);
            
            if (checkout != null) {
                _context.Remove(checkout);
            }
        }

        private void CloseExistingCheckoutHistory(int assetId, DateTime now) {
            var history = _context.CheckoutHistories
                .FirstOrDefault(h=> h.LibraryAsset.Id ==assetId && h.CheckedIn ==null);
            if (history != null) {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void UpdateAssetStatus(int assetId, string statVal) {
            var item = _context.LibraryAssets
            .FirstOrDefault(a => a.Id==assetId);
            
            _context.Update(item);

            item.Status = _context.Statuses
                .FirstOrDefault(status => status.Name ==statVal);
        }
    }

}