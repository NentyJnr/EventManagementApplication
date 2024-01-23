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
using NCSEvent.API.Commons.Extensions;

namespace NCSEvent.API.Services.Implementations
{
    public class EventManagementService : IEventManagementService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EventManagementService> _logger;
        private readonly IUploadImageHelper _uploadImageHelper;

        public EventManagementService(AppDbContext dbContext, ILogger<EventManagementService> logger, IUploadImageHelper uploadImageHelper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadImageHelper = uploadImageHelper;
        }

        public async Task<ServerResponse<Events>> Create(EventManagementDTO request)
        {
            var response = new ServerResponse<Events>();

            try
            {
                var existingEvent = await _dbContext.Events
                    .FirstOrDefaultAsync(e => e.Name == request.Name);

                if (existingEvent != null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "No Event Found"
                    };
                    return response;
                }

                var newEvent = request.Adapt<Events>();

                //Handle Passport Upload Logic
                string coverImage = await _uploadImageHelper.UploadImage(request.CoverImage);
               newEvent.CoverImage = coverImage;

                newEvent.IsActive = true;
                newEvent.DateCreated = DateTime.Now;
                newEvent.IsDeactivated = false;

                _dbContext.Events.Add(newEvent);
                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = newEvent;
                response.SuccessMessage = "Event created successfully.";
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

        public async Task<ServerResponse<Events>> Update(EventUpdateDTO request)
        {
            var response = new ServerResponse<Events>();

            try
            {
                var existingEvent = await _dbContext.Events.FindAsync(request.Id);
                if (existingEvent == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Event not found."
                    };

                    return response;
                }

                existingEvent.Name = request.Name;
                existingEvent.StartDate = request.StartDate;
                existingEvent.EndDate = request.EndDate;
                existingEvent.Location = request.Location;
                existingEvent.EventType = request.EventType;
                existingEvent.Information = request.Information;
                existingEvent.IsActive = true;
                existingEvent.DateModified = DateTime.Now;

                string coverImage = await _uploadImageHelper.UploadImage(request.CoverImage);
                existingEvent.CoverImage = coverImage;

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = existingEvent;
                response.SuccessMessage = "Event Updated successfully.";
            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    ResponseDescription = "Failed to update Event."
                };
            }

            return response;
        }
        public async Task<ServerResponse<bool>> Delete(int Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingEvent = await _dbContext.Events.FindAsync(Id);

                if (existingEvent == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.SUCCESS,
                        ResponseDescription = "Event not found."
                    };

                    return response;
                }

                existingEvent.IsActive = false;

                _dbContext.Events.Remove(existingEvent);
                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = true;
                response.SuccessMessage = "Event Deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    ResponseDescription = "Failed to delete Event."
                };
            }

            return response;
        }

        public async Task<ServerResponse<List<Events>>> GetAllRecord()
        {
            var response = new ServerResponse<List<Events>>();

            try
            {
                var data = await _dbContext.Events
                    .Select(e => new Events
                    {
                        Id = e.Id,
                        Name = e.Name,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Location = e.Location,
                        DateCreated = e.DateCreated,
                        MembershipTypes = e.MembershipTypes,
                        EventImages = e.EventImages,
                        GuestSpeakers = e.GuestSpeakers,
                        CoverImage = e.CoverImage,
                        IsActive = e.IsActive,
                        IsDeactivated = e.IsDeactivated,
                        DateModified = e.DateModified,
                        EventType = e.EventType,
                        Information = e.Information,
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
            }

            return response;
        }

        public async Task<ServerResponse<Events>> GetRecordById(int id)
        {
            var response = new ServerResponse<Events>();

            try
            {
                var record = await _dbContext.Events
                    .Where(e => e.Id == id)
                    .Select(e => new Events
                    {
                        Id = e.Id,
                        Name = e.Name,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Location = e.Location,
                        DateCreated = e.DateCreated,
                        CoverImage = e.CoverImage,
                        MembershipTypes = e.MembershipTypes,
                        EventImages = e.EventImages,
                        GuestSpeakers = e.GuestSpeakers,
                        IsActive = e.IsActive,
                        IsDeactivated =e.IsDeactivated,
                        DateModified = e.DateModified,
                        EventType = e.EventType,
                        Information = e.Information,
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
                var existingEvent = await _dbContext.Events.FindAsync(id);

                if (existingEvent == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Event not found."
                    };

                    return response;
                }

                // Set IsActive only if existingEvent is not null
                existingEvent.IsActive = true;
                existingEvent.IsDeactivated = false;

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
                var existingEvent = await _dbContext.Events.FindAsync(id);

                if (existingEvent == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Event not found."
                    };

                    response.Data = false;

                    return response;
                }

                existingEvent.IsActive = false;
                existingEvent.IsDeactivated = true;

                _dbContext.Events.Update(existingEvent);
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

