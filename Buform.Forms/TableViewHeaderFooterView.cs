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
        private readonly HeaderFooterView _headerFooterView;

        private CGSize _estimatedSize;

        public TableViewHeaderFooterView(
            Type viewType,
            object bindingContext
        )
            : base((NSString) viewType.Name)
        {
            _headerFooterView = (Activator.CreateInstance(viewType) as HeaderFooterView)!;

            _headerFooterView.BindingContext = bindingContext;

            var contentRenderer = Platform.CreateRenderer(_headerFooterView);

            AddSubview(contentRenderer.NativeView);

            MeasureView();
        }

        private void MeasureView()
        {
            var width = Bounds.Width;

            var request = _headerFooterView.Measure(
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

            Layout.LayoutChildIntoBoundingRegion(_headerFooterView, bounds);
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