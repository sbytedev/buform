using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Buform
{
    public class PickerFormItem<TValue> : PickerFormItemBase<TValue>, IPickerFormItem
    {
        private Func<TValue?, string?>? _formatter;
        private IEnumerable<TValue>? _source;

        public virtual Func<TValue?, string?>? Formatter
        {
            get => _formatter;
            set
            {
                _formatter = value;

                foreach (var option in Options.OfType<PickerOptionFormItem<TValue>>())
                {
                    option.Formatter = Formatter;
                }

                NotifyPropertyChanged(nameof(Formatter));
                NotifyPropertyChanged(nameof(FormattedValue));
            }
        }

        public virtual IEnumerable<TValue>? Source
        {
            get => _source;
            set
            {
                _source = value;

                Options = _source?.Select(CreateOption) ?? Array.Empty<IPickerOptionFormItem>();

                NotifyPropertyChanged(nameof(Source));
                NotifyPropertyChanged(nameof(Options));
            }
        }

        public override string? FormattedValue => Formatter?.Invoke(Value) ?? Value?.ToString();

        public PickerFormItem(Expression<Func<TValue>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }

        protected virtual IPickerOptionFormItem CreateOption(TValue value)
        {
            return new PickerOptionFormItem<TValue>(value)
            {
                Formatter = Formatter
            };
        }

        public override void Pick(IPickerOptionFormItem? item)
        {
            Value = (TValue?)item?.Value;
        }

        public override bool IsPicked(IPickerOptionFormItem item)
        {
            return Equals(Value, item.Value);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _formatter = null;
                _source = null;
            }

            base.Dispose(isDisposing);
        }
    }

    public class PickerFormItem : PickerFormItem<string?>
    {
        public PickerFormItem(Expression<Func<string?>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }
    }
}