using Buform.Example.Core;
using MvvmCross;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Buform.Example.MvvmCross.iOS
{
    [Preserve(AllMembers = true)]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public sealed class MenuViewController : MvxTableViewController<MenuViewModel>
    {
        private FormTableViewSource? _source;

        public MenuViewController() : base(UITableViewStyle.InsetGrouped)
        {
            /* Required constructor */
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            _source = new FormTableViewSource(TableView);

            var set = CreateBindingSet();
            set.Bind(this).For(v => v.Title).To(vm => vm.Title);
            set.Bind(_source).For(v => v.Form).To(vm => vm.Form);
            set.Apply();
        }
    }
}