using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IAccount
    {
        Task<ServerResponse<LoginResponse>> Login(string email, string password);
        Task<ServerResponse<List<UserDTO>>> GetAllRecord();
        Task<ServerResponse<bool>> LogOut();
    }
}
