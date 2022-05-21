using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Buform
{
    public class AsyncPickerFormItem<TValue> : PickerFormItemBase<TValue>, IAsyncPickerFormItem
    {
        private Func<TValue?, string?>? _formatter;
        private Func<CancellationToken, Task<IEnumerable<TValue>>>? _sourceFactory;
        
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

        public virtual Func<CancellationToken, Task<IEnumerable<TValue>>>? SourceFactory
        {
            get => _sourceFactory;
            set
            {
                _sourceFactory = value;

                NotifyPropertyChanged();
            }
        }

        public virtual AsyncPickerLoadingState State { get; private set; }

        public override string? FormattedValue => Formatter?.Invoke(Value) ?? Value?.ToString();

        public AsyncPickerFormItem(Expression<Func<TValue>> targetProperty) : base(targetProperty)
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

        public virtual async Task LoadItemsAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (State != AsyncPickerLoadingState.None)
                {
                    return;
                }

                if (SourceFactory == null)
                {
                    State = AsyncPickerLoadingState.Loaded;
                    NotifyPropertyChanged(nameof(State));

                    return;
                }

                State = AsyncPickerLoadingState.Loading;
                NotifyPropertyChanged(nameof(State));

                var source = await SourceFactory(cancellationToken).ConfigureAwait(false);

                Options = source?.Select(CreateOption) ?? Array.Empty<IPickerOptionFormItem>();
                NotifyPropertyChanged(nameof(Options));

                State = AsyncPickerLoadingState.Loaded;
                NotifyPropertyChanged(nameof(State));
            }
            catch
            {
                State = AsyncPickerLoadingState.Failed;
                NotifyPropertyChanged(nameof(State));
            }
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
            }

            base.Dispose(isDisposing);
        }
    }

    public class AsyncPickerFormItem : AsyncPickerFormItem<string?>
    {
        public AsyncPickerFormItem(Expression<Func<string?>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }
    }
}