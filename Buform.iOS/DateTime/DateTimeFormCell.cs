using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(DateTimeFormCell))]
    public class DateTimeFormCell : FormCell<DateTimeFormItem>
    {
        protected virtual UILabel? Label { get; set; }
        protected virtual UIDatePicker? DatePicker { get; set; }

        public DateTimeFormCell()
        {
            /* Required constructor */
        }

        public DateTimeFormCell(IntPtr handle) : base(handle)
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

            DatePicker = new UIDatePicker()
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            DatePicker.ValueChanged += OnValueChanged;

            DatePicker.SetContentCompressionResistancePriority(1000, UILayoutConstraintAxis.Horizontal);

            ContentView.AddSubviews(Label, DatePicker);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, ContentView.LayoutMargins.Left),
                DatePicker.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 4),
                DatePicker.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -4),
                DatePicker.LeadingAnchor.ConstraintEqualTo(Label.TrailingAnchor, 8),
                DatePicker.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -ContentView.LayoutMargins.Right)
            });
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if (Item == null)
            {
                return;
            }

            Item.Value = DatePicker?.Date.ToDateTime() ?? DateTime.UtcNow;
        }

        protected virtual void UpdateReadOnlyState()
        {
            if (DatePicker == null)
            {
                return;
            }

            DatePicker.Enabled = !Item?.IsReadOnly ?? false;
        }

        protected virtual void UpdateMinValue()
        {
            if (DatePicker == null)
            {
                return;
            }

            DatePicker.MinimumDate = Item?.MinValue.ToDate() ?? NSDate.DistantPast;
        }

        protected virtual void UpdateMaxValue()
        {
            if (DatePicker == null)
            {
                return;
            }

            DatePicker.MaximumDate = Item?.MaxValue.ToDate() ?? NSDate.DistantFuture;
        }

        protected virtual void UpdateLabel()
        {
            if (Label == null)
            {
                return;
            }

            Label.Text = Item?.Label;
        }

        protected virtual void UpdateInputType()
        {
            if (DatePicker == null)
            {
                return;
            }

            var inputType = Item?.InputType;

            switch (inputType)
            {
                case DateTimeInputType.Date:
                    DatePicker.Mode = UIDatePickerMode.Date;
                    break;
                case DateTimeInputType.Time:
                    DatePicker.Mode = UIDatePickerMode.Time;
                    break;
                case DateTimeInputType.DateTime:
                case null:
                    DatePicker.Mode = UIDatePickerMode.DateAndTime;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
            }
        }

        protected virtual void UpdateValue()
        {
            if (DatePicker == null)
            {
                return;
            }

            var dateTime = Item?.Value ?? DateTime.UtcNow;
            var date = dateTime.ToDate();

            if (DatePicker.MaximumDate?.Compare(date) == NSComparisonResult.Ascending)
            {
                date = DatePicker.MaximumDate;
            }

            if (DatePicker.MinimumDate?.Compare(date) == NSComparisonResult.Descending)
            {
                date = DatePicker.MinimumDate;
            }

            DatePicker.Date = date;
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
            UpdateLabel();
            UpdateInputType();
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
                case nameof(Item.Label):
                    UpdateLabel();
                    break;
                case nameof(Item.InputType):
                    UpdateInputType();
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
                var datePicker = DatePicker;
                if (datePicker != null)
                {
                    datePicker.ValueChanged -= OnValueChanged;
                }

                DatePicker?.Dispose();
                DatePicker = null;

                Label?.Dispose();
                Label = null;
            }

            base.Dispose(disposing);
        }
    }
}