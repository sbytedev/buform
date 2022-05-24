using System;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(SliderFormCell))]
    public class SliderFormCell : FormCell<SliderFormItem>
    {
        protected virtual UISlider? Slider { get; set; }

        public SliderFormCell()
        {
            /* Required constructor */
        }

        public SliderFormCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        protected override void Initialize()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            Slider = new UISlider
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            Slider.ValueChanged += OnValueChanged;

            ContentView.AddSubviews(Slider);

            ContentView.AddConstraints(new[]
            {
                Slider.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                Slider.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                Slider.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                Slider.TrailingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TrailingAnchor)
            });
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if (Slider == null)
            {
                return;
            }

            if (Item == null)
            {
                return;
            }

            Item.Value = Slider.Value;
        }

        protected virtual void UpdateReadOnlyState()
        {
            if (Slider == null)
            {
                return;
            }

            Slider.Enabled = !Item?.IsReadOnly ?? true;
        }

        protected virtual void UpdateMinValue()
        {
            if (Slider == null)
            {
                return;
            }

            Slider.MinValue = Item?.MinValue ?? float.MinValue;
        }

        protected virtual void UpdateMaxValue()
        {
            if (Slider == null)
            {
                return;
            }

            Slider.MaxValue = Item?.MaxValue ?? float.MaxValue;
        }

        protected virtual void UpdateValue(bool isAnimated)
        {
            Slider?.SetValue(Item?.Value ?? 0f, isAnimated);
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateMinValue();
            UpdateMaxValue();
            UpdateValue(false);
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
                case nameof(Item.Value):
                    UpdateValue(true);
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var slider = Slider;
                if (slider != null)
                {
                    slider.ValueChanged -= OnValueChanged;
                }

                Slider?.Dispose();
                Slider = null;
            }

            base.Dispose(disposing);
        }
    }
}