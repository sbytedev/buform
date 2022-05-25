using System;

namespace Buform
{
    public class SegmentsOptionFormItem<TValue> : FormItem<TValue>, ISegmentsOptionFormItem
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

        public SegmentsOptionFormItem(TValue value) : base(value)
        {
            /* Required constructor */
        }
    }
}