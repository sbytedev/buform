using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(StepperFormCell))]
    public class StepperFormCell : FormCell<StepperFormItem>
    {
        protected virtual UILabel? Label { get; set; }
        protected virtual UIStepper? Stepper { get; set; }

        public StepperFormCell()
        {
            /* Required constructor */
        }

        public StepperFormCell(IntPtr handle) : base(handle)
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

            Stepper = new UIStepper();
            Stepper.ValueChanged += OnValueChanged;

            AccessoryView = Stepper;

            ContentView.AddSubviews(Label);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, ContentView.LayoutMargins.Left),
                Label.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -ContentView.LayoutMargins.Right)
            });
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if (Item == null)
            {
                return;
            }

            Item.Value = (int)(Stepper?.Value ?? 0);
        }

        protected virtual void UpdateReadOnlyState()
        {
            if (Stepper == null)
            {
                return;
            }

            Stepper.Enabled = !Item?.IsReadOnly ?? true;
        }

        protected virtual void UpdateMinValue()
        {
            if (Stepper == null)
            {
                return;
            }

            Stepper.MinimumValue = Item?.MinValue ?? float.MinValue;
        }

        protected virtual void UpdateMaxValue()
        {
            if (Stepper == null)
            {
                return;
            }

            Stepper.MaximumValue = Item?.MaxValue ?? float.MaxValue;
        }

        protected virtual void UpdateStepAmount()
        {
            if (Stepper == null)
            {
                return;
            }

            Stepper.StepValue = Item?.StepAmount ?? 1;
        }

        protected virtual void UpdateLabel()
        {
            if (Label == null)
            {
                return;
            }

            Label.Text = Item?.Label;
        }

        protected virtual void UpdateValue()
        {
            if (Stepper == null)
            {
                return;
            }

            Stepper.Value = Item?.Value ?? 0;
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
            UpdateMinValue();
            UpdateMaxValue();
            UpdateStepAmount();
            UpdateLabel();
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
                case nameof(Item.MinValue):
                    UpdateMinValue();
                    break;
                case nameof(Item.MaxValue):
                    UpdateMaxValue();
                    break;
                case nameof(Item.StepAmount):
                    UpdateStepAmount();
                    break;
                case nameof(Item.Label):
                    UpdateLabel();
                    break;
                case nameof(Item.Value):
                    UpdateValue();
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
                var stepper = Stepper;
                if (stepper != null)
                {
                    stepper.ValueChanged -= OnValueChanged;
                }

                Stepper?.Dispose();
                Stepper = null;

                Label?.Dispose();
                Label = null;
            }

            base.Dispose(disposing);
        }
    }
}