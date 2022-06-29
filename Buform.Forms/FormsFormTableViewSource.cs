using System;
using UIKit;

namespace Buform
{
    public class FormsFormTableViewSource : FormTableViewSource
    {
        public FormsFormTableViewSource(UITableView tableView) : base(tableView)
        {
            /* Required constructor */
        }

        protected override UITableViewHeaderFooterView? GetViewForFooter(nint section, object sectionItem)
        {
            var sectionType = sectionItem.GetType();

            if (!BuformForms.TryGetFooterViewType(sectionType, out var viewType))
            {
                return base.GetViewForFooter(section, sectionItem);
            }

            return viewType == null
                ? base.GetViewForFooter(section, sectionItem)
                : new TableViewHeaderFooterView(viewType, sectionItem);
        }

        protected override UITableViewHeaderFooterView? GetViewForHeader(nint section, object sectionItem)
        {
            var sectionType = sectionItem.GetType();

            if (!BuformForms.TryGetHeaderViewType(sectionType, out var viewType))
            {
                return base.GetViewForHeader(section, sectionItem);
            }

            return viewType == null
                ? base.GetViewForHeader(section, sectionItem)
                : new TableViewHeaderFooterView(viewType, sectionItem);
        }
    }
}