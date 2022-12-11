using ShopOnline.Core.DTOs;
using ShopOnline.Core.Models;

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
