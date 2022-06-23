using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Buform
{
    public sealed class TableViewHeaderFooterView : UITableViewHeaderFooterView
    {
        private readonly FormsHeaderFooterView _formsHeaderFooterView;
        private readonly IVisualElementRenderer _renderer;

        private CGSize _estimatedSize;

        public TableViewHeaderFooterView(
            Type viewType,
            object bindingContext
        )
            : base((NSString) viewType.Name)
        {
            _formsHeaderFooterView = (Activator.CreateInstance(viewType) as FormsHeaderFooterView)!;

            _formsHeaderFooterView.BindingContext = bindingContext;

            _renderer = Platform.CreateRenderer(_formsHeaderFooterView);

            AddSubview(_renderer.NativeView);

            MeasureView();
        }

        private void MeasureView()
        {
            var width = Bounds.Width;

            var request = _formsHeaderFooterView.Measure(
                width,
                double.PositiveInfinity,
                MeasureFlags.IncludeMargins
            );

            var verticalMargins = LayoutMargins.Top + LayoutMargins.Bottom;

            _estimatedSize = new CGSize(
                width,
                Math.Ceiling(request.Request.Height - verticalMargins)
            );

            var bounds = new Rectangle(
                0,
                0,
                _estimatedSize.Width,
                _estimatedSize.Height
            );

            Layout.LayoutChildIntoBoundingRegion(_formsHeaderFooterView, bounds);

            _renderer.NativeView.Frame = bounds.ToRectangleF();
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            return _estimatedSize;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            MeasureView();
        }
    }
}