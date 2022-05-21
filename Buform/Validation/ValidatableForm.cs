using System.Linq;

namespace Buform
{
    public abstract class ValidatableForm : Form
    {
        private bool _isValid;

        public virtual bool IsValid
        {
            get => _isValid;
            protected set
            {
                _isValid = value;

                NotifyPropertyChanged();
            }
        }

        protected ValidatableForm(object target) : base(target)
        {
            _isValid = true;
        }

        protected virtual void SetValidationErrors(bool isForced, params FormValidationError[] validationErrors)
        {
            var items = this
                .SelectMany(item => item)
                .OfType<IValidatableFormItem>()
                .ToArray();

            foreach (var item in items)
            {
                var validationError = validationErrors.FirstOrDefault(
                    i => i.TargetPropertyName == item.PropertyName
                );

                item.SetValidationError(validationError, isForced);
            }
        }

        protected virtual void ResetValidationErrors()
        {
            var items = this
                .SelectMany(item => item)
                .OfType<IValidatableFormItem>()
                .ToArray();

            foreach (var item in items)
            {
                item.ResetValidationError();
            }
        }

        protected override void OnItemValueChanged(object sender, FormValueChangedEventArgs e)
        {
            base.OnItemValueChanged(sender, e);

            PerformValidation();
        }

        public abstract bool PerformValidation();

        public abstract bool PerformForcedValidation();

        public abstract void ResetValidation();
    }
}