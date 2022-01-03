using DigitalBanking.ServiceExtensions;
using DigitalBanking.Services.Contracts;
using DTO.Requests;
using DTO.Response;
using Services.BusinessLogic;

namespace DigitalBanking.Modules
{
    public class CardModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("v1/Cards", getCards);
        }

        private async Task<IResult> getCards(string customerId)
        {
            try
            {
                Card card = new Card();
                var request = new CardRequest { CustomerID = customerId };   
                var response = card.GetCard(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new Error
                {
                    Message = "Failed to process request.",
                    ExceptionDetails = ex
                });
            }
        }

    }
}
