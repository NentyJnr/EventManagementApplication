using Microsoft.EntityFrameworkCore.Storage;
using NCSEvent.API.Entities;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using NCSEvent.API.Commons.Models;
using NCSEvent.API.Commons.DTO;
using Microsoft.EntityFrameworkCore;

namespace NCSEvent.API.Commons.Extensions
{
    public class EmailHelper
    {
        private readonly EmailServiceBinding _emailServiceBinding;
        private readonly AppDbContext _context;
        private readonly IHostEnvironment _environment;
        public EmailHelper(EmailServiceBinding emailServiceBinding, AppDbContext context, IHostEnvironment environment)
        {
            _emailServiceBinding = emailServiceBinding;
            _context = context;
            _environment = environment;
        }

        public async Task<EmailServiceResponse> SendMail(EmailBodyRequest request)
        {
            var result = new EmailServiceResponse();

            Events events = await _context.Events.FirstOrDefaultAsync(u => u.Id == request.EventId);

            RegistrationForm user = await _context.RegistrationForms.FirstOrDefaultAsync(u => u.Id == request.GuestId);

            var fullName = $"{user.FirstName} {user.LastName}";

            string htmlPath = _environment.ContentRootPath + Path.DirectorySeparatorChar + "EmailTemplates/FeedbackTemplate.html";
            string htmlContent = Convert.ToString(Utilities.ReadHtmlFile(htmlPath));
            var body = htmlContent
                .Replace("{FIRSTNAME}", user.FirstName)
                .Replace("{EVENT}", events.Name)
                .Replace("{FEEDBACKLINK}", request.FeedbackLink);
            var emailPayLoad = new EmailServiceModel
            {
                from = _emailServiceBinding?.Sender ?? "",
                messageBody = body,
                projectCode = _emailServiceBinding?.ProjectCode ?? "",
                to = user.Email,
                sentNow = true,
                subject = "Feedback",
                recieverName = fullName,
                scheduleDate = DateTime.Now,
                senderName = _emailServiceBinding?.SenderName ?? "N/A",
            };

            using (var httpClient = new HttpClient())
            {
                var url = _emailServiceBinding.BaseUrl;
                string path = _emailServiceBinding.PostMEssage;
                httpClient.BaseAddress = new Uri(url);
                emailPayLoad.scheduleDate = DateTime.Now;
                emailPayLoad.otherEmails = new List<OtherEmail>
                {
                    new OtherEmail
                    {
                        bbcRecieverEmail = "",
                        bbcRecieverName = "",
                        ccRecieverEmail = "",
                        ccRecieverName = ""
                    }
                                };
                var payLoad = JsonConvert.SerializeObject(emailPayLoad);

                StringContent content = new StringContent(payLoad, Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(path, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<EmailServiceResponse>(apiResponse);
                }
                bool status = false;
                if (result != null)
                {

                    status = result.statusCode == "200" ? true : false;

                }
                await _context.MessagingSystems.AddAsync(new MessagingSystem
                {
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                ,
                    IsActive = true,
                    Message = JsonConvert.SerializeObject(request),
                    MessageType = "",
                    Status = status
                });
                await _context.SaveChangesAsync();

                return result;
            }

        }
       
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
