using System;
using Buform.Example.Core;
using SByteDev.Common.Extensions;
using UIKit;

namespace Buform.Example.MvvmCross.iOS
{
    public sealed class DigitGenerationCell : FormCell<DigitGenerationItem>
    {
        private UILabel? _titleLabel;
        private UILabel? _valueLabel;
        private UIButton? _button;

        public DigitGenerationCell()
        {
            /* Required constructor */
        }

        public DigitGenerationCell(IntPtr handle) : base(handle)
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

            _button.Enabled = Item.IsReadOnly;
        }

        private void UpdateTitle()
        {
            if (_titleLabel == null)
            {
                return;
            }

            _titleLabel.Text = Item?.Title;
        }

        private void UpdateValue()
        {
            if (_valueLabel == null)
            {
                return;
            }

            _valueLabel.Text = Item?.Value.ToString();
        }

        private void UpdateButtonText()
        {
            _button?.SetTitle(Item?.RegenerateButtonText, UIControlState.Normal);
        }

        private void ExecuteCommand(object _, EventArgs __)
        {
            Item?.RegenerateCommand.SafeExecute();
        }

        protected override void Initialize()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            ContentView.UserInteractionEnabled = true;

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

            _button = UIButton.FromType(UIButtonType.Custom);
            _button.TranslatesAutoresizingMaskIntoConstraints = false;
            _button.TouchUpInside += ExecuteCommand;

            ContentView.AddSubviews(_titleLabel, _valueLabel, _button);

            ContentView.AddConstraints(new[]
            {
                _titleLabel.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                _titleLabel.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                _titleLabel.LeadingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.LeadingAnchor),
                _valueLabel.TopAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TopAnchor),
                _valueLabel.BottomAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.BottomAnchor),
                _valueLabel.LeadingAnchor.ConstraintEqualTo(_titleLabel.LayoutMarginsGuide.TrailingAnchor, 16),
                _button.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor),
                _button.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor),
                _button.TrailingAnchor.ConstraintEqualTo(ContentView.LayoutMarginsGuide.TrailingAnchor)
            });
        }

        protected override void OnItemSet()
        {
            UpdateReadOnlyState();
            UpdateTitle();
            UpdateValue();
            UpdateButtonText();
        }

        protected override void OnItemPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Item.IsReadOnly):
                    UpdateReadOnlyState();
                    break;
                case nameof(Item.Title):
                    UpdateTitle();
                    break;
                case nameof(Item.Value):
                    UpdateValue();
                    break;
                case nameof(Item.RegenerateButtonText):
                    UpdateButtonText();
                    break;
            }
        }
    }
}