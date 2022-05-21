using UIKit;

namespace Buform
{
    public interface IExpandableCellController
    {
        void OnExpanded(UITableViewCell sourceCell, UITableViewCell expandedCell);
        void OnCollapsed(UITableViewCell sourceCell, UITableViewCell collapsedCell);
    }
}