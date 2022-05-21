using System;
using System.Threading.Tasks;
using UIKit;

namespace Buform
{
    public class DialogPickerPresenter<TItem> : PickerPresenterBase<TItem> where TItem : class, IPickerFormItemBase
    {
        protected Func<TItem, UIViewController> ViewControllerFactory { get; }

        public DialogPickerPresenter(Func<TItem, UIViewController> viewControllerFactory)
        {
            ViewControllerFactory = viewControllerFactory;
        }

        public override Task PickAsync(UIView sourceView, TItem item)
        {
            var viewController = GetViewController();

            if (viewController == null)
            {
                return Task.CompletedTask;
            }

            var pickerViewController = ViewControllerFactory(item);

            var navigationController = new UINavigationController(pickerViewController);

            navigationController.ModalPresentationStyle = UIModalPresentationStyle.Popover;

            navigationController.PopoverPresentationController.SourceView = sourceView;
            navigationController.PopoverPresentationController.SourceRect = sourceView.Bounds;

            return viewController.PresentViewControllerAsync(navigationController, true);
        }
    }
}