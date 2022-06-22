using Xamarin.Forms;

namespace Buform
{
    public class FormsHeaderFooterView : ContentView
    {
    }

    public class FormsHeaderFooterView<TGroup> : FormsHeaderFooterView
        where TGroup : class, IFormGroup
    {
        protected TGroup? Group => BindingContext as TGroup;
    }
}