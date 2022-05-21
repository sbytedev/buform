using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(MultilineTextFormCell))]
    public class MultilineTextFormCell : FormCell<IMultilineTextFormItem>
    {
        protected virtual UITextView? TextView { get; set; }

        public MultilineTextFormCell()
        {
            /* Required constructor */
        }

        public MultilineTextFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected override void Initialize()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            TextView = new UITextView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextContainerInset = UIEdgeInsets.Zero,
                DataDetectorTypes = UIDataDetectorType.All,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.Label
            };

            TextView.TextContainer.LineFragmentPadding = 0;

            TextView.Changed += OnChanged;

            ContentView.AddSubviews(TextView);

            var bottomConstraint = TextView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom);

            bottomConstraint.Priority = 999;

            ContentView.AddConstraints(new[]
            {
                TextView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                bottomConstraint,
                TextView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, ContentView.LayoutMargins.Left),
                TextView.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -ContentView.LayoutMargins.Right),
                TextView.HeightAnchor.ConstraintEqualTo(200)
            });
        }

        protected virtual void OnChanged(object sender, EventArgs e)
        {
            if (TextView == null)
            {
                return;
            }
            
            Item?.SetValue(TextView.Text);
        }

        protected virtual void UpdateReadOnlyState()
        {
            if (TextView == null)
            {
                return;
            }

            TextView.Editable = !Item?.IsReadOnly ?? true;
        }

        protected virtual void UpdateInputType()
        {
            if (TextView == null)
            {
                return;
            }

            TextView.KeyboardType = Item?.InputType switch
            {
                TextInputType.Default => UIKeyboardType.Default,
                TextInputType.NumberAndPunctuation => UIKeyboardType.NumbersAndPunctuation,
                TextInputType.Number => UIKeyboardType.NumberPad,
                TextInputType.Decimal => UIKeyboardType.DecimalPad,
                TextInputType.Phone => UIKeyboardType.PhonePad,
                TextInputType.Url => UIKeyboardType.Url,
                TextInputType.EmailAddress => UIKeyboardType.EmailAddress,
                null => UIKeyboardType.Default,
                _ => throw new ArgumentOutOfRangeException(nameof(Item.InputType),  Item?.InputType, null)
            };
        }

        protected virtual void UpdateValue()
        {
            if (TextView == null)
            {
                return;
            }

            TextView.Text = Item?.FormattedValue;
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateInputType();
            UpdateValue();
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Item.IsReadOnly):
                    UpdateReadOnlyState();
                    break;
                case nameof(Item.InputType):
                    UpdateInputType();
                    break;
                case nameof(Item.FormattedValue):
                    UpdateValue();
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var textView = TextView;
                if (textView != null)
                {
                    textView.Changed -= OnChanged;
                }

                TextView?.Dispose();
                TextView = null;
            }

            base.Dispose(disposing);
        }
    }
}