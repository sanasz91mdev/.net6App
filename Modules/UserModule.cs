using AppNet6.DTO.Requests;
using AppNet6.DTO.Response;
using DataAccess;
using FluentValidation.Results;
using MiniValidation;
using DigitalBanking.ServiceExtensions;

namespace AppNet6.Modules
{
    public class UserModule : ICarterModule
    {
        DbManager dbManager = new DbManager();
        ILogger _logger;

        public UserModule(ILogger<UserModule> logger)
        {
            _logger = logger;
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/users", getUser);
            app.MapGet("/api/v2/users", getUsers)
                .Produces<List<UserResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Search").WithTags("v2Users");
            app.MapPost("/api/v1/users", createUser);

        }

        private async Task<IResult> getUser()
        {
            // logger.LogInformation("Get users v1 called.");

            _logger.LogInfo(nameof(UserModule), "Get users v1 called.");

            return Results.Ok(new
            {
                name = "sana",
                contact = "03323344553"
            });
        }

        private async Task<IResult> get(ILogger logger)
        {
            var user = "sana";
            var users = dbManager.executeQuery("SELECT * FROM IRIS_CONFIG.USERS");
            if(users != null)
            {
                return Results.Ok(new
                {
                    name = "sana",
                    contact = "03323344553"
                });
            }
            else
            {
                return Results.NotFound(new
                {
                    error = $"no details found for user {user}"
                });
            }

 
        }

        private async Task<IResult> getUsers()
        {
            return Results.Ok(new UserResponse
            {
                Name = "sana",
                contactNumber = "03323344553",
                emailAddress = "sana.zehra@gmail.com"
            });
        }

        private async Task<IResult> createUser(UserRequest user, ILogger<UserModule> logger)
        {;
            logger.LogInformation("Get User called.");
            //MiniValidator
            MiniValidator.TryValidate(user, out var errors);

            //FluentValidator
            var validator = new UserValidator();
            ValidationResult results = validator.Validate(user);
            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;

           return errors?.Count > 0 ? Results.BadRequest(new {error= errors.FirstOrDefault()}): Results.Created("/v1/user", user);
           //return failures?.Count > 0 ? Results.BadRequest(new { error = failures[0].ErrorMessage }) : Results.Created("/v1/user", user);

        }


    }
}
