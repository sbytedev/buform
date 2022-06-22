using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Buform
{
    public class FormsFormTableViewSource : FormTableViewSource
    {
        public FormsFormTableViewSource(UITableView tableView) : base(tableView)
        {
            /* Required constructor */
        }

        protected override UITableViewHeaderFooterView? GetViewForHeader(nint section, object sectionItem)
        {
            var sectionType = sectionItem.GetType();

            if (!BuformForms.TryGetHeaderViewType(sectionType, out var viewType))
            {
                return base.GetViewForHeader(section, sectionItem);
            }

            if (viewType == null)
            {
                return base.GetViewForHeader(section, sectionItem);
            }

            var headerView = new TableViewHeaderFooterView(viewType, sectionItem);

            return headerView;
        }
    }
}