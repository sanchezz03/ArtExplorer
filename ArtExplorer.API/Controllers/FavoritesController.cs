using ArtExplorer.BLL.Dtos;
using ArtExplorer.BLL.Services;
using ArtExplorer.BLL.Services.Interfaces;
using ArtExplorer.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ArtExplorer.API.Controllers;

[Route("api/favorites")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] FavoriteDto favoriteDto)
    {
        var userId = favoriteDto.UserId;
        var artworkId = favoriteDto.ArtworkId;

        var result = _favoriteService.CreateFavorite(userId, artworkId);
        return Ok(new { message = result });
    }

    [HttpGet("Read/{userId}")]
    public async Task<IActionResult> Read(string userId)
    {
        var favorites = _favoriteService.ReadFavorite(userId);
        return Ok(favorites);
    }

    [HttpPost("Update/{newArtworkId}")]
    public async Task<IActionResult> Update([FromBody] FavoriteDto favoriteDto, int newArtworkId)
    {
        var userId = favoriteDto.UserId;
        var artworkId = favoriteDto.ArtworkId;
        var result = _favoriteService.UpdateFavorite(userId, artworkId, newArtworkId);
        return Ok(new { message = result });
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] FavoriteDto favoriteDto)
    {
        var userId = favoriteDto.UserId;
        var artworkId = favoriteDto.ArtworkId;

        var result = _favoriteService.DeleteFavorite(userId, artworkId);
        return Ok(new { message = result });
    }
}

