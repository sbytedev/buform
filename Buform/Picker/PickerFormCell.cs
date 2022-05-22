using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(PickerFormCell))]
    public class PickerFormCell : PickerFormCellBase<IPickerFormItem>
    {
        protected virtual PickerPresenterBase<IPickerFormItem>? PickerPresenter { get; private set; }

        public override bool IsSelectable => !Item?.IsReadOnly ?? false;

        public PickerFormCell()
        {
            /* Required constructor */
        }

        public PickerFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected virtual UIViewController CreateViewController(IPickerFormItem item)
        {
            return new PickerViewController<IPickerFormItem>(UITableViewStyle.InsetGrouped, item);
        }

        protected virtual UIAlertController CreateAlertController(IPickerFormItem item)
        {
            var alertController = UIAlertController.Create(
                item.Label,
                item.Message,
                UIAlertControllerStyle.ActionSheet
            );

            foreach (var listItem in item.Options)
            {
                var alertAction = UIAlertAction.Create(
                    listItem.FormattedValue ?? string.Empty,
                    UIAlertActionStyle.Default,
                    _ => item.Pick(listItem)
                );

                alertController.AddAction(alertAction);
            }

            if (item.CanBeCleared)
            {
                var clearAlertAction = UIAlertAction.Create(
                    Buform.Texts.Clear,
                    UIAlertActionStyle.Destructive,
                    _ => item.Pick(default)
                );

                alertController.AddAction(clearAlertAction);
            }

            var cancelAlertAction = UIAlertAction.Create(
                Buform.Texts.Cancel,
                UIAlertActionStyle.Cancel,
                null
            );

            alertController.AddAction(cancelAlertAction);

            return alertController;
        }

        protected virtual void UpdateInputType()
        {
            if (Item == null)
            {
                return;
            }

            PickerPresenter?.Dispose();

            switch (Item.InputType)
            {
                case PickerInputType.Default:
                    PickerPresenter = new DefaultPickerPresenter<IPickerFormItem>(CreateViewController);
                    Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    ValueLabelTrailingConstraint!.Constant = -8;
                    break;
                case PickerInputType.Dialog:
                    PickerPresenter = new DialogPickerPresenter<IPickerFormItem>(CreateViewController);
                    Accessory = UITableViewCellAccessory.None;
                    ValueLabelTrailingConstraint!.Constant = -ContentView.LayoutMargins.Right;
                    break;
                case PickerInputType.PopUp:
                    PickerPresenter = new PopUpPickerPresenter<IPickerFormItem>(CreateAlertController);
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