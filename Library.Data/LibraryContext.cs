using System;
using Microsoft.EntityFrameworkCore;
using Library.Data.Models;

namespace Library.Data
{
    public class LibraryContext: DbContext {
        public LibraryContext(DbContextOptions options) : base(options) {  }

        public DbSet<Patron> Patrons {get; set; }
        public DbSet<Book> Books {get; set;}
        public DbSet<BranchHours> BranchHours {get; set;}
        public DbSet<Checkout> Checkouts {get; set;}
        public DbSet<CheckoutHistory> CheckoutHistories {get; set;}
        public DbSet<Hold> Holds {get; set;}
        public DbSet<LibraryAsset> LibraryAssets {get; set;}
        public DbSet<LibraryBranch> LibraryBranches {get; set;}
        public DbSet<LibraryCard> LibraryCard {get; set;}
        public DbSet<Status> Statuses {get; set;}
        public DbSet<Video> Videos {get; set;}

    }
}
