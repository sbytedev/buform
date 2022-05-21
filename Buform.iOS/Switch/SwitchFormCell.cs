using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(SwitchFormCell))]
    public class SwitchFormCell : FormCell<SwitchFormItem>
    {
        protected virtual UILabel? Label { get; set; }
        protected virtual UISwitch? Switch { get; set; }

        public SwitchFormCell()
        {
            /* Required constructor */
        }

        public SwitchFormCell(IntPtr handle) : base(handle)
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

            Switch = new UISwitch();
            Switch.ValueChanged += OnValueChanged;

            AccessoryView = Switch;

            ContentView.AddSubviews(Label);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, ContentView.LayoutMargins.Left),
                Label.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -ContentView.LayoutMargins.Right)
            });
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (Item == null)
            {
                return;
            }

            Item.Value = Switch?.On ?? false;
        }

        private void UpdateReadOnlyState()
        {
            if (Switch == null)
            {
                return;
            }

            Switch.Enabled = !Item?.IsReadOnly ?? true;
        }

        private void UpdateLabel()
        {
            if (Label == null)
            {
                return;
            }

            Label.Text = Item?.Label;
        }

        private void UpdateValue(bool isAnimated)
        {
            Switch?.SetState(Item?.Value ?? false, isAnimated);
        }
        
        private void UpdateValidationErrorMessage()
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
            UpdateValue(false);
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
                case nameof(Item.Value):
                    UpdateValue(true);
                    break;
                case nameof(Item.ValidationErrorMessage):
                    UpdateValidationErrorMessage();
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var @switch = Switch;
                if (@switch != null)
                {
                    @switch.ValueChanged -= OnValueChanged;
                }

                Switch?.Dispose();
                Switch = null;

                Label?.Dispose();
                Label = null;
            }

            base.Dispose(disposing);
        }
    }
}