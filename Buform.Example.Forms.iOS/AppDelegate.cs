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
    }
}