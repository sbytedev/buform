using Xamarin.Forms;

namespace Buform
{
    public class HeaderFooterView : ContentView
    {
    }

    public class HeaderFooterView<TGroup> : HeaderFooterView
        where TGroup : class, IFormGroup
    {
        protected TGroup? Group => BindingContext as TGroup;
    }
}