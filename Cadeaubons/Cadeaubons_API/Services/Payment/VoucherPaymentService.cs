using Cadeaubons_API.Services.Email;
using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Email;
using Stripe.Checkout;

namespace Cadeaubons_API.Services.Payment
{
    public class VoucherPaymentService
    {
        private readonly DomainManager _domainManager;
        private readonly IEmailService _emailService;

        public VoucherPaymentService(DomainManager domainManager, IEmailService emailService)
        {
            _domainManager = domainManager;
            _emailService = emailService;
        }

        public async Task HandleSuccess(Session session)
        {
            // 🔥 data uit Stripe halen
            var dto = new VoucherDTO
            {
                PurchaseDate = DateTime.Parse(session.Metadata["PurchaseDate"]),
                InitialAmount = decimal.Parse(
                    session.Metadata["InitialAmount"],
                    System.Globalization.CultureInfo.InvariantCulture
                    ),

                BuyerId = int.Parse(session.Metadata["BuyerId"]),
                BuyerFullName = session.Metadata["BuyerFullName"],

                UserId = int.Parse(session.Metadata["UserId"]),
                UserEmail = session.Metadata["UserEmail"],
                UserFullName = session.Metadata["UserFullName"],

                ThemeId = int.Parse(session.Metadata["ThemeId"]),
                ThemeName = session.Metadata["ThemeName"]
            };

            // 1. voucher maken
            var voucher = _domainManager.AddVoucher(dto);

            // 2. payment opslaan
            var payment = new PaymentDTO
            {
                StripeId = session.Id,
                Amount = dto.InitialAmount,
                PaymentSuccess = true,
                Date = DateTime.Now,
                VoucherId = voucher.Id
            };

            _domainManager.AddPayment(payment);

            // 3. email maken
            string body = EmailTemplateService.GetVoucherGiftEmailBody(dto, voucher.Number);

            // 4. email versturen
            await _emailService.SendAsync(
                dto.UserEmail,
                "🎁 Je hebt een cadeaubon ontvangen!",
                body
            );
        }
    }
}
