using System;
using System.Windows.Input;
using SByteDev.Common.Extensions;

namespace Buform
{
    public class ButtonFormItem : FormItem<ICommand>
    {
        private string? _label;
        private ButtonInputType _inputType;

        public override bool IsReadOnly
        {
            get => base.IsReadOnly || !Value.SafeCanExecute();
            set => base.IsReadOnly = value;
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

        public virtual ButtonInputType InputType
        {
            get => _inputType;
            set
            {
                _inputType = value;

                NotifyPropertyChanged();
            }
        }

        public ButtonFormItem(ICommand value) : base(value)
        {
            if (Value == null)
            {
                return;
            }

            Value.CanExecuteChanged += OnCanExecuteChanged;
        }

        protected virtual void OnCanExecuteChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(IsReadOnly));
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (Value != null)
                {
                    Value.CanExecuteChanged -= OnCanExecuteChanged;
                }
            }

            base.Dispose(isDisposing);
        }
    }
}