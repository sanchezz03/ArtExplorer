using Microsoft.AspNetCore.Identity;

namespace ArtExplorer.DAL.Entities;

public class Favorite
{
    public string UserId { get; set; }
    public int ArtworkId { get; set; }

    public User User { get; set; }
}
