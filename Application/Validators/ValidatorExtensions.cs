using FluentValidation;

namespace Application.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty().MinimumLength(6).WithMessage("Password needs to contain at least 6 characteres").Matches("[A-Z]").WithMessage("Password needs to contain at least one lower case letter").Matches("[0-9]").WithMessage("Password needs to contain at least one number").Matches("[^-a-zA-Z0-9]").WithMessage("Password needs to contain at least one symboles");

            return options;
        }
    }
}