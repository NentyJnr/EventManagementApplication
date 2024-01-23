using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NCSEvent.API.Entities;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Services.Interfaces;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Services.Implementations
{
    public class MembershipTypeService : IMembershipTypeService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<MembershipTypeService> _logger;
        private readonly UserManager<Users> _userManager;



        public MembershipTypeService(AppDbContext dbContext, ILogger<MembershipTypeService> logger, UserManager<Users> userManager)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager;
        }

        public async Task<ServerResponse<MembershipType>> Create(MembershipTypeDTO request)
        {
            var response = new ServerResponse<MembershipType>();

            if (!request.IsValid(out ValidationResponse source))
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.BAD_REQUEST,
                    ResponseDescription = "Request Unsuccessful."
                };

                return response;
            }

            try
            {
                var existingEvent = await _dbContext.MembershipTypes
                    .FirstOrDefaultAsync(e => e.Name == request.Name && e.EventId == request.EventId);

                if (existingEvent != null)
                {
                    response.Error = new ErrorResponse

                    {
                        ResponseCode = ResponseCodes.RECORD_EXISTS,
                        ResponseDescription = "Membership Type Exists"
                    };
                    return response;
                }

                var newMembershipType = request.Adapt<MembershipType>();

                newMembershipType.IsActive = true;
                newMembershipType.DateCreated = DateTime.Now;
                newMembershipType.IsDeactivated = false;

                _dbContext.MembershipTypes.Add(newMembershipType);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = newMembershipType;
                response.SuccessMessage = "MembershipType created successfully.";

            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.FAIL,
                    ResponseDescription = "Request Not Succcessful"
                };
            }

            return response;
        }

        public async Task<ServerResponse<MembershipType>> Update(MembershipTypeDTO request)
        {
            var response = new ServerResponse<MembershipType>();

            if (!request.IsValid(out ValidationResponse source))
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.BAD_REQUEST,
                    ResponseDescription = "Request Unsuccessful."
                };
            }

            try
            {

                var existingMembershipType = await _dbContext.MembershipTypes.FindAsync(request.Id);
                if (existingMembershipType == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "MembershipType not found."
                    };

                    return response;
                }

                request.Adapt(existingMembershipType);

                existingMembershipType.Name = request.Name;
                existingMembershipType.Amount = request.Amount;
                existingMembershipType.EventId = request.EventId;
                existingMembershipType.IsActive = true;
                existingMembershipType.DateModified = DateTime.Now;

                _dbContext.MembershipTypes.Update(existingMembershipType);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = existingMembershipType;
                response.SuccessMessage = "MembershipType Updated successfully.";

            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    ResponseDescription = "Failed to update MembershipType."
                };
            }
            return response;
        }


        public async Task<ServerResponse<bool>> Delete(int Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingMembershipType = await _dbContext.MembershipTypes.FindAsync(Id);

                if (existingMembershipType == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.SUCCESS,
                        ResponseDescription = "MembershipType not found."
                    };

                    return response;
                }

                existingMembershipType.IsActive = false;

                _dbContext.MembershipTypes.Remove(existingMembershipType);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = true;
                response.SuccessMessage = "MembershipType Deleted successfully.";

            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    ResponseDescription = "Failed to delete MembershipType."
                };
            }

            return response;
        }


        public async Task<ServerResponse<List<MembershipType>>> GetAllRecord()
        {
            var response = new ServerResponse<List<MembershipType>>();

            try
            {
                var data = await _dbContext.MembershipTypes
                    .Select(e => new MembershipType
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Amount = e.Amount,
                        EventId = e.EventId,
                        Event = e.Event,
                        DateCreated = e.DateCreated,
                        IsActive = e.IsActive,
                        IsDeactivated = e.IsDeactivated,
                        DateModified = e.DateModified,
                    })
                    .ToListAsync();

                response.Data = data;
                response.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                    ResponseDescription = "Failed to fetch Event."
                };
                response.Data = new List<MembershipType>();
            }

            return response;
        }


        public async Task<ServerResponse<MembershipType>> GetAllRecordById(int id)
        {
            var response = new ServerResponse<MembershipType>();

            try
            {
                var record = await _dbContext.MembershipTypes
                    .Where(e => e.Id == id)
                    .Select(e => new MembershipType
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Amount = e.Amount,
                        EventId = e.EventId,
                        Event = e.Event,
                        DateCreated = e.DateCreated,
                        IsActive = e.IsActive,
                        IsDeactivated = e.IsDeactivated,
                        DateModified = e.DateModified,
                    })
                    .FirstOrDefaultAsync();

                if (record == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Event not found."
                    };

                    return response;
                }

                response.Data = record;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = $"An error occurred while retrieving the event: {ex.Message}"
                };
            }

            return response;
        }


        public async Task<ServerResponse<bool>> Activate(int id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingMembershipType = await _dbContext.MembershipTypes.FindAsync(id);

                if (existingMembershipType == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Event not found."
                    };

                    return response;
                }

                // Set IsActive only if existingEvent is not null
                existingMembershipType.IsActive = true;
                existingMembershipType.IsDeactivated = false;

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = true;
                response.SuccessMessage = "Event Activated successfully.";
            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.BAD_REQUEST,
                    ResponseDescription = $"An error occurred while activating the event: {ex.Message}"
                };
            }

            return response;
        }


        public async Task<ServerResponse<bool>> Deactivate(int id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingMembershipType = await _dbContext.MembershipTypes.FindAsync(id);

                if (existingMembershipType == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Event not found."
                    };

                    response.Data = false;

                    return response;
                }

                existingMembershipType.IsActive = false;
                existingMembershipType.IsDeactivated = true;

                _dbContext.MembershipTypes.Update(existingMembershipType);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = true;
                response.SuccessMessage = "Event Deactivated successfully.";
            }

            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                    ResponseDescription = "An error occurred while deactivating the event"
                };
                response.Data = false;
            }

            return response;
        }
    }
}

