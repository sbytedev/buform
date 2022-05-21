using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Buform
{
    public class MultiValuePickerFormItem<TValue> : PickerFormItemBase<IEnumerable<TValue>?>, IMultiValuePickerFormItem
    {
        private Func<TValue?, string?>? _itemFormatter;
        private Func<IEnumerable<TValue>?, string?>? _valueFormatter;
        private IEnumerable<TValue>? _source;

        public override string? FormattedValue => ValueFormatter?.Invoke(Value) ?? Value?.ToString();

        public virtual Func<TValue?, string?>? ItemFormatter
        {
            get => _itemFormatter;
            set
            {
                _itemFormatter = value;

                foreach (var option in Options.OfType<PickerOptionFormItem<TValue>>())
                {
                    option.Formatter = ItemFormatter;
                }

                NotifyPropertyChanged(nameof(ItemFormatter));
            }
        }

        public virtual Func<IEnumerable<TValue>?, string?>? ValueFormatter
        {
            get => _valueFormatter;
            set
            {
                _valueFormatter = value;

                NotifyPropertyChanged(nameof(ValueFormatter));
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

        public MultiValuePickerFormItem(Expression<Func<IEnumerable<TValue>?>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }

        protected virtual IPickerOptionFormItem CreateOption(TValue value)
        {
            return new PickerOptionFormItem<TValue>(value)
            {
                Formatter = ItemFormatter
            };
        }

        public override void Pick(IPickerOptionFormItem? option)
        {
            var value = option?.Value is TValue itemValue ? itemValue : default;

            if (Value == default)
            {
                if (value != null)
                {
                    Value = new[] { value };
                }
            }
            else if (value != null)
            {
                Value = Value.Contains(value)
                    ? Value.Where(i => !Equals(i, value)).ToArray()
                    : Value.Concat(new[] { value! }).ToArray();
            }
        }

        public override bool IsPicked(IPickerOptionFormItem option)
        {
            return Value != default && Value.Contains(option.Value is TValue value ? value : default);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _itemFormatter = null;
                _valueFormatter = null;
                _source = null;
            }

            base.Dispose(isDisposing);
        }
    }

    public class MultiValuePickerFormItem : MultiValuePickerFormItem<string>
    {
        public MultiValuePickerFormItem(Expression<Func<IEnumerable<string>?>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }
    }
}