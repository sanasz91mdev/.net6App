using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace AppNet6.DTO.Requests
{
    public class UserRequest
    {
        [Required, MinLength(3)]
        public string Name { get; set; }

        public string contactNumber { get; set; }
    }

    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.contactNumber).NotEmpty().WithMessage("Please specify a valid number");
        }

        private bool BeAValidPostcode(string postcode)
        {
            // custom postcode validating logic goes here
            return false;
        }


    }

    public static class MyCustomValidators
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count < num).WithMessage("The list contains too many items");
        }
    }
}
