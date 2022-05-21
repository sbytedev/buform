using System;
using System.Linq.Expressions;

namespace Buform
{
    public class StepperFormItem : ValidatableFormItem<int>
    {
        private int _minValue;
        private int _maxValue;
        private int _stepAmount;
        private string? _label;

        public virtual int MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;

                NotifyPropertyChanged();
            }
        }

        public virtual int MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;

                NotifyPropertyChanged();
            }
        }

        public virtual int StepAmount
        {
            get => _stepAmount;
            set
            {
                _stepAmount = value;

                NotifyPropertyChanged();
            }
        }

        public virtual string? Label
        {
            get => _label;
            set
            {
                _label = value;

                NotifyPropertyChanged();
            }
        }

        public StepperFormItem(Expression<Func<int>> targetProperty) : base(targetProperty)
        {
            _minValue = int.MinValue;
            _maxValue = int.MaxValue;
            _stepAmount = 1;
        }
    }
}