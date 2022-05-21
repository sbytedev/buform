using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Buform
{
    public class ListFormGroup<TValue> : FormGroup<ListFormItem<TValue>>, IListFormGroup
    {
        private string? _headerLabel;
        private string? _footerLabel;
        private Func<TValue?, string?>? _formatter;
        private IEnumerable<TValue>? _source;

        public virtual string? HeaderLabel
        {
            get => _headerLabel;
            set
            {
                _headerLabel = value;

                NotifyPropertyChanged();
            }
        }

        public virtual string? FooterLabel
        {
            get => _footerLabel;
            set
            {
                _footerLabel = value;

                NotifyPropertyChanged();
            }
        }

        public virtual Func<TValue?, string?>? Formatter
        {
            get => _formatter;
            set
            {
                _formatter = value;

                foreach (var item in this)
                {
                    item.Formatter = Formatter;
                }

                NotifyPropertyChanged(nameof(Formatter));
            }
        }

        public virtual IEnumerable<TValue>? Source
        {
            get => _source;
            set
            {
                if (_source is INotifyCollectionChanged oldNotifyCollectionChanged)
                {
                    oldNotifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
                }

                _source = value ?? Array.Empty<TValue>();
                
                if (_source is INotifyCollectionChanged newNotifyCollectionChanged)
                {
                    newNotifyCollectionChanged.CollectionChanged += OnCollectionChanged;
                }

                Reset();

                NotifyPropertyChanged(nameof(Source));
            }
        }

        public ListFormGroup(string? headerLabel = null, string? footerLabel = null)
        {
            _headerLabel = headerLabel;
            _footerLabel = footerLabel;
        }

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Reset();

            NotifyPropertyChanged(nameof(Source));
        }

        protected virtual void Reset()
        {
            ClearItems();

            if (Source == null)
            {
                return;
            }

            foreach (var value in Source)
            {
                var item = new ListFormItem<TValue>(value)
                {
                    Formatter = Formatter
                };

                Add(item);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_source is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
                }
            }

            _formatter = null;
            _source = null;

            base.Dispose(isDisposing);
        }
    }
}