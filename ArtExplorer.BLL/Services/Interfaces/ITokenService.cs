using ArtExplorer.DAL.Entities;

namespace ArtExplorer.BLL.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
