using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Buform
{
    public class Form : FormCollection<IFormGroup>
    {
        private bool _isReadOnly;

        public virtual IFormItem? this[Expression<Func<object?>> targetProperty] => GetItem<IFormItem>(targetProperty);

        protected virtual object? Target { get; private set; }

        public virtual bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                if (_isReadOnly == value)
                {
                    return;
                }

                _isReadOnly = value;

                NotifyPropertyChanged();

                UpdateItems(this.SelectMany(item => item));
            }
        }

        public Form(object target)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        protected virtual void OnItemValueChanged(object sender, FormValueChangedEventArgs e)
        {
            NotifyValueChanged(e);
        }

        protected virtual void UpdateItems(IEnumerable<IFormItem> items)
        {
            foreach (var item in items)
            {
                item.IsReadOnly = IsReadOnly;
            }
        }

        protected virtual void InitializeItems(IEnumerable<IFormItem> items)
        {
            if (Target == null)
            {
                return;
            }

            foreach (var item in items)
            {
                item.Initialize(this, Target);
            }
        }

        protected override void InsertItem(int index, IFormGroup item)
        {
            base.InsertItem(index, item);

            item.ValueChanged += OnItemValueChanged;

            InitializeItems(item);
            UpdateItems(item);
        }

        protected override void SetItem(int index, IFormGroup item)
        {
            base.SetItem(index, item);

            item.ValueChanged += OnItemValueChanged;

            InitializeItems(item);
            UpdateItems(item);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];

            item.ValueChanged -= OnItemValueChanged;

            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                item.ValueChanged -= OnItemValueChanged;
            }

            base.ClearItems();
        }

        public virtual TItem? GetItem<TItem>(Expression<Func<object?>> targetProperty) where TItem : class, IFormItem
        {
            var targetPropertyName = targetProperty.GetMemberName();

            return GetItem<TItem>(targetPropertyName);
        }

        public virtual TItem? GetItem<TItem>(string targetPropertyName) where TItem : class, IFormItem
        {
            var item = this.SelectMany(item => item)
                .FirstOrDefault(item => item.PropertyName == targetPropertyName);

            return item as TItem;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var group in this)
                {
                    foreach (var item in group)
                    {
                        item.ValueChanged -= OnItemValueChanged;
                        item.Dispose();
                    }

                    group.Dispose();
                }

                Target = null;
            }

            base.Dispose(isDisposing);
        }
    }
}