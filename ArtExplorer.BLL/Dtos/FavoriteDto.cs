using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExplorer.BLL.Dtos;

public class FavoriteDto
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public int ArtworkId { get; set; }
}
