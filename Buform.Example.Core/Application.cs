using MvvmCross.ViewModels;

namespace Buform.Example.Core
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