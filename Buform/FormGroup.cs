using System.Collections.Generic;

namespace Buform
{
    public abstract class FormGroup<TFormItem> : FormCollection<TFormItem>, IFormGroup where TFormItem : IFormItem
    {
        IEnumerator<IFormItem> IEnumerable<IFormItem>.GetEnumerator()
        {
            return (IEnumerator<IFormItem>)base.GetEnumerator();
        }
        
        protected virtual void OnItemValueChanged(object sender, FormValueChangedEventArgs e)
        {
            NotifyValueChanged(e);
        }

        protected override void InsertItem(int index, TFormItem item)
        {
            base.InsertItem(index, item);

            item.ValueChanged += OnItemValueChanged;
        }

        protected override void SetItem(int index, TFormItem item)
        {
            base.SetItem(index, item);

            item.ValueChanged += OnItemValueChanged;
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

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var item in this)
                {
                    item.ValueChanged -= OnItemValueChanged;
                    item.Dispose();
                }
            }

            base.Dispose(isDisposing);
        }
    }
}