using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<ServerResponse<FormDTO>> IsMemberRegistered(VerifyMembershipNoDTO request);
        Task<ServerResponse<PaymentSummaryDTO>> Register(FormDTO request);
        Task<ServerResponse<List<RegistrationForm>>> GetAllGuests(int id);
    }
}
