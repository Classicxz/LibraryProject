using System;
using System.Collections.Generic;
using System.Text;
using Library.Data.Models;

namespace Library.Data {
    //interface for LibraryAssets to get, set, add assets
    public interface ILibraryAsset {
                IEnumerable<LibraryAsset> GetAll();
                LibraryAsset GetById(int id);
                void Add(LibraryAsset newAsset);
                string GetAuthorOrDirector(int id);
                string GetDeweyIndex(int id);
                string GetType(int id);
                string GetTitle(int id);
                string GetIsbn(int id);

                LibraryBranch GetCurrentLocation(int id);
    }

}