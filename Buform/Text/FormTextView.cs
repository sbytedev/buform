using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [Register(nameof(FormTextView))]
    public sealed class FormTextView : UITextView
    {
        private readonly UILabel _placeholderLabel;

        public string? Placeholder
        {
            get => _placeholderLabel.Text;
            set => _placeholderLabel.Text = value;
        }

        public override string? Text
        {
            get => base.Text;
            set
            {
                base.Text = value;

                TextChanged();
            }
        }

        public override CGSize IntrinsicContentSize => new(base.IntrinsicContentSize.Width, 200);

        public FormTextView()
        {
            TextContainerInset = UIEdgeInsets.Zero;
            DataDetectorTypes = UIDataDetectorType.All;
            Font = UIFont.PreferredBody;
            TextColor = UIColor.Label;

            TextContainer.LineFragmentPadding = 0;
            
            Changed += OnChanged;

            _placeholderLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = UIColor.PlaceholderText,
                Font = UIFont.PreferredBody,
                TextAlignment = TextAlignment,
                Lines = 0
            };

            AddSubview(_placeholderLabel);
        }

        private void OnChanged(object sender, EventArgs e)
        {
            TextChanged();
        }

        private void TextChanged()
        {
            _placeholderLabel.Hidden = !string.IsNullOrEmpty(Text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Changed -= OnChanged;
            }

            base.Dispose(disposing);
        }
    }
}