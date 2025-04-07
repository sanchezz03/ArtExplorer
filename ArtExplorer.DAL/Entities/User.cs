using Microsoft.AspNetCore.Identity;

namespace ArtExplorer.DAL.Entities;

public class User : IdentityUser
{
    public string DisplayName { get; set; }
}
