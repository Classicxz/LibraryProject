﻿using System;
using System.Collections.Generic;
using Library.Data;
using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Services
{
    public class LibraryAssetService : ILibraryAsset {
        private LibraryContext _context;
        public LibraryAssetService(LibraryContext context) {
            _context = context;
        }
        public void Add(LibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return _context.LibraryAssets
            .Include(asset => asset.Status)
            .Include(asset => asset.Location);
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>()
            .Where(asset => asset.Id ==id).Any();

            var isVideo = _context.LibraryAssets.OfType<Video>()
            .Where(asset => asset.Id ==id).Any();

            //add more assets like magazines or kindle asset
            //ternary
            return isBook ?
            _context.Books.FirstOrDefault(book => book.Id == id).Author :
            _context.Videos.FirstOrDefault(video => video.Id == id).Director
            ?? "Unkown";
        }

        public LibraryAsset GetById(int id)
        {
            return GetAll()
            .FirstOrDefault(asset => asset.Id ==id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
            //return _context.LibraryAssets.FirstOrDefault(asset => asset.Id==id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            //Need to worry about discriminator
            if (_context.Books.Any(book => book.Id == id)) {
                return _context.Books.First(book => book.Id ==id).DeweyIndex;
            } else return "";
        }

        public string GetIsbn(int id)
        {
            if (_context.Books.Any(book => book.Id == id)) {
                return _context.Books.First(book => book.Id ==id).ISBN;
            } else return "";
        }

        public string GetTitle(int id)
        {
            return _context.LibraryAssets
            .FirstOrDefault(a => a.Id ==id)
            .Title;
        }

        public string GetType(int id)
        {
            var book = _context.LibraryAssets.OfType<Book>()
            .Where(b => b.Id == id);

            //redo this later
            return book.Any() ? "Book" :"Video";
        }
    }
}
