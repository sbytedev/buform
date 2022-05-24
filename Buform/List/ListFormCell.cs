using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(ListFormCell))]
    public class ListFormCell : FormCell<IListFormItem>
    {
        protected virtual UILabel? Label { get; set; }

        public ListFormCell()
        {
            /* Required constructor */
        }

        public ListFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected override void Initialize()
        {
            SelectionStyle = UITableViewCellSelectionStyle.Default;

            Label = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.Label
            };

            ContentView.AddSubviews(Label);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                Label.TrailingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TrailingAnchor)
            });
        }

        protected virtual void UpdateValue()
        {
            if (Label == null)
            {
                return;
            }

            Label.Text = Item?.FormattedValue;
        }

        protected override void OnItemSet()
        {
            UpdateValue();
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Item.FormattedValue):
                    UpdateValue();
                    break;
            }
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