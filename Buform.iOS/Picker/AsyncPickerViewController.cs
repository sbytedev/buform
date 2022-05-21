using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    public class AsyncPickerViewController : PickerViewController<IAsyncPickerFormItem>
    {
        protected CancellationTokenSource? CancellationTokenSource { get; private set; }

        public AsyncPickerViewController(UITableViewStyle style, IAsyncPickerFormItem item) : base(style, item)
        {
            /* Required constructor */
        }

        protected virtual  async Task LoadItemsIfNeeded()
        {
            if (Item == null)
            {
                return;
            }

            if (Item.State == AsyncPickerLoadingState.Loaded)
            {
                return;
            }

            CancellationTokenSource?.Cancel();
            CancellationTokenSource = new CancellationTokenSource();

            var activityIndicator = new UIActivityIndicatorView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray,
                HidesWhenStopped = true
            };

            View!.AddSubview(activityIndicator);

            View.AddConstraints(new[]
            {
                activityIndicator.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
                activityIndicator.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor)
            });

            activityIndicator.StartAnimating();

            await Item.LoadItemsAsync(CancellationTokenSource.Token).ConfigureAwait(true);

            activityIndicator.StopAnimating();
            activityIndicator.RemoveFromSuperview();
            activityIndicator.Dispose();

            TableView.ReloadData();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            await LoadItemsIfNeeded().ConfigureAwait(false);
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            return Item?.State != AsyncPickerLoadingState.Loaded ? string.Empty : Item?.Message ?? string.Empty;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CancellationTokenSource?.Cancel();
            }

            base.Dispose(disposing);
        }
    }
}