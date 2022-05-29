using System;
using System.Collections.Generic;
using System.Linq;

namespace Buform
{
    public abstract class FormGroup<TFormItem> : FormCollection<TFormItem>, IFormGroup where TFormItem : IFormItem
    {
        private readonly IDictionary<IFormItem, int> _hiddenItems = new Dictionary<IFormItem, int>();

        public IEnumerable<IFormItem> HiddenItems => _hiddenItems.Keys;

        IEnumerator<IFormItem> IEnumerable<IFormItem>.GetEnumerator()
        {
            return (IEnumerator<IFormItem>)base.GetEnumerator();
        }
        
        protected virtual void OnItemValueChanged(object sender, FormValueChangedEventArgs e)
        {
            NotifyValueChanged(e);
        }

        protected virtual void OnItemVisibilityChanged(object sender, EventArgs e)
        {
            if (sender is not TFormItem item)
            {
                return;
            }

            if (item.IsVisible)
            {
                if (!_hiddenItems.TryGetValue(item, out var index))
                {
                    return;
                }

                _hiddenItems.Remove(item);

                base.InsertItem(index, item);
            }
            else
            {
                var index = IndexOf(item);

                if (index < 0)
                {
                    return;
                }

                _hiddenItems[item] = index;

                base.RemoveItem(index);
            }
        }
        
        protected virtual void ShiftHiddenItems(int index, int shift)
        {
            foreach (var hiddenItem in _hiddenItems)
            {
                if (hiddenItem.Value > index)
                {
                    _hiddenItems[hiddenItem.Key] = hiddenItem.Value + shift;
                }
            }
        }

        protected override void InsertItem(int index, TFormItem item)
        {
            ShiftHiddenItems(index, 1);

            if (item.IsVisible)
            {
                base.InsertItem(index, item);
            }
            else
            {
                _hiddenItems[item] = index;
            }

            item.ValueChanged += OnItemValueChanged;
            item.VisibilityChanged += OnItemVisibilityChanged;
        }

        protected override void SetItem(int index, TFormItem item)
        {
            var existingItem = this[index];

            existingItem.ValueChanged += OnItemValueChanged;
            existingItem.VisibilityChanged += OnItemVisibilityChanged;
            
            base.SetItem(index, item);

            item.ValueChanged += OnItemValueChanged;
            item.VisibilityChanged += OnItemVisibilityChanged;
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];

            item.ValueChanged -= OnItemValueChanged;
            item.VisibilityChanged -= OnItemVisibilityChanged;

            ShiftHiddenItems(index, -1);

            if (item.IsVisible)
            {
                base.RemoveItem(index);
            }
            else
            {
                _hiddenItems.Remove(item);
            }
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                item.ValueChanged -= OnItemValueChanged;
                item.VisibilityChanged -= OnItemVisibilityChanged;
                item.Dispose();
            }

            foreach (var item in HiddenItems)
            {
                item.ValueChanged -= OnItemValueChanged;
                item.VisibilityChanged -= OnItemVisibilityChanged;
                item.Dispose();
            }

            _hiddenItems.Clear();

            base.ClearItems();
        }

        public virtual IFormItem? GetItem(string propertyName)
        {
            return this.FirstOrDefault<IFormItem>(item => item.PropertyName == propertyName) 
                   ?? HiddenItems.FirstOrDefault(item => item.PropertyName == propertyName);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var item in this)
                {
                    item.ValueChanged -= OnItemValueChanged;
                    item.VisibilityChanged -= OnItemVisibilityChanged;
                    item.Dispose();
                }

                foreach (var item in HiddenItems)
                {
                    item.ValueChanged -= OnItemValueChanged;
                    item.VisibilityChanged -= OnItemVisibilityChanged;
                    item.Dispose();
                }
            }

            base.Dispose(isDisposing);
        }
    }
}