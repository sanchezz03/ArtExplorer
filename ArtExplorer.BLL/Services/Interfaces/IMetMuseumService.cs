using ArtExplorer.BLL.Dtos;

namespace ArtExplorer.BLL.Services.Interfaces;

public interface IMetMuseumService
{
    Task<(List<ArtDto> Results, int TotalCount)> SearchArtworksAsync(string? author, string? year, int page, int pageSize);

    Task<ArtDto?> GetArtworkByIdAsync(int objectId);
}
