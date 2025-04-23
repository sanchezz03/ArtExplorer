using ArtExplorer.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtExplorer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtworksController : ControllerBase
{
    private readonly IMetMuseumService _service;

    public ArtworksController(IMetMuseumService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetArtworks(
        [FromQuery] string? author,
        [FromQuery] string? year,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (results, totalCount) = await _service.SearchArtworksAsync(author, year, page, pageSize);
        return Ok(new { totalCount, results });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetArtworkById(int id)
    {
        var artwork = await _service.GetArtworkByIdAsync(id);
        if (artwork == null)
            return NotFound();

        return Ok(artwork);
    }
}
