using ArtExplorer.BLL.Dtos;
using ArtExplorer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExplorer.BLL.Services.Interfaces;

public interface IFavoriteService
{
    string CreateFavorite(string userID, int artworkID);
    List<FavoriteDto> ReadFavorite(string userID);
    string DeleteFavorite(string userID, int artworkID);
}
