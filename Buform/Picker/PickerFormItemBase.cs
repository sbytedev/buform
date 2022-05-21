using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Buform
{
    public abstract class PickerFormItemBase<TValue> : ValidatableFormItem<TValue>, IPickerFormItemBase
    {
        private string? _label;
        private string? _message;
        private bool _canBeCleared;
        private PickerInputType _inputType;

        public virtual string? Label
        {
            get => _label;
            set
            {
                _label = value;

                NotifyPropertyChanged();
            }
        }

        public virtual string? Message
        {
            get => _message;
            set
            {
                _message = value;

                NotifyPropertyChanged();
            }
        }

        public virtual bool CanBeCleared
        {
            get => _canBeCleared;
            set
            {
                _canBeCleared = value;

                NotifyPropertyChanged();
            }
        }

        public virtual PickerInputType InputType
        {
            get => _inputType;
            set
            {
                _inputType = value;

                NotifyPropertyChanged();
            }
        }

        public abstract string? FormattedValue { get; }

        public IEnumerable<IPickerOptionFormItem> Options { get; protected set; }

        protected PickerFormItemBase(Expression<Func<TValue>> targetProperty) : base(targetProperty)
        {
            Options = Array.Empty<IPickerOptionFormItem>();
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();

            NotifyPropertyChanged(nameof(FormattedValue));
        }

        public abstract void Pick(IPickerOptionFormItem? option);

        public abstract bool IsPicked(IPickerOptionFormItem option);
    }
}