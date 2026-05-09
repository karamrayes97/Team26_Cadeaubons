using Cadeaubons_API.Options;
using Cadeaubons_API.Services.Email;
using Cadeaubons_API.Services.Payment;
using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Email;
using Stripe;
using Stripe.Checkout;

namespace Cadeaubons_API
{

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			
			builder.Services.Configure<MailtrapOptions>(
				builder.Configuration.GetSection("Mailtrap")
			);

            builder.Services.AddScoped<IEmailService, MailtrapEmailService>();
            builder.Services.AddScoped<StripeService>();
            builder.Services.AddScoped<VoucherPaymentService>();
			builder.Services.AddScoped<DomainManager>();


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

			StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            app.MapPost(
				"/create-checkout-session",
                (VoucherDTO dto, StripeService stripeService) =>
				{
					var session = stripeService.CreateCheckout(dto);

                    if (session is null)
                        return Results.Problem("Failed to create Stripe session");

                    return Results.Ok(new { url = session.Url });
				}
			);

            app.MapPost(
				"/stripe-webhook", 
				async (HttpRequest request, VoucherPaymentService paymentService) =>
			{
				var json = await new StreamReader(request.Body).ReadToEndAsync();

                Event stripeEvent;

                try
                {
                    stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        builder.Configuration["Stripe:WebhookSecret"]
                    );
                }
                catch
                {
                    return Results.BadRequest("Invalid Stripe signature");
                }

                if (stripeEvent.Type != EventTypes.CheckoutSessionCompleted)
                    return Results.Ok();

                var session = stripeEvent.Data.Object as Session;

                if (session is null)
                    return Results.Ok();

                try
                {
                    await paymentService.HandleSuccess(session);
                }
                catch 
                {
                    return Results.Ok();
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