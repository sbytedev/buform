using System;
using System.Linq.Expressions;

namespace Buform
{
    public class SwitchFormItem : ValidatableFormItem<bool>
    {
        private string? _label;

        public virtual string? Label
        {
            get => _label;
            set
            {
                _label = value;

                NotifyPropertyChanged();
            }
        }

        public SwitchFormItem(Expression<Func<bool>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }
    }
}