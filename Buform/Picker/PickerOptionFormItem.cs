using System;

namespace Buform
{
    public class PickerOptionFormItem<TValue> : FormItem<TValue>, IPickerOptionFormItem
    {
        private Func<TValue?, string?>? _formatter;
        
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

        public PickerOptionFormItem(TValue value) : base(value)
        {
            /* Required constructor */
        }
    }
}