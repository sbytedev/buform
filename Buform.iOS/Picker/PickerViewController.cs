using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using SByteDev.Xamarin.iOS.Extensions;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    public class PickerViewController<TItem> : UITableViewController where TItem : class, IPickerFormItemBase
    {
        protected TItem? Item { get; private set; }

        public override CGSize PreferredContentSize
        {
            get => base.PreferredContentSize;
            set
            {
                base.PreferredContentSize = value;

                // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
                if (NavigationController?.PopoverPresentationController == null)
                {
                    return;
                }

                NavigationController.PreferredContentSize = PreferredContentSize;
            }
        }

        public PickerViewController(UITableViewStyle style, TItem item) : base(style)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));

            Item.PropertyChanged += OnPropertyChanged;
        }

        protected virtual void SetLeftBarButtonDoneItem()
        {
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(
                UIBarButtonSystemItem.Done,
                (_, _) => DismissViewController(true, null)
            );
        }

        protected virtual void SetRightBarButtonClearItem()
        {
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(
                Buform.Texts.Clear,
                UIBarButtonItemStyle.Plain,
                (_, _) =>
                {
                    if (Item != null)
                    {
                        Item.Value = null;
                    }
                }
            );
        }

        protected virtual async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100)).ConfigureAwait(false);

            this.InvokeOnMainThreadIfNeeded(() => TableView.ReloadData());
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = Item?.Label;
            
            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            if (NavigationController?.PopoverPresentationController != null)
            {
                SetLeftBarButtonDoneItem();
            }

            if (Item?.CanBeCleared ?? false)
            {
                SetRightBarButtonClearItem();
            }
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return Item?.Options.Count() ?? 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        [SuppressMessage("ReSharper", "NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract")]
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return TableView.DequeueReusableCell(nameof(UITableViewCell)) 
                   ?? new UITableViewCell(UITableViewCellStyle.Default, nameof(UITableViewCell));
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            var item = Item?.Options.ElementAt(indexPath.Row);

            if (item == null)
            {
                return;
            }

            if (Item == null)
            {
                return;
            }

            cell.TextLabel.Text = item.FormattedValue;

            cell.Accessory = Item.IsPicked(item)
                ? UITableViewCellAccessory.Checkmark
                : UITableViewCellAccessory.None;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = Item?.Options.ElementAt(indexPath.Row);

            if (item == null)
            {
                return;
            }

            Item?.Pick(item);
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            return Item?.Message ?? string.Empty;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            if (NavigationController?.PopoverPresentationController != null)
            {
                PreferredContentSize = TableView.ContentSize.Height < Buform.Picker.MinimumPopUpSize.Height 
                    ? Buform.Picker.MinimumPopUpSize 
                    : TableView.ContentSize;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var item = Item;
                if (item != null)
                {
                    item.PropertyChanged -= OnPropertyChanged;
                }

                Item = null;
            }

            base.Dispose(disposing);
        }
    }
}