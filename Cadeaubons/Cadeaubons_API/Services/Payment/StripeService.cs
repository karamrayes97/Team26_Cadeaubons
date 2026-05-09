using Cadeaubons_Domain.DTO;
using Stripe.Checkout;

namespace Cadeaubons_API.Services.Payment
{
    public class StripeService
    {
        public Session CreateCheckout(VoucherDTO dto)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        UnitAmount = (long)(dto.InitialAmount * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Cadeaubon"
                        }
                    }
                }
            },
                Mode = "payment",
                SuccessUrl = "https://localhost:7011/success",
                CancelUrl = "https://localhost:7011/cancel",
                
                Metadata = new Dictionary<string, string>
                {
                    ["PurchaseDate"] = dto.PurchaseDate.ToString("o"),

                    ["InitialAmount"] = dto.InitialAmount.ToString(System.Globalization.CultureInfo.InvariantCulture),

                    ["BuyerId"] = dto.BuyerId.ToString(),
                    ["BuyerFullName"] = dto.BuyerFullName,

                    ["UserId"] = dto.UserId.ToString(),
                    ["UserEmail"] = dto.UserEmail,
                    ["UserFullName"] = dto.UserFullName,

                    ["ThemeId"] = dto.ThemeId.ToString(),
                    ["ThemeName"] = dto.ThemeName
                }
            };

            var service = new SessionService();
            return service.Create(options);
        }
    }
}
