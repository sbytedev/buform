using MvvmCross.ViewModels;

namespace Buform.Example.MvvmCross.Core
{
    public sealed class Application : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RegisterAppStart<MenuViewModel>();
        }
    }
}