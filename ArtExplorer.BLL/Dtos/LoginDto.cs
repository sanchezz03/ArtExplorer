using System.ComponentModel.DataAnnotations;

namespace ArtExplorer.BLL.Dtos;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}

