using ArtExplorer.BLL.Dtos;
using ArtExplorer.BLL.Services.Interfaces;
using ArtExplorer.DAL.Data;
using ArtExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExplorer.BLL.Services;

public class FavoriteService : IFavoriteService
{
    private readonly AppDbContext _context;

    public FavoriteService(AppDbContext context)
    {
        _context = context;
    }

    public string CreateFavorite(string userID, int artworkID)
    {
        bool alreadyFavorited =  _context.Favorites.Any(f => f.UserId == userID && f.ArtworkId == artworkID);

        if (alreadyFavorited)
        {
            return "Artwork is already in favorites.";
        }

        var favorite = new Favorite
        {
            UserId = userID,
            ArtworkId = artworkID
        };

        _context.Favorites.Add(favorite);
        _context.SaveChanges();

        return "Artwork added to favorites successfully.";
    }

    public List<FavoriteDto> ReadFavorite(string userID)
    {
        var favorites = _context.Favorites
            .Where(f => f.UserId == userID)
            .Select(f => new FavoriteDto
            {
                UserId = f.UserId,
                ArtworkId = f.ArtworkId
            })
            .ToList();

        return favorites;
    }

    public string UpdateFavorite(string userID, int artworkID, int newArtworkID)
    {
        var favorite = _context.Favorites.FirstOrDefault(f => f.UserId == userID && f.ArtworkId == artworkID);

        if (favorite == null)
        {
            return "Favorite not found.";
        }

        _context.Favorites.Remove(favorite);

        var newFavorite = new Favorite
        {
            UserId = userID,
            ArtworkId = newArtworkID
        };

        _context.Favorites.Add(newFavorite);

        _context.SaveChanges();

        return "Favorite updated successfully.";
    }

    public string DeleteFavorite(string userID, int artworkID)
    {
        var favorite = _context.Favorites.FirstOrDefault(f => f.UserId == userID && f.ArtworkId == artworkID);

        if (favorite == null)
        {
            return "Favorite not found.";
        }

        _context.Favorites.Remove(favorite);
        _context.SaveChanges();

        return "Favorite removed successfully.";
    }
}
