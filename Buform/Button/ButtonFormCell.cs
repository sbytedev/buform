using System;
using Foundation;
using SByteDev.Common.Extensions;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(ButtonFormCell))]
    public class ButtonFormCell : FormCell<ButtonFormItem>
    {
        public override bool IsSelectable => !Item?.IsReadOnly ?? false;

        protected virtual UILabel? Label { get; set; }

        public ButtonFormCell()
        {
            /* Required constructor */
        }

        public ButtonFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected override void Initialize()
        {
            Label = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.Label
            };

            ContentView.AddSubviews(Label);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, ContentView.LayoutMargins.Top),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -ContentView.LayoutMargins.Bottom),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, ContentView.LayoutMargins.Left),
                Label.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -ContentView.LayoutMargins.Right)
            });
        }

        protected virtual void UpdateReadOnlyState()
        {
            if (Label == null)
            {
                return;
            }

            Label.Enabled = !Item?.IsReadOnly ?? true;
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
            if (Label == null)
            {
                return;
            }

            Label.TextColor = Item?.InputType switch
            {
                ButtonInputType.Default => UIColor.Label,
                ButtonInputType.Done => UIButton.Appearance.TintColor,
                ButtonInputType.Destructive => UIColor.SystemRed,
                null => UIColor.SystemBlue,
                _ => throw new ArgumentOutOfRangeException(nameof(Item.InputType), Item?.InputType, null)
            };
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateLabel();
            UpdateInputType();
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
                case nameof(Item.InputType):
                    UpdateInputType();
                    break;
            }
        }

        public override void OnSelected()
        {
            base.OnSelected();

            Item?.Value.SafeExecute();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Label?.Dispose();
                Label = null;
            }

            base.Dispose(disposing);
        }
    }
}