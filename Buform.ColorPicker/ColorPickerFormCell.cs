using System;
using CoreGraphics;
using Foundation;
using SByteDev.Xamarin.iOS.Extensions;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(ColorPickerFormCell))]
    public class ColorPickerFormCell : FormCell<ColorPickerFormItem>
    {
        protected virtual IUIColorPickerViewControllerDelegate? Delegate { get; private set; } 
        
        protected virtual UILabel? Label { get; set; }
        protected virtual UIView? ColorView { get; set; }

        public override bool IsSelectable => !Item?.IsReadOnly ?? false;

        public ColorPickerFormCell()
        {
            /* Required constructor */
        }

        public ColorPickerFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected virtual UIColorPickerViewController CreateColorPickerViewController(ColorPickerFormItem item)
        {
            Delegate?.Dispose();
            Delegate = new ColorPickerViewControllerDelegate(item);

            return new UIColorPickerViewController
            {
                Delegate = Delegate,
                SelectedColor = item.Value.ToColor()
            };
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

            ColorView = new UIView
            {
                Frame = new CGRect(0, 0, 22, 22),
                ClipsToBounds = true
            };

            ColorView.Layer.CornerRadius = 11;

            AccessoryView = ColorView;

            ContentView.AddSubviews(Label);

            ContentView.AddConstraints(new[]
            {
                Label.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                Label.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                Label.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                Label.TrailingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TrailingAnchor)
            });
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
            if (ColorView == null)
            {
                return;
            }

            ColorView.BackgroundColor = Item?.Value.ToColor() ?? UIColor.Clear;
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
            UpdateLabel();
            UpdateValue();
            UpdateValidationErrorMessage();
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
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

        public override async void OnSelected()
        {
            base.OnSelected();

            if (Item == null)
            {
                return;
            }

            var colorPickerViewController = CreateColorPickerViewController(Item);

            var viewController = GetViewController();

            if (viewController == null)
            {
                return;
            }

            await viewController.PresentViewControllerAsync(colorPickerViewController, true).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ColorView?.Dispose();
                ColorView = null;

                Label?.Dispose();
                Label = null;

                Delegate?.Dispose();
                Delegate = null;
            }

            base.Dispose(disposing);
        }
    }
}