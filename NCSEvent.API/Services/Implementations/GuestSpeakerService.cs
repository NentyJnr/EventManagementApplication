using Mapster;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Services.Implementations
{
    public class GuestSpeakerService : IGuestSpeaker
    {
        private readonly AppDbContext _dbContext;
        private readonly IUploadImageHelper _uploadImageHelper;
        public GuestSpeakerService(AppDbContext dbContext, IUploadImageHelper uploadImageHelper)
        {
            _dbContext = dbContext;
            _uploadImageHelper = uploadImageHelper;
        }

        public async Task<ServerResponse<GuestSpeaker>> Create(GuestSpeakerDTO request)
        {
            var response = new ServerResponse<GuestSpeaker>();

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
                //var existingSpeaker = await _dbContext.GuestSpeakers
                //    .FirstOrDefaultAsync(e => e.Name == request.Name && e.EventId == request.EventId);

                //if (existingEvent != null)
                //{
                //    response.Error = new ErrorResponse

                //    {
                //        ResponseCode = ResponseCodes.RECORD_EXISTS,
                //        ResponseDescription = "Membership Type Exists"
                //    };
                //    return response;
                //}

                var newSpeaker = request.Adapt<GuestSpeaker>();

                newSpeaker.IsActive = true;
                newSpeaker.DateCreated = DateTime.Now;
                newSpeaker.IsDeactivated = false;

                string image = await _uploadImageHelper.UploadImage(request.Image);
                newSpeaker.ImageUrl = image;

                _dbContext.GuestSpeakers.Add(newSpeaker);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = newSpeaker;
                response.SuccessMessage = "Guest Speaker created successfully.";

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

        public async Task<ServerResponse<GuestSpeaker>> Update(UpdateGuestSpeakerDTO request)
        {
            var response = new ServerResponse<GuestSpeaker>();

            try
            {

                var existingSpeaker = await _dbContext.GuestSpeakers.FindAsync(request.Id);
                if (existingSpeaker == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Guest Speaker not found."
                    };

                    return response;
                }

                request.Adapt(existingSpeaker);
                existingSpeaker.DateModified = DateTime.Now;
                string image = await _uploadImageHelper.UploadImage(request.Image);
                existingSpeaker.ImageUrl = image;


                _dbContext.GuestSpeakers.Update(existingSpeaker);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = existingSpeaker;
                response.SuccessMessage = "Guest Speaker Updated successfully.";

            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.SUCCESS,
                    ResponseDescription = "Failed to update Speaker."
                };
            }
            return response;
        }


        public async Task<ServerResponse<bool>> Delete(int Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingSpeaker = await _dbContext.GuestSpeakers.FindAsync(Id);

                if (existingSpeaker == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.SUCCESS,
                        ResponseDescription = "Guest Speaker not found."
                    };

                    return response;
                }

                existingSpeaker.IsActive = false;

                _dbContext.GuestSpeakers.Remove(existingSpeaker);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = true;
                response.SuccessMessage = "Guest Speaker Deleted successfully.";

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


        public async Task<ServerResponse<List<GuestSpeaker>>> GetAllRecord()
        {
            var response = new ServerResponse<List<GuestSpeaker>>();

            try
            {
                var data = await _dbContext.GuestSpeakers
                    .Select(e => new GuestSpeaker
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Biography = e.Biography,
                        Topic = e.Topic,
                        ImageUrl = e.ImageUrl,
                        EventId = e.EventId,
                        Event = e.Event,
                        DateCreated = e.DateCreated,
                        IsActive = e.IsActive,
                        IsDeactivated = e.IsDeactivated,
                        DateModified = e.DateModified
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
                response.Data = new List<GuestSpeaker>();
            }

            return response;
        }


        public async Task<ServerResponse<GuestSpeaker>> GetRecordById(int id)
        {
            var response = new ServerResponse<GuestSpeaker>();

            try
            {
                var record = await _dbContext.GuestSpeakers
                    .Where(e => e.Id == id)
                    .Select(e => new GuestSpeaker
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Biography = e.Biography,
                        Topic = e.Topic,
                        ImageUrl = e.ImageUrl,
                        EventId = e.EventId,
                        Event = e.Event,
                        DateCreated = e.DateCreated,
                        IsActive = e.IsActive,
                        IsDeactivated = e.IsDeactivated,
                        DateModified = e.DateModified
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
                var existingSpeaker = await _dbContext.GuestSpeakers.FindAsync(id);

                if (existingSpeaker == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Speaker not found."
                    };

                    return response;
                }

                existingSpeaker.IsActive = true;
                existingSpeaker.IsDeactivated = false;

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = true;
                response.SuccessMessage = "Speaker Activated successfully.";
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
                var existingSpeaker = await _dbContext.GuestSpeakers.FindAsync(id);

                if (existingSpeaker == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Speaker not found."
                    };

                    response.Data = false;

                    return response;
                }

                existingSpeaker.IsActive = false;
                existingSpeaker.IsDeactivated = true;

                _dbContext.GuestSpeakers.Update(existingSpeaker);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = true;
                response.SuccessMessage = "Speaker Deactivated successfully.";
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
