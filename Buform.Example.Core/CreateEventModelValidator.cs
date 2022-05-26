using FluentValidation;

namespace Buform.Example.Core
{
    public sealed class CreateEventModelValidator : AbstractValidator<CreateEventModel>
    {
        public CreateEventModelValidator()
        {
            RuleFor(item => item.Title).NotEmpty();
        }
    }
}