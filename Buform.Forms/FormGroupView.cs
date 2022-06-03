using Xamarin.Forms;

namespace Buform
{
    public class FormGroupView : ContentView
    {
    }

    public class FormGroupView<TGroup> : FormGroupView
        where TGroup : class, IFormGroup
    {
        protected TGroup? Group => BindingContext as TGroup;
    }
}