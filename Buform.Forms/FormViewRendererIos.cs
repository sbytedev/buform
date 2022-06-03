using System.ComponentModel;
using Buform;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FormView), typeof(FormViewRenderer))]

namespace Buform
{
    public class FormViewRenderer : ViewRenderer<FormView, UITableView>
    {
        private void UpdateSourceForm()
        {
            if (Control.Source is not FormsFormTableViewSource source)
            {
                return;
            }

            if (Element == null)
            {
                return;
            }

            source.Form = Element.Form;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FormView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            var tableView = CreateNativeControl();

            SetNativeControl(tableView);

            UpdateSourceForm();
        }

        protected override UITableView CreateNativeControl()
        {
            var tableView = new UITableView(CGRect.Empty, UITableViewStyle.InsetGrouped);

            var source = new FormsFormTableViewSource(tableView);

            tableView.Source = source;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            return tableView;
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