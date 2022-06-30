using System;
using Buform.Example.Core;
using CoreGraphics;
using Foundation;
using SByteDev.Common.Extensions;
using UIKit;

namespace Buform.Example.MvvmCross.iOS
{
    [Preserve(AllMembers = true)]
    [Register(nameof(RandomNumberGeneratorCell))]
    public sealed class RandomNumberGeneratorCell : FormCell<RandomNumberGeneratorItem>
    {
        private const string GoForwardIconName = "goforward";

        private UILabel? _titleLabel;
        private UILabel? _valueLabel;
        private UIButton? _button;

        public RandomNumberGeneratorCell()
        {
            /* Required constructor */
        }

        public RandomNumberGeneratorCell(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        private void UpdateReadOnlyState()
        {
            if (Item == null)
            {
                return;
            }

            if (_button == null)
            {
                return;
            }

            _button.Enabled = !Item.IsReadOnly;
        }

        private void UpdateLabel()
        {
            if (_titleLabel == null)
            {
                return;
            }

            _titleLabel.Text = Item?.Label;
        }

        private void UpdateValue()
        {
            if (_valueLabel == null)
            {
                return;
            }

            _valueLabel.Text = Item?.Value.ToString();
        }

        private void OnTouchUpInside(object _, EventArgs __)
        {
            Item?.GenerateCommand.SafeExecute();
        }

        protected override void Initialize()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            _titleLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.Label
            };

            _valueLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.PreferredBody,
                TextColor = UIColor.SecondaryLabel
            };

            var image = UIImage.GetSystemImage(GoForwardIconName)!;

            _button = UIButton.FromType(UIButtonType.System);
            _button.Frame = new CGRect(0, 0, image.Size.Width, image.Size.Height);
            _button.TouchUpInside += OnTouchUpInside;
            _button.SetImage(image, UIControlState.Normal);

            AccessoryView = _button;

            ContentView.AddSubviews(_titleLabel, _valueLabel);

            ContentView.AddConstraints(new[]
            {
                _titleLabel.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                _titleLabel.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                _titleLabel.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                _valueLabel.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                _valueLabel.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                _valueLabel.LeadingAnchor.ConstraintEqualTo(_titleLabel.LayoutMarginsGuide.TrailingAnchor, 16),
            });
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateLabel();
            UpdateValue();
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
                    UpdateValue();
                    break;
            }
        }
    }
}