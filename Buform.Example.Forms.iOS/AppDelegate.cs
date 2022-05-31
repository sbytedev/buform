using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;
using UIKit;

namespace Buform.Example.Forms.iOS
{
    [Preserve(AllMembers = true)]
    [Register(nameof(AppDelegate))]
    public sealed class AppDelegate : MvxFormsApplicationDelegate<Setup, Core.Application, Application>
    {
        private static void Main(string[] args)
        {
            UIApplication.Main(args, null, typeof(AppDelegate));
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var color = UIColor.FromName("Main");

            UIBarButtonItem.Appearance.TintColor = color;
            UIButton.Appearance.TintColor = color;
            UIDatePicker.Appearance.TintColor = color;
            UISlider.Appearance.TintColor = color;
            UITextField.Appearance.TintColor = color;
            UITextView.Appearance.TintColor = color;

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}