using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SByteDev.Common.Extensions;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(SegmentsFormCell))]
    public class SegmentsFormCell : FormCell<ISegmentsFormItem>
    {
        private NSLayoutConstraint? _segmentedControlLeadingConstraint;
        private NSLayoutConstraint? _labelWidthConstraint;

        private IEnumerable<IListFormItem> _items = Array.Empty<IListFormItem>();

        protected virtual UILabel? Label { get; set; }
        protected virtual UISegmentedControl? SegmentedControl { get; set; }

        public SegmentsFormCell()
        {
            /* Required constructor */
        }

        public SegmentsFormCell(IntPtr handle) : base(handle)
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

            SegmentedControl = new UISegmentedControl
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            SegmentedControl.ValueChanged += OnValueChanged;

            SegmentedControl.SetContentCompressionResistancePriority(1000, UILayoutConstraintAxis.Horizontal);

            ContentView.AddSubviews(Label, SegmentedControl);

            _segmentedControlLeadingConstraint = SegmentedControl.LeadingAnchor.ConstraintEqualTo(Label.TrailingAnchor, 8);

            _labelWidthConstraint = Label.WidthAnchor.ConstraintEqualTo(0);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, ContentView.LayoutMargins.Left),
                _labelWidthConstraint,
                SegmentedControl.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 8),
                SegmentedControl.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -8),
                _segmentedControlLeadingConstraint,
                SegmentedControl.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -ContentView.LayoutMargins.Right)
            });
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if (SegmentedControl == null)
            {
                return;
            }

            if (Item == null)
            {
                return;
            }

            if (!_items.Any())
            {
                Item.Value = null;
            }
            else if (SegmentedControl.SelectedSegment < 0)
            {
                Item.Value = null;
            }
            else
            {
                Item.Value = _items.ElementAtOrDefault((int)SegmentedControl.SelectedSegment)?.Value;
            }
        }

        protected virtual void UpdateReadOnlyState()
        {
            if (SegmentedControl == null)
            {
                return;
            }

            SegmentedControl.Enabled = !Item?.IsReadOnly ?? true;
        }

        protected virtual void UpdateLabel()
        {
            if (Label == null)
            {
                return;
            }

            Label.Text = Item?.Label;

            if (_segmentedControlLeadingConstraint == null || _labelWidthConstraint == null)
            {
                return;
            }

            var hasLabel = !string.IsNullOrWhiteSpace(Item?.Label);

            _segmentedControlLeadingConstraint.Constant = hasLabel ? 8 : 0;
            _labelWidthConstraint.Active = !hasLabel;
        }

        protected virtual void UpdateItems()
        {
            if (SegmentedControl == null)
            {
                return;
            }

            _items = Item?.Items ?? Array.Empty<IListFormItem>();

            SegmentedControl.RemoveAllSegments();

            var items = _items as IListFormItem[] ?? _items.ToArray();

            for (var index = 0; index < items.Length; index++)
            {
                var value = items[index];

                SegmentedControl.InsertSegment(value.FormattedValue ?? string.Empty, index, false);
            }
        }

        protected virtual void UpdateValue()
        {
            if (SegmentedControl == null)
            {
                return;
            }

            if (!_items.Any() || Item?.Value == null)
            {
                SegmentedControl.SelectedSegment = nint.MinValue;
            }
            else
            {
                var holder = _items.FirstOrDefault(item => Equals(item.Value, Item.Value));

                SegmentedControl.SelectedSegment = holder == null 
                    ? nint.MinValue 
                    : _items.IndexOf(holder);
            }
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
            UpdateItems();
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
                case nameof(Item.Items):
                    UpdateItems();
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
                var segmentedControl = SegmentedControl;
                if (segmentedControl != null)
                {
                    segmentedControl.ValueChanged -= OnValueChanged;
                }

                SegmentedControl?.Dispose();
                SegmentedControl = null;

                Label?.Dispose();
                Label = null;
            }

            base.Dispose(disposing);
        }
    }
}