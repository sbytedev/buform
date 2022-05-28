using Xamarin.Forms;

namespace Buform
{
    public class FormView : ListView
    {
        public static readonly BindableProperty FormProperty = BindableProperty.Create(
            nameof(Form),
            typeof(Form),
            typeof(FormView)
        );

        public Form? Form
        {
            get => (Form?)GetValue(FormProperty);
            set => SetValue(FormProperty, value);
        }
    }
}