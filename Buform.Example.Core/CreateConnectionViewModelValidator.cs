using FluentValidation;

namespace Buform.Example.Core
{
    public sealed class CreateConnectionViewModelValidator : AbstractValidator<CreateConnectionViewModel>
    {
        public CreateConnectionViewModelValidator()
        {
            RuleFor(item => item.Server).NotEmpty();
            RuleFor(item => item.Port).NotEmpty().GreaterThan(0);
            RuleFor(item => item.Password).NotEmpty();
        }
    }
}