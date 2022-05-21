using System;
using System.Linq.Expressions;

namespace Buform
{
    public class DateTimeFormItem : ValidatableFormItem<DateTime?>
    {
        private DateTime _minValue;
        private DateTime _maxValue;
        private string? _label;
        private DateTimeInputType _inputType;
        
        public virtual DateTime MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;

                NotifyPropertyChanged();
            }
        }

        public virtual DateTime MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;

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

        public virtual DateTimeInputType InputType
        {
            get => _inputType;
            set
            {
                _inputType = value;

                NotifyPropertyChanged();
            }
        }

        public DateTimeFormItem(Expression<Func<DateTime?>> targetProperty) : base(targetProperty)
        {
            _minValue = DateTime.MinValue;
            _maxValue = DateTime.MaxValue;
        }
    }
}