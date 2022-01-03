using DigitalBanking.ServiceExtensions;
using DigitalBanking.Services.Contracts;

namespace DigitalBanking.Modules
{
    public class AccountModule : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("v1/accounts", getAccounts);
        }


        private async Task<IResult> getAccounts(HttpContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            return Results.Ok(
                new
                {
                    accountId = "234567890000",
                    accountTitle = "sana zehra"
                }
                );
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddSingleton<IAccountService, AccountService>();
        }
    }
}
