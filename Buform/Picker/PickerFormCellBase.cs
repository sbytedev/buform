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
                TextColor = UIColor.SecondaryLabel,
                TextAlignment = UITextAlignment.Right
            };

            ContentView.AddSubviews(Label, ValueLabel);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                ValueLabel.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                ValueLabel.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                ValueLabel.LeadingAnchor.ConstraintEqualTo(Label.TrailingAnchor, 10),
                ValueLabel.TrailingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TrailingAnchor)
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