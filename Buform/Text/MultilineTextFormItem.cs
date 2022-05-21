using System;
using System.Linq.Expressions;

namespace Buform
{
    public class MultilineTextFormItem<TValue> : ValidatableFormItem<TValue>, IMultilineTextFormItem
    {
        private readonly Func<string?, TValue?> _converter;

        private TextInputType _inputType;
        private Func<TValue?, string?>? _formatter;

        public virtual TextInputType InputType
        {
            get => _inputType;
            set
            {
                _inputType = value;

                NotifyPropertyChanged();
            }
        }

        public virtual Func<TValue?, string?>? Formatter
        {
            get => _formatter;
            set
            {
                _formatter = value;

                NotifyPropertyChanged(nameof(Formatter));
                NotifyPropertyChanged(nameof(FormattedValue));
            }
        }

        public virtual string? FormattedValue => _formatter?.Invoke(Value) ?? Value?.ToString();

        public MultilineTextFormItem(Expression<Func<TValue>> targetProperty, Func<string?, TValue?> converter) 
            : base(targetProperty)
        {
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();

            NotifyPropertyChanged(nameof(FormattedValue));
        }

        public virtual void SetValue(string? value)
        {
            Value = _converter.Invoke(value);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _formatter = null;
            }

            base.Dispose(isDisposing);
        }
    }

    public class MultilineTextFormItem : MultilineTextFormItem<string?>
    {
        public MultilineTextFormItem(Expression<Func<string?>> targetProperty) 
            : base(targetProperty, item => item)
        {
            /* Required constructor */
        }
    }
}