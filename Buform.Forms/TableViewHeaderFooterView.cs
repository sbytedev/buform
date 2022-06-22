using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Buform
{
    public sealed class TableViewHeaderFooterView : UITableViewHeaderFooterView
    {
        private readonly Type _viewType;

            public TableViewHeaderFooterView(Type viewType)
                : base((NSString) viewType.Name)
            {
                _viewType = viewType;
            }

            private UITableView? GetTableView()
            {
                var view = this as UIView;

                while (true)
                {
                    switch (view.Superview)
                    {
                        case null:
                            return null;
                        case UITableView tableView:
                            return tableView;
                        default:
                            view = view.Superview;
                            break;
                    }
                }
            }

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                var tableView = GetTableView();

                if (tableView == null)
                {
                    return;
                }

                if (Activator.CreateInstance(_viewType) is not FormGroupView formGroupView)
                {
                    return;
                }

                var contentRenderer = Platform.CreateRenderer(formGroupView);

                Platform.SetRenderer(formGroupView, contentRenderer);

                var width = Bounds.Width;

                var request = formGroupView.Measure(
                    width,
                    double.PositiveInfinity,
                    MeasureFlags.IncludeMargins
                );

                var verticalMargins = LayoutMargins.Top + LayoutMargins.Bottom;

                var bounds = new Rectangle(
                    0,
                    0,
                    width,
                    Math.Ceiling(request.Request.Height) - verticalMargins
                );

                Layout.LayoutChildIntoBoundingRegion(formGroupView, bounds);

                Device.BeginInvokeOnMainThread(() => { tableView.TableHeaderView = contentRenderer.NativeView; });

                contentRenderer.NativeView.Frame = Bounds;
            }
    }
}