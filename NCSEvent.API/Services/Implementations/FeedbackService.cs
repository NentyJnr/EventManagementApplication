using Mapster;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Services.Implementations
{
    public class FeedbackService : IFeedback
    {
        private readonly AppDbContext _dbContext;
        public FeedbackService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServerResponse<Feedbacks>> Create(FeedbackDTO request)
        {
            var response = new ServerResponse<Feedbacks>();

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
                var newFeedback = request.Adapt<Feedbacks>();

                newFeedback.IsActive = true;
                newFeedback.DateCreated = DateTime.Now;
                newFeedback.IsDeactivated = false;

                _dbContext.Feedbacks.Add(newFeedback);

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Data = newFeedback;
                response.SuccessMessage = "Feedback added successfully.";

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

        public async Task<ServerResponse<List<Feedbacks>>> GetAllRecord()
        {
            var response = new ServerResponse<List<Feedbacks>>();

            try
            {
                var data = await _dbContext.Feedbacks
                    .Select(e => new Feedbacks
                    {
                        Id = e.Id,
                        Feedback = e.Feedback,
                        Rating = e.Rating,
                        Suggestion = e.Suggestion,
                        EventId = e.EventId,
                        Event = e.Event,
                        DateCreated = e.DateCreated
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
                    ResponseDescription = "Failed to fetch Feedbacks."
                };
                response.Data = new List<Feedbacks>();
            }

            return response;
        }
    }
}
