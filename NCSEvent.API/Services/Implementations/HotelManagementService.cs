using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.DTO;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Services.Implementations
{
    public class HotelManagementService : IHotelManagementService
    {
        private readonly AppDbContext _dbContext;

        public HotelManagementService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ServerResponse<HotelManagement>> CreateHotel(HotelManagementDto request)
        {
            var response = new ServerResponse<HotelManagement>();

            var hotel = new HotelManagement();
            hotel.HotelAddress = request.HotelAddress;
            hotel.HotelName = request.HotelName;
            hotel.HotelType = request.HotelType;
            hotel.IsDeleted = false;
            hotel.IsActive = true;
            hotel.Amount = request.Amount;
            hotel.Contact = request.Contact;
            hotel.RoomAvailability = request.RoomAvailability;
            hotel.RoomType = request.RoomType;
            hotel.DateCreated = DateTime.Now;
            hotel.DateModified = DateTime.Now;

            await _dbContext.AddAsync(hotel);
            var res = await _dbContext.SaveChangesAsync() > 0;
            if (res)
            {
                response.SuccessMessage = "Hotel successfully created";
                response.IsSuccessful = true;
                response.Data = hotel;
            }
            else
            {
                response.SuccessMessage = "Something went wrong hotel can not be created at the moment";
                response.IsSuccessful = false;
            }

            return response;
        }

        public async Task<ServerResponse<List<HotelManagement>>> GetHotels()
        {
            var response = new ServerResponse<List<HotelManagement>>();
            var allHotels = await _dbContext.Hotels.ToListAsync();

            response.SuccessMessage = allHotels.Count > 0
                ? "List of hotels retrieved successfully"
                : "No hotels found.";

            response.IsSuccessful = true;
            response.Data = allHotels;

            return response;
        }

        public async Task<ServerResponse<HotelManagement>> GetHotelById(long hotelId)
        {
            var response = new ServerResponse<HotelManagement>();

            var hotel = await _dbContext.Hotels.FindAsync(hotelId);

            if (hotel == null)
            {
                response.SuccessMessage = "hotel not found";
                response.IsSuccessful = false;
            }
            else
            {
                response.SuccessMessage = "hotel successfully retrieved";
                response.Data = hotel;
                response.IsSuccessful = true;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> UpdateHotel(long hotelId, HotelManagementDto request)
        {
            var response = new ServerResponse<bool>();


            var hotel = await _dbContext.Hotels.FindAsync(hotelId);

            if (hotel != null)
            {
                hotel.HotelName = request.HotelName;
                hotel.HotelAddress = request.HotelAddress;
                hotel.HotelType = request.HotelType;
                hotel.Amount = request.Amount;
                hotel.Contact = request.Contact;
                hotel.DateModified = DateTime.Now;
                hotel.DateCreated = DateTime.Now;
                hotel.IsActive = request.IsActive;
                hotel.IsDeleted = request.IsDeleted;

                //_dbContext.Update(hotel);
                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Hotel updated successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Hotel not found";
                response.IsSuccessful = false;
            }


            return response;
        }


        public async Task<ServerResponse<bool>> DeleteHotel(long hotelId)
        {
            var response = new ServerResponse<bool>();

            var hotel = await _dbContext.Hotels.FindAsync(hotelId);

            if (hotel != null)
            {
                _dbContext.Hotels.Remove(hotel);
                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Hotel deleted successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Hotel not found";
                response.IsSuccessful = false;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> DeactivateHotel(long hotelId)
        {
            var response = new ServerResponse<bool>();

            var hotel = await _dbContext.Hotels.FindAsync(hotelId);
            if (hotel != null)
            {
                hotel.IsActive = false;

                _dbContext.Update(hotel);

                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Hotel Deactivated successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Hotel not found";
                response.IsSuccessful = false;
            }


            return response;
        }


        public async Task<ServerResponse<bool>> ActivateHotel(long hotelId)
        {
            var response = new ServerResponse<bool>();

            var hotel = await _dbContext.Hotels.FindAsync(hotelId);
            if (hotel != null)
            {
                hotel.IsActive = true;

                _dbContext.Update(hotel);

                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Hotel Activated successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Hotel not found";
                response.IsSuccessful = false;
            }


            return response;
        }

    }
}
