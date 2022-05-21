using System.Threading.Tasks;
using Foundation;
using SByteDev.Xamarin.iOS.Extensions;
using UIKit;

namespace Buform
{
    public abstract class PickerPresenterBase<TItem> : NSObject where TItem : class, IPickerFormItemBase
    {
        protected static UIViewController? GetViewController()
        {
            return UIApplication.SharedApplication.GetTopViewController();
        }

        public abstract Task PickAsync(UIView sourceView, TItem item);
    }
}