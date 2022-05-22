using System;
using System.Drawing;
using System.Linq.Expressions;

namespace Buform
{
    public class ColorPickerFormItem : ValidatableFormItem<Color>
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

        public ColorPickerFormItem(Expression<Func<Color>> targetProperty) : base(targetProperty)
        {
            /* Required constructor */
        }
    }
}