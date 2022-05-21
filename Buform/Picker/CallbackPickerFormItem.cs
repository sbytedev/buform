using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Buform
{
    public class CallbackPickerFormItem<TValue> : ValidatableFormItem<TValue>, ICallbackPickerFormItem
    {
        private string? _label;
        private Func<Task<TValue>>? _callback;
        private Func<TValue?, string?>? _formatter;

        public virtual string? Label
        {
            get => _label;
            set
            {
                _label = value;

                NotifyPropertyChanged();
            }
        }

        public virtual Func<Task<TValue>>? Callback
        {
            get => _callback;
            set
            {
                _callback = value;

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

        public CallbackPickerFormItem(Expression<Func<TValue>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }

        public virtual async void ExecuteCallback()
        {
            if (_callback == null)
            {
                return;
            }

            Value = await _callback.Invoke().ConfigureAwait(true);
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();

            NotifyPropertyChanged(nameof(FormattedValue));
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _formatter = null;
                _callback = null;
            }

            base.Dispose(isDisposing);
        }
    }

    public class CallbackPickerFormItem : CallbackPickerFormItem<string?>
    {
        public CallbackPickerFormItem(Expression<Func<string?>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }
    }
}