namespace NCSEvent.API.Commons.DTO
{
    public class PaymentSummaryDTO
    {
        public long RegistrationFormId { get; set; }
        public string Email { get; set; }
        public decimal EventAmount { get; set; }
        public decimal HotelAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
