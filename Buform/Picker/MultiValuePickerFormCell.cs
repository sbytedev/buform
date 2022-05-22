using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(MultiValuePickerFormCell))]
    public class MultiValuePickerFormCell : PickerFormCellBase<IMultiValuePickerFormItem>
    {
        protected virtual PickerPresenterBase<IMultiValuePickerFormItem>? PickerPresenter { get; private set; }

        public override bool IsSelectable => !Item?.IsReadOnly ?? false;

        public MultiValuePickerFormCell()
        {
            /* Required constructor */
        }

        public MultiValuePickerFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected virtual UIViewController CreateViewController(IMultiValuePickerFormItem item)
        {
            return new PickerViewController<IMultiValuePickerFormItem>(UITableViewStyle.InsetGrouped, item);
        }

        private void UpdateInputType()
        {
            if (Item == null)
            {
                return;
            }

            PickerPresenter?.Dispose();

            switch (Item.InputType)
            {
                case PickerInputType.Default:
                    PickerPresenter = new DefaultPickerPresenter<IMultiValuePickerFormItem>(CreateViewController);
                    Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    ValueLabelTrailingConstraint!.Constant = -8;
                    break;
                case PickerInputType.Dialog:
                    PickerPresenter = new DialogPickerPresenter<IMultiValuePickerFormItem>(CreateViewController);
                    Accessory = UITableViewCellAccessory.None;
                    ValueLabelTrailingConstraint!.Constant = -ContentView.LayoutMargins.Right;
                    break;
                case PickerInputType.PopUp:
                    PickerPresenter = new DefaultPickerPresenter<IMultiValuePickerFormItem>(CreateViewController);
                    Accessory = UITableViewCellAccessory.None;
                    ValueLabelTrailingConstraint!.Constant = -ContentView.LayoutMargins.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Item.InputType), Item.InputType, null);
            }
        }

        protected override void OnItemSet()
        {
            UpdateLabel(Item?.Label);
            UpdateInputType();
            UpdateValue(Item?.FormattedValue);
            UpdateValidationErrorMessage(Item?.ValidationErrorMessage);
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Item.Label):
                    UpdateLabel(Item?.Label);
                    break;
                case nameof(Item.InputType):
                    UpdateInputType();
                    break;
                case nameof(Item.FormattedValue):
                    UpdateValue(Item?.FormattedValue);
                    break;
                case nameof(Item.ValidationErrorMessage):
                    UpdateValidationErrorMessage(Item?.ValidationErrorMessage);
                    break;
            }
        }

        public override async void OnSelected()
        {
            base.OnSelected();

            if (Item == null)
            {
                return;
            }

            if (PickerPresenter == null)
            {
                return;
            }

            await PickerPresenter.PickAsync(this, Item).ConfigureAwait(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PickerPresenter?.Dispose();
                PickerPresenter = null;
            }

            base.Dispose(disposing);
        }
    }
}