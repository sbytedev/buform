using System;
using System.Threading.Tasks;
using UIKit;

namespace Buform
{
    public class DefaultPickerPresenter<TItem> : PickerPresenterBase<TItem> where TItem : class, IPickerFormItemBase
    {
        protected Func<TItem, UIViewController> ViewControllerFactory { get; }

        public DefaultPickerPresenter(Func<TItem, UIViewController> viewControllerFactory)
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

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (viewController.NavigationController == null)
            {
                var navigationController = new UINavigationController(pickerViewController);

                return viewController.PresentViewControllerAsync(navigationController, true);
            }

            viewController.NavigationController.PushViewController(pickerViewController, true);

            return Task.CompletedTask;
        }
    }
}