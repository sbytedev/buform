using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(TextFormCell))]
    public class TextFormCell : FormCell<ITextFormItem>
    {
        private NSLayoutConstraint? _textFieldLeadingConstraint;
        private NSLayoutConstraint? _labelWidthConstraint;

        protected virtual UILabel? Label { get; set; }
        protected virtual UITextField? TextField { get; set; }

        public override bool IsSelectable => !Item?.IsReadOnly ?? false;

        public TextFormCell()
        {
            /* Required constructor */
        }

        public TextFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected override void Initialize()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            Label = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.Label
            };

            TextField = new UITextField
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextAlignment = UITextAlignment.Right,
                ClearButtonMode = UITextFieldViewMode.WhileEditing
            };

            TextField.EditingChanged += OnEditingChanged;

            ContentView.AddSubviews(Label, TextField);

            _textFieldLeadingConstraint = Label.TrailingAnchor.ConstraintEqualTo(TextField.LeadingAnchor, -10);

            _labelWidthConstraint = Label.WidthAnchor.ConstraintEqualTo(0);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                _labelWidthConstraint,
                TextField.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                TextField.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                _textFieldLeadingConstraint,
                TextField.TrailingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TrailingAnchor)
            });
        }

        private void OnEditingChanged(object sender, EventArgs e)
        {
            if (TextField == null)
            {
                return;
            }

            Item?.SetValue(TextField.Text);
        }

        private void UpdateReadOnlyState()
        {
            if (TextField == null)
            {
                return;
            }

            TextField.Enabled = !Item?.IsReadOnly ?? true;
        }

        private void UpdateLabel()
        {
            if (Label == null)
            {
                return;
            }

            Label.Text = Item?.Label;

            if (_textFieldLeadingConstraint == null || _labelWidthConstraint == null)
            {
                return;
            }

            var hasLabel = !string.IsNullOrWhiteSpace(Item?.Label);

            _textFieldLeadingConstraint.Constant = hasLabel ? -10 : 0;
            _labelWidthConstraint.Active = !hasLabel;

            if (TextField == null)
            {
                return;
            }

            TextField.TextAlignment = hasLabel ? UITextAlignment.Right : UITextAlignment.Left;
        }

        protected virtual void UpdatePlaceholder()
        {
            if (TextField == null)
            {
                return;
            }

            TextField.Placeholder = Item?.Placeholder;
        }

        protected virtual void UpdateInputType()
        {
            if (TextField == null)
            {
                return;
            }

            TextField.KeyboardType = Item?.InputType switch
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

        protected virtual void UpdateSecuredState()
        {
            if (TextField == null)
            {
                return;
            }

            TextField.SecureTextEntry = Item?.IsSecured ?? false;
        }

        protected virtual void UpdateValue()
        {
            if (TextField == null)
            {
                return;
            }

            TextField.Text = Item?.FormattedValue;
        }

        protected virtual void UpdateValidationErrorMessage()
        {
            if (Label == null)
            {
                return;
            }

            Label.TextColor = Item?.ValidationErrorMessage == null ? UIColor.Label : UIColor.SystemRed;
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateLabel();
            UpdatePlaceholder();
            UpdateInputType();
            UpdateSecuredState();
            UpdateValue();
            UpdateValidationErrorMessage();
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Item.IsReadOnly):
                    UpdateReadOnlyState();
                    break;
                case nameof(Item.Label):
                    UpdateLabel();
                    break;
                case nameof(Item.Placeholder):
                    UpdatePlaceholder();
                    break;
                case nameof(Item.InputType):
                    UpdateInputType();
                    break;
                case nameof(Item.IsSecured):
                    UpdateSecuredState();
                    break;
                case nameof(Item.FormattedValue):
                    UpdateValue();
                    break;
                case nameof(Item.ValidationErrorMessage):
                    UpdateValidationErrorMessage();
                    break;
            }
        }

        public override void OnSelected()
        {
            base.OnSelected();

            TextField?.BecomeFirstResponder();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var textField = TextField;
                if (textField != null)
                {
                    textField.EditingChanged -= OnEditingChanged;
                }

                TextField?.Dispose();
                TextField = null;

                Label?.Dispose();
                Label = null;
            }

            base.Dispose(disposing);
        }
    }
}