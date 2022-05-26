using Buform.Example.Core;
using MvvmCross;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Buform.Example.MvvmCross.iOS
{
    [Preserve(AllMembers = true)]
    [MvxModalPresentation(WrapInNavigationController = true, ModalPresentationStyle = UIModalPresentationStyle.FormSheet)]
    public sealed class ComponentsViewController : MvxTableViewController<ComponentsViewModel>
    {
        private FormTableViewSource? _source;

        public ComponentsViewController() : base(UITableViewStyle.InsetGrouped)
        {
            /* Required constructor */
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem("Read-only", UIBarButtonItemStyle.Plain, null);

            TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            _source = new FormTableViewSource(TableView);

            var set = CreateBindingSet();
            set.Bind(this).For(v => v.Title).To(vm => vm.Title);
            set.Bind(NavigationItem.LeftBarButtonItem).To(vm => vm.CloseCommand);
            set.Bind(NavigationItem.RightBarButtonItem).To(vm => vm.ToggleReadOnlyModeCommand);
            set.Bind(_source).For(v => v.Form).To(vm => vm.Form);
            set.Apply();
        }
    }
}