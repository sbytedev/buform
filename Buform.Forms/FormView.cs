using Xamarin.Forms;

namespace Buform
{
    public class FormView : View
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