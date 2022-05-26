using Buform.Example.Core;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Buform.Example.MvvmCross.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    [MvxModalPresentation(WrapInNavigationController = true, ModalPresentationStyle = UIModalPresentationStyle.FormSheet)]
    public sealed class CreateEventViewController : MvxTableViewController<CreateEventViewModel>
    {
        private FormTableViewSource? _source;

        public CreateEventViewController() : base(UITableViewStyle.InsetGrouped)
        {
            /* Required constructor */
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem("Add", UIBarButtonItemStyle.Done, null);

            TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            _source = new FormTableViewSource(TableView);

            var set = CreateBindingSet();
            set.Bind(this).For(v => v.Title).To(vm => vm.Title);
            set.Bind(NavigationItem.LeftBarButtonItem).To(vm => vm.CancelCommand);
            set.Bind(NavigationItem.RightBarButtonItem).To(vm => vm.CreateCommand);
            set.Bind(_source).For(v => v.Form).To(vm => vm.Form);
            set.Apply();
        }
    }
}