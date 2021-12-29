using DigitalBanking.Services.Contracts;

namespace DigitalBanking.Modules
{
    public class PaymentModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("v1/payment", doPayment);
        }

        private async Task<IResult> doPayment(HttpContext context, IOTPService service)
        {
            var body = context.Request.Body;

            //......
            //......

            //validate OTP 1st ...
            bool isValid = service.ValidateOTP();
            if (isValid)
            {
                //do payment
                return Results.BadRequest(new { error = "Payment successful" });


            }
            else
                return Results.BadRequest(new { error = "OTP validation failed." });

        }
    }
}
