using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.DTO;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IHotelManagementService
    {
        Task<ServerResponse<HotelManagement>> CreateHotel(HotelManagementDto request);
        Task<ServerResponse<List<HotelManagement>>> GetHotels();
        Task<ServerResponse<HotelManagement>> GetHotelById(long hotelId);
        Task<ServerResponse<bool>> UpdateHotel(long hotelId, HotelManagementDto request);
        Task<ServerResponse<bool>> DeleteHotel(long hotelId);
        Task<ServerResponse<bool>> DeactivateHotel(long hotelId);
        Task<ServerResponse<bool>> ActivateHotel(long hotelId);
    }
}
