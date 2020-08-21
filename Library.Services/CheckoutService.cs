using System;
using System.Collections.Generic;
using Library.Data;
using Library.Data.Models;
using Library.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;


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

            RemoveExistingCheckouts(assetId);
            CloseExistingCheckoutHistory(assetId, now);

            var currentHolds = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == assetId);
            
            if (currentHolds.Any()) {
                CheckoutToEarliestHold(assetId, currentHolds);
            }

            UpdateAssetStatus(assetId, "Available");

            _context.SaveChanges();
            //remove existing checkouts on item
            //close all checkout history
            //look for existing holds on the item
            //if there exist holds, checkout the item to librarycard with earliest hold
            //else update item status to available
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds) {
            
            var earliestHold = currentHolds
                .OrderBy(holds => holds.HoldPlaced)
                .FirstOrDefault();

            var card = earliestHold.LibraryCard;
            
            //fufilled hold so remove it
            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckOutItem(assetId, card.Id);
        }

        public void CheckOutItem(int assetId, int libraryCardId) {
            if (IsCheckedOut(assetId)) {
                return;
                //Maybe some logic stuff?
            }
            var item = _context.LibraryAssets
                .FirstOrDefault(a => a.Id==assetId);

            UpdateAssetStatus(assetId, "Checked Out");
            var libraryCard = _context.LibraryCard
                .Include(card => card.Checkouts)
                .FirstOrDefault(card => card.Id == libraryCardId);
            
            var now = DateTime.Now;

            var checkout = new Checkout {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            //add to checkout table

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        public bool IsCheckedOut(int assetId) {
            var isCheckedOut = _context.Checkouts
            .Where(co =>co.LibraryAsset.Id == assetId)
            .Any();

            //false if no values

            return isCheckedOut;
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

        public string GetCurrecntHoldPatronName(int holdId)  {
            var hold = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id ==holdId);

            //POSSIBLE null exception use null conditional
            var cardId = hold?.LibraryCard.Id;

            var patron = _context.Patrons. Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron?.FirstName + " " + patron.LastName;

        }

        public DateTime GetCurrentHoldPlaced(int holdId) {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id ==holdId)
                .HoldPlaced;
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

        public void PlaceHold(int assetId, int libraryCardId) {
            var now = DateTime.Now;

            var asset = _context.LibraryAssets
                .FirstOrDefault(a => a.Id ==assetId);
            
            var card = _context.LibraryCard
                .FirstOrDefault(c => c.Id == libraryCardId);

            if(asset.Status.Name == "Available") {
                UpdateAssetStatus(assetId, "On Hold");
            }

            var hold = new Hold {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            _context.Add(hold);
            _context.SaveChanges();

            


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

        public string GetCurrentCheckoutPatron(int assetId) {
            var checkout = GetCheckoutByAssetId(assetId);

            if (checkout == null) {
                return "";
            }

            var cardId = checkout.LibraryCard.Id;

            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.LastName;
        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context.Checkouts
                .Include (c => c.LibraryAsset)
                .Include (c => c.LibraryCard)
                .FirstOrDefault(c => c.LibraryAsset.Id == assetId);
        }

       
    }

}