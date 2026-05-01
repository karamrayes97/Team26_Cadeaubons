using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;
using Stripe;
using Stripe.Checkout;
using System.Runtime.CompilerServices;

namespace Cadeaubons_API
{

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddScoped<UserService>();
			builder.Services.AddScoped<CityService>();
			builder.Services.AddScoped<StoreService>();
			builder.Services.AddScoped<ThemeService>();
			builder.Services.AddScoped<Repository>();
			builder.Services.AddScoped<VoucherService>();
			builder.Services.AddScoped<PaymentService>();


			builder.Services.AddScoped<DomainManager>();

			VoucherDTO VoucherDTOStripe = new VoucherDTO();


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

			StripeConfiguration.ApiKey = "sk_test_51TP5XM5UFaYg1ZE3pevQkkl2NQOEWM3czwsmtgJsPM4goTmA84KiPafdZ2XjkfVRGqAVK9gCKDLDZ48gENWjGMz4000xqxC6r3";

			app.MapPost("/create-checkout-session", async (VoucherDTO dTO) =>
			{
				VoucherDTOStripe = dTO;
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
								UnitAmount = (long)(dTO.InitialAmount * 100),
								ProductData = new SessionLineItemPriceDataProductDataOptions
								{
									Name = "Test Product"
								}
							}
						}
					},
					Mode = "payment",
					SuccessUrl = "https://localhost:7011/success",
					CancelUrl = "https://localhost:7011/cancel"
				};

				var service = new SessionService();
				var session = await service.CreateAsync(options);

				return Results.Ok(new { url = session.Url });
			});

			app.MapPost("/stripe-webhook", async (HttpRequest request, DomainManager _domainManager) =>
			{
				var json = await new StreamReader(request.Body).ReadToEndAsync();

				//var stripeEvent = EventUtility.ParseEvent(json);

				var endpointSecret = "whsec_Oe7HhEu1jVL3viLHQNF83P7POGX5Njhr";

				var stripeEvent = EventUtility.ConstructEvent(
					json,
					request.Headers["Stripe-Signature"],
					endpointSecret
				);

				if (stripeEvent.Type == "checkout.session.completed")
				{
					var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

					Console.WriteLine("PAYMENT SUCCESS:");
					Console.WriteLine(session.Id);



					try
					{
						Voucher voucher = _domainManager.AddVoucher(VoucherDTOStripe);

						PaymentDTO paymentDTO = new PaymentDTO();

						paymentDTO.StripeId = session.Id;
						paymentDTO.Amount = VoucherDTOStripe.InitialAmount;
						paymentDTO.PaymentSuccess = true;
						paymentDTO.Date = DateTime.Now;
						paymentDTO.VoucherId = voucher.Id;

						_domainManager.AddPayment(paymentDTO);


					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						return Results.Problem(ex.Message);
					}
				}

				return Results.Ok();
			});

			app.MapGet("/success", () =>
			{
				return Results.Content(" Payment completed successfully!", "text/html");
			});

			app.MapGet("/cancel", () =>
			{
				return Results.Content(" Payment Canceled!", "text/html");
			});


			app.Run();


		}
	}

}