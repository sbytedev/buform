using System.ComponentModel;
using Buform;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FormView), typeof(FormViewRenderer))]

namespace Buform
{
    public class FormViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Source = new FormTableViewSource(Control);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName != FormView.FormProperty.PropertyName)
            {
                return;
            }

            if (Control.Source is not FormTableViewSource source)
            {
                return;
            }

            if (Element is FormView formView)
            {
                source.Form = formView.Form;
            }
        }
    }
}