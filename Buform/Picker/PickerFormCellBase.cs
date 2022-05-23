using System;
using UIKit;

namespace Buform
{
    public abstract class PickerFormCellBase<TItem> : FormCell<TItem> where TItem : class, IFormItem
    {
        protected virtual UILabel? Label { get; set; }
        protected virtual UILabel? ValueLabel { get; set; }

        protected PickerFormCellBase()
        {
            /* Required constructor */
        }

        protected PickerFormCellBase(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected override void Initialize()
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;

            Label = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.Label
            };

            ValueLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.SecondaryLabel
            };

            ValueLabel.SetContentCompressionResistancePriority((float)UILayoutPriority.Required, UILayoutConstraintAxis.Horizontal);

            ContentView.AddSubviews(Label, ValueLabel);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, ContentView.LayoutMargins.Left),
                ValueLabel.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                ValueLabel.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom),
                ValueLabel.LeadingAnchor.ConstraintEqualTo(Label.TrailingAnchor, 8),
                ValueLabel.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -8)
            });
        }

        protected virtual void UpdateReadOnlyState()
        {
            if (Label == null)
            {
                return;
            }

            SelectionStyle = Item?.IsReadOnly ?? true
                ? UITableViewCellSelectionStyle.None
                : UITableViewCellSelectionStyle.Default;
        }

        protected virtual void UpdateLabel(string? text)
        {
            if (Label == null)
            {
                return;
            }

            Label.Text = text;
        }

        protected virtual void UpdateValue(string? text)
        {
            if (ValueLabel == null)
            {
                return;
            }

            ValueLabel.Text = text;
        }

        protected virtual void UpdateValidationErrorMessage(string? validationErrorMessage)
        {
            if (Label == null)
            {
                return;
            }

            Label.TextColor = validationErrorMessage == null ? UIColor.Label : UIColor.SystemRed;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Label?.Dispose();
                Label = null;

                ValueLabel?.Dispose();
                ValueLabel = null;
            }

            base.Dispose(disposing);
        }
    }
}