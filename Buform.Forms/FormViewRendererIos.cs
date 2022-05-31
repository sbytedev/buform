using System.ComponentModel;
using Buform;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FormView), typeof(FormViewRenderer))]

namespace Buform
{
    public class FormViewRenderer : ListViewRenderer
    {
        private void UpdateSourceForm()
        {
            if (Control.Source is not FormTableViewSource source)
            {
                return;
            }

            if (Element is not FormView formView)
            {
                return;
            }

            source.Form = formView.Form;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            Control.Source = new FormTableViewSource(Control);

            UpdateSourceForm();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName != FormView.FormProperty.PropertyName)
            {
                return;
            }

            UpdateSourceForm();
        }
    }
}