using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace Buform
{
    public sealed class FluentValidationForm<TTarget> : ValidatableForm
    {
        private readonly TTarget _target;
        private readonly IValidator<TTarget> _validator;

        public FluentValidationForm(TTarget target, IValidator<TTarget> validator) : base(target!)
        {
            _target = target;
            _validator = validator;
        }

        private static FormValidationError GetValidationError(
            string propertyName,
            IEnumerable<ValidationFailure> failures
        )
        {
            return new FormValidationError(
                propertyName,
                string.Join(Environment.NewLine, failures.Select(failure => failure.ErrorMessage))
            );
        }

        private static FormValidationError[] GetValidationErrors(ValidationResult result)
        {
            return result.Errors
                .GroupBy(item => item.PropertyName)
                .Select(item => GetValidationError(item.Key, item.ToArray()))
                .ToArray();
        }

        public override bool PerformValidation()
        {
            var result = _validator.Validate(_target);

            IsValid = result.IsValid;

            SetValidationErrors(false, GetValidationErrors(result));

            return IsValid;
        }

        public override bool PerformForcedValidation()
        {
            var result = _validator.Validate(_target);

            IsValid = result.IsValid;

            SetValidationErrors(true, GetValidationErrors(result));

            return IsValid;
        }

        public override void ResetValidation()
        {
            var result = _validator.Validate(_target);

            IsValid = result.IsValid;

            ResetValidationErrors();
        }
    }
}