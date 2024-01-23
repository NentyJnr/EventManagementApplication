using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Services.Implementations
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _context;
        public SessionService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ServerResponse<bool>> CreateSessionAsync(SessionDTO request)
        {
            var response = new ServerResponse<bool>();
            if (!request.IsValid(out ValidationResponse source))
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = source.Code,
                    ResponseDescription = source.Message
                };
                return response;
            }

            var dataMapped = request.Adapt<Sessions>();
            if (dataMapped is null)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.INVALID_OBJECT_MAPPING
                };
                return response;
            }
            dataMapped.DateCreated = DateTime.Now;
            dataMapped.IsActive = true;
            await _context.Sessions.AddAsync(dataMapped);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                response.IsSuccessful = true;
                response.Data = true; response.SuccessMessage = "Successful";
            }
            else
            {
                response.SuccessMessage = "Request Not Successful";
            }
            return response;
        }

        public async Task<ServerResponse<bool>> DeleteSessionAsync(string userId)
        {
            var response = new ServerResponse<bool>();
            if (string.IsNullOrWhiteSpace(userId))
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.INVALID_PARAMETER,
                    ResponseDescription = "Invalid Parameter"
                };
                return response;

            }

            var record = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
            if (record == null)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                    ResponseDescription = "Record does not exist"
                };
                return response;

            }
            _context.Sessions.Remove(record);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                response.Data = true; response.SuccessMessage = "Successful";
            }
            else
            {
                response.SuccessMessage = "Request not Successful";
            }
            return response;
        }

        public async Task<ServerResponse<bool>> UpdateSessionAsync(UpdateSessionDTO request)
        {
            var response = new ServerResponse<bool>();
            if (request.IsValid(out ValidationResponse source))
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = source.Code,
                    ResponseDescription = source.Message
                };
                return response;
            }

            var record = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId.Equals(request.UserId));
            if (record is null)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.INVALID_OBJECT,
                    ResponseDescription = "Invalid Object"
                };
                return response;
            }

            record.Token = request.Token;
            _context.Sessions.Update(record);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                response.Data = true; response.SuccessMessage = "Successful";
            }
            else
            {
                response.SuccessMessage = ResponseCodes.REQUEST_NOT_SUCCESSFUL;
            }
            return response;
        }
    }
}
