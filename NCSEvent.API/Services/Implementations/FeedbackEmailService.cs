using NCSEvent.API.Commons.Extensions;
using NCSEvent.API.Commons.Models;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Implementations
{
    public class FeedbackEmailService
    {
        private readonly AppDbContext _context;
        private readonly EmailHelper _emailHelper;

        public FeedbackEmailService(AppDbContext context, EmailHelper emailHelper)
        {
            _context = context;
            _emailHelper = emailHelper;
        }

        public void SendFeedbackEmails()
        {
            var startedEvents = _context.Events.Where(e => e.StartDate <= DateTime.Now && e.EventTime <= DateTime.Now && e.EndDate >= DateTime.Now);

            foreach (var @event in startedEvents)
            {
                var guests = _context.RegistrationForms
                    .Where(r => r.EventManagementId == @event.Id && r.PaymentConfirmed == true && r.FeedbackEmailSent == false)
                    .ToList();

                foreach (var guest in guests)
                {

                    var feedbackLink = "https://ncs-event-app.vercel.app/feedback-form/" + guest.EventManagementId;

                    var emailBodyRequest = new EmailBodyRequest
                    {
                        EventId = @event.Id,
                        GuestId = guest.Id,
                        FeedbackLink = feedbackLink
                    };

                    _emailHelper.SendMail(emailBodyRequest).Wait();

                    guest.FeedbackEmailSent = true;
                    _context.SaveChanges();
                }
            }
        }
    }
}
