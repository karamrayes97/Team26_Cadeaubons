using Stripe;
using Stripe.Checkout;

namespace Cadeaubons_API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					policy => policy.AllowAnyOrigin()
								  .AllowAnyHeader()
								  .AllowAnyMethod());
			});

			var app = builder.Build();

			app.UseHttpsRedirection();
			app.UseCors("AllowAll");

			StripeConfiguration.ApiKey = "sk_test_MgvkTWK1jRG3olSRx9B7Mmxo";

			app.MapPost("/create-checkout-session", async () =>
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
								UnitAmount = 1000,
								ProductData = new SessionLineItemPriceDataProductDataOptions
								{
									Name = "Test Product"
								}
							}
						}
					},
					Mode = "payment",
					SuccessUrl = "https://example.com/success",
					CancelUrl = "https://example.com/cancel"
				};

				var service = new SessionService();
				var session = await service.CreateAsync(options);

				return Results.Ok(new { url = session.Url });
			});

			app.MapPost("/stripe-webhook", async (HttpRequest request) =>
			{
				var json = await new StreamReader(request.Body).ReadToEndAsync();

				var stripeEvent = EventUtility.ParseEvent(json);

				if (stripeEvent.Type == "checkout.session.completed")
				{
					var session = stripeEvent.Data.Object as Session;

					Console.WriteLine("PAYMENT SUCCESS:");
					Console.WriteLine(session.Id);

					// ?? TEST: this is where voucher would be created
				}

				return Results.Ok();
			});

			app.Run();


		}
	}

}