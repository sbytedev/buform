using Buform.Example.MvvmCross.Core;
using Foundation;
using MvvmCross.Platforms.Ios.Core;
using UIKit;

namespace Buform.Example.MvvmCross.iOS
{
    [Preserve(AllMembers = true)]
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, Application>
    {
        private static void Main(string[] args)
        {
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}