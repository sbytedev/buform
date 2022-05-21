using System;
using System.Linq.Expressions;

namespace Buform
{
    public class SliderFormItem : FormItem<float>
    {
        private float _minValue;
        private float _maxValue;

        public virtual float MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;

                NotifyPropertyChanged();
            }
        }

        public virtual float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;

                NotifyPropertyChanged();
            }
        }

        public SliderFormItem(Expression<Func<float>> targetProperty) : base(targetProperty)
        {
            _minValue = float.MinValue;
            _maxValue = float.MaxValue;
        }
    }
}