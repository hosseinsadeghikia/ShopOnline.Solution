using ShopOnline.Core.Models;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Core.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDto userDto);
        Task<string> CreateToken();
        Task<string> CreateRefreshToken();
        Task<TokenRequest?> VerifyRefreshToken(TokenRequest request);
    }
}
