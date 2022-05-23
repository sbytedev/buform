using System;
using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(CallbackPickerFormCell))]
    public class CallbackPickerFormCell : PickerFormCellBase<ICallbackPickerFormItem>
    {
        public override bool IsSelectable => !Item?.IsReadOnly ?? false;

        public CallbackPickerFormCell()
        {
            /* Required constructor */
        }

        public CallbackPickerFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateLabel(Item?.Label);
            UpdateValue(Item?.FormattedValue);
            UpdateValidationErrorMessage(Item?.ValidationErrorMessage);
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Item.IsReadOnly):
                    UpdateReadOnlyState();
                    break;
                case nameof(Item.Label):
                    UpdateLabel(Item?.Label);
                    break;
                case nameof(Item.Value):
                    UpdateValue(Item?.FormattedValue);
                    break;
                case nameof(Item.ValidationErrorMessage):
                    UpdateValidationErrorMessage(Item?.ValidationErrorMessage);
                    break;
            }
        }

        public override void OnSelected()
        {
            base.OnSelected();

            Item?.ExecuteCallback();
        }
    }
}