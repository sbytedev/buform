using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Foundation;
using SByteDev.Common.Extensions;
using SByteDev.Xamarin.iOS.Extensions;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    public abstract class TableViewSource : UITableViewSource
    {
        private readonly IDictionary<object, IDisposable> _sectionItemsSubscriptions;
        private readonly List<NSIndexPath> _expandedIndexPaths;
        private readonly IDictionary<Type, IExpandableCellController> _expandableCellControllers;

        private IEnumerable? _sourceItems;
        private IEnumerable<IEnumerable<object>>? _items;
        private IDisposable? _sectionsSubscription;

        protected bool IsDisposed { get; private set; }

        protected UITableView TableView { get; }

        public UITableViewRowAnimation AddRowAnimation { get; set; }
        public UITableViewRowAnimation RemoveRowAnimation { get; set; }
        public UITableViewRowAnimation ReplaceRowAnimation { get; set; }
        public UITableViewRowAnimation AddSectionAnimation { get; set; }
        public UITableViewRowAnimation RemoveSectionAnimation { get; set; }
        public UITableViewRowAnimation ReplaceSectionAnimation { get; set; }

        public IEnumerable? Items
        {
            get => _sourceItems;
            set
            {
                if (ReferenceEquals(_sourceItems, value))
                {
                    return;
                }

                ResetExpandableCells();

                _sourceItems = value;

                _items = _sourceItems switch
                {
                    IEnumerable<IEnumerable<object>> items => items,
                    IEnumerable<object> items => new[] { items },
                    _ => null
                };

                this.InvokeOnMainThreadIfNeeded(ReloadTableViewData);
            }
        }

        protected TableViewSource(UITableView tableView)
        {
            TableView = tableView;

            _sectionItemsSubscriptions = new Dictionary<object, IDisposable>();
            _expandedIndexPaths = new List<NSIndexPath>();
            _expandableCellControllers = new Dictionary<Type, IExpandableCellController>();

            TableView.Source = this;
        }

        private void ReloadTableViewData()
        {
            ClearSubscriptions();
            AddSubscriptions();

            TableView.ReloadData();
        }

        private void ClearSubscriptions()
        {
            ClearSectionItemsSubscription();
            ClearItemsSubscriptions();
        }

        private void ClearSectionItemsSubscription()
        {
            _sectionsSubscription?.Dispose();
        }

        private void ClearItemsSubscriptions()
        {
            ClearItemsSubscriptions(_sectionItemsSubscriptions.Keys);
        }

        private void ClearItemsSubscriptions(IEnumerable<object> sectionItems)
        {
            var array = sectionItems.ToArray();

            for (var index = array.Length - 1; index >= 0; index--)
            {
                ClearItemsSubscription(array[index]);
            }
        }

        private void ClearItemsSubscription(object sectionItem)
        {
            if (!_sectionItemsSubscriptions.TryGetValue(sectionItem, out var subscription))
            {
                return;
            }

            subscription?.Dispose();

            _sectionItemsSubscriptions.Remove(sectionItem);
        }

        private void AddSubscriptions()
        {
            if (_items == null)
            {
                return;
            }

            AddSectionItemsSubscription(_items);
            AddItemsSubscriptions(_items);
        }

        private void AddSectionItemsSubscription(IEnumerable<IEnumerable<object>> sectionItems)
        {
            if (sectionItems is INotifyCollectionChanged notifyCollectionChanged)
            {
                _sectionsSubscription = new WeakEventSubscription(notifyCollectionChanged, OnSectionItemsChanged);
            }
        }

        private void AddItemsSubscriptions(IEnumerable<IEnumerable<object>> sectionItems)
        {
            foreach (var sectionItem in sectionItems)
            {
                AddItemSubscription(sectionItem);
            }
        }

        private void AddItemSubscription(IEnumerable<object> sectionItem)
        {
            if (sectionItem is INotifyCollectionChanged notifyCollectionChanged)
            {
                _sectionItemsSubscriptions[sectionItem] = new WeakEventSubscription(notifyCollectionChanged, OnItemsChanged); 
            }
        }

        private void OnSectionItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_items == null || _sectionsSubscription == null)
            {
                return;
            }

            this.InvokeOnMainThreadIfNeeded(() => UpdateSections(args));
        }

        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_items == null || !_sectionItemsSubscriptions.TryGetValue(sender, out _))
            {
                return;
            }

            var section = _items.IndexOf(sender);

            this.InvokeOnMainThreadIfNeeded(() => UpdateSection(section, args));
        }

        private void UpdateSections(NotifyCollectionChangedEventArgs args)
        {
            ResetExpandableCells();

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddSections(args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveSections(args);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceSections(args);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveSections(args);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ReloadTableViewData();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(NotifyCollectionChangedEventArgs), args, null);
            }
        }

        private void UpdateSection(int section, NotifyCollectionChangedEventArgs args)
        {
            ResetExpandableCells();

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddSectionItems(section, args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveSectionItems(section, args);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceSectionItems(section, args);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveSectionItems(section, args);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ReloadTableViewData();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(NotifyCollectionChangedEventArgs), args, null);
            }
        }

        private void AddSections(NotifyCollectionChangedEventArgs args)
        {
            TableView.InsertSections(
                CreateIndexSet(args.NewStartingIndex, args.NewItems.Count),
                AddSectionAnimation
            );

            foreach (var item in args.NewItems)
            {
                if (item is IEnumerable<object> items)
                {
                    AddItemSubscription(items);
                }
            }
        }

        private void RemoveSections(NotifyCollectionChangedEventArgs args)
        {
            TableView.DeleteSections(
                CreateIndexSet(args.OldStartingIndex, args.OldItems.Count),
                RemoveSectionAnimation
            );

            foreach (var item in args.OldItems)
            {
                ClearItemsSubscription(item);
            }
        }

        private void ReplaceSections(NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems.Count != args.OldItems.Count)
            {
                return;
            }

            TableView.ReloadSections(
                CreateIndexSet(args.NewStartingIndex, args.NewItems.Count),
                ReplaceSectionAnimation
            );
        }

        private void MoveSections(NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
            {
                return;
            }

            TableView.MoveSection(args.OldStartingIndex, args.NewStartingIndex);
        }

        private void AddSectionItems(int section, NotifyCollectionChangedEventArgs args)
        {
            TableView.InsertRows(
                CreateIndexPaths(args.NewStartingIndex, args.NewItems.Count, section),
                AddRowAnimation
            );
        }

        private void RemoveSectionItems(int section, NotifyCollectionChangedEventArgs args)
        {
            TableView.DeleteRows(
                CreateIndexPaths(args.OldStartingIndex, args.OldItems.Count, section),
                RemoveRowAnimation
            );

            if (GetSectionItem(section)?.Any() ?? false)
            {
                return;
            }

            TableView.ReloadSections(
                CreateIndexSet(section, 1),
                RemoveRowAnimation
            );
        }

        private void ReplaceSectionItems(int section, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems.Count != args.OldItems.Count)
            {
                return;
            }

            var indexPaths = Enumerable
                .Range(args.NewStartingIndex, args.NewItems.Count)
                .Select(item => NSIndexPath.FromRowSection(item, section))
                .ToArray();

            TableView.ReloadRows(indexPaths, ReplaceRowAnimation);
        }

        private void MoveSectionItems(int section, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
            {
                return;
            }

            TableView.MoveRow(
                NSIndexPath.FromRowSection(args.OldStartingIndex, section),
                NSIndexPath.FromRowSection(args.NewStartingIndex, section)
            );
        }

        private static NSIndexPath[] CreateIndexPaths(int startingPosition, int count, nint section)
        {
            var indexPaths = new NSIndexPath[count];

            for (var i = 0; i < count; i++)
            {
                indexPaths[i] = NSIndexPath.FromRowSection(i + startingPosition, section);
            }

            return indexPaths;
        }

        private static NSIndexSet CreateIndexSet(int startingPosition, int count)
        {
            return NSIndexSet.FromNSRange(new NSRange(startingPosition, count));
        }

        private IEnumerable<object>? GetSectionItem(nint section)
        {
            return _items?.ElementAtOrDefault((int)section);
        }

        private object? GetItem(NSIndexPath indexPath)
        {
            indexPath = GetShiftedIndexPath(indexPath);
            
            return GetSectionItem(indexPath.Section)?.ElementAtOrDefault(indexPath.Row);
        }

        private NSIndexPath GetShiftedIndexPath(NSIndexPath indexPath)
        {
            var indexPaths = _expandedIndexPaths
                .Where(item => item.Section == indexPath.Section)
                .ToArray();

            if (indexPaths.Length == 0)
            {
                return indexPath;
            }

            var shift = indexPaths.Count(item => item.Row < indexPath.Row);

            return shift == 0 
                ? indexPath 
                : NSIndexPath.FromRowSection(indexPath.Row - shift, indexPath.Section);
        }

        protected virtual string GetCellReuseIdentifier(object item)
        {
            return item.GetType().Name;
        }
        
        protected virtual string GetExpandedCellReuseIdentifier(object item)
        {
            return item.GetType().Name;
        }

        protected virtual string? GetHeaderReuseIdentifier(object item)
        {
            return item.GetType().Name;
        }

        protected virtual string? GetFooterReuseIdentifier(object item)
        {
            return item.GetType().Name;
        }

        protected virtual object GetHeaderSectionItem(nint section, object sectionItem)
        {
            return sectionItem;
        }

        protected virtual object GetFooterSectionItem(nint section, object sectionItem)
        {
            return sectionItem;
        }

        private void ResetExpandableCells()
        {
            // TODO: Collapse all cells.

            _expandedIndexPaths.Clear();
        }
        
        private bool IsIndexPathForExpandedCell(NSIndexPath indexPath)
        {
            if (indexPath.Row == 0)
            {
                return false;
            }

            var shiftedIndexPath = NSIndexPath.FromRowSection(indexPath.Row - 1, indexPath.Section);

            return _expandedIndexPaths.Contains(shiftedIndexPath);
        }

        private bool HandleExpandableCell(UITableViewCell? cell, NSIndexPath indexPath)
        {
            if (cell == null)
            {
                return false;
            }

            if (!_expandableCellControllers.TryGetValue(cell.GetType(), out var controller))
            {
                return false;
            }

            return _expandedIndexPaths.Contains(indexPath)
                ? TryCollapse(controller, indexPath) 
                : TryExpand(controller, indexPath);
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private bool TryExpand(IExpandableCellController controller, NSIndexPath indexPath)
        {
            var shiftedIndexPath = NSIndexPath.FromRowSection(indexPath.Row + 1, indexPath.Section);

            _expandedIndexPaths.Add(indexPath);

            TableView.InsertRows(new[] { shiftedIndexPath }, UITableViewRowAnimation.Automatic);

            var sourceCell = TableView.CellAt(indexPath);
            var expandedCell = TableView.CellAt(shiftedIndexPath);

            if (sourceCell == null || expandedCell == null)
            {
                return true;
            }

            controller.OnExpanded(sourceCell, expandedCell);

            TableView.DeselectRow(indexPath, true);

            return true;
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private bool TryCollapse(IExpandableCellController controller, NSIndexPath indexPath)
        {
            var shiftedIndexPath = NSIndexPath.FromRowSection(indexPath.Row - 1, indexPath.Section);

            var sourceCell = TableView.CellAt(indexPath);
            var expandedCell = TableView.CellAt(shiftedIndexPath);

            if (sourceCell == null || expandedCell == null)
            {
                return false;
            }

            _expandedIndexPaths.Remove(shiftedIndexPath);

            controller.OnCollapsed(sourceCell, expandedCell);

            TableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Automatic);

            return true;
        }

        public sealed override nint NumberOfSections(UITableView tableView)
        {
            return _items?.Count() ?? 0;
        }

        public sealed override nint RowsInSection(UITableView tableView, nint section)
        {
            var rowsInSection = GetSectionItem((int)section)?.Count() ?? 0;

            var expandedIndexPathsCount = _expandedIndexPaths.Count(item => item.Section == section);

            return rowsInSection + expandedIndexPathsCount;
        }

        public sealed override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            var itemView = GetCell(indexPath, item);

            if (itemView == null)
            {
                throw new InvalidOperationException($"Can not cell for index path {indexPath}.");
            }

            return itemView;
        }

        protected virtual UITableViewCell GetCell(NSIndexPath indexPath, object item)
        {
            var reuseIdentifier = IsIndexPathForExpandedCell(indexPath)
                ? GetExpandedCellReuseIdentifier(item)
                : GetCellReuseIdentifier(item);

            if (reuseIdentifier == null)
            {
                throw new InvalidOperationException($"Can not get reuse identifier for index path {indexPath}.");
            }

            return TableView.DequeueReusableCell(reuseIdentifier, indexPath)!;
        }

        public sealed override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            WillDisplay(indexPath, cell, item);
        }

        protected virtual void WillDisplay(NSIndexPath indexPath, UITableViewCell cell, object item)
        {
            /* Nothing to do */
        }

        public sealed override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            return CanEditRow(indexPath, item);
        }

        protected virtual bool CanEditRow(NSIndexPath indexPath, object item)
        {
            return false;
        }

        public sealed override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath)
        {
            return EditingStyleForRow(tableView, indexPath) != UITableViewCellEditingStyle.None;
        }

        public sealed override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            return EditingStyleForRow(indexPath, item);
        }

        protected virtual UITableViewCellEditingStyle EditingStyleForRow(NSIndexPath indexPath, object item)
        {
            return UITableViewCellEditingStyle.None;
        }

        public sealed override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            CommitEditingStyle(editingStyle, indexPath, item);
        }

        protected virtual void CommitEditingStyle(UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath, object item)
        {
            /* Nothing to do */
        }

        public sealed override NSIndexPath WillSelectRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            return WillSelectRow(indexPath, item);
        }

        protected virtual NSIndexPath WillSelectRow(NSIndexPath indexPath, object item)
        {
            return indexPath;
        }

        public sealed override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            if (!HandleExpandableCell(TableView.CellAt(indexPath), indexPath))
            {
                if (ShouldBeAutomaticallyDeselected(indexPath, item))
                {
                    tableView.DeselectRow(indexPath, true);
                }
            }

            RowSelected(indexPath, item);
        }

        protected virtual bool ShouldBeAutomaticallyDeselected(NSIndexPath indexPath, object item)
        {
            return false;
        }

        protected virtual void RowSelected(NSIndexPath indexPath, object item)
        {
            /* Nothing to do */
        }

        public sealed override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);

            if (item == null)
            {
                throw new InvalidOperationException($"Can not get item for index path {indexPath}.");
            }

            AccessoryButtonTapped(indexPath, item);
        }

        protected virtual void AccessoryButtonTapped(NSIndexPath indexPath, object item)
        {
            /* Nothing to do */
        }

        public sealed override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var sectionItem = GetSectionItem(section);

            if (sectionItem == null)
            {
                throw new InvalidOperationException($"Can not get section item for section {section}.");
            }

            var headerSectionItem = GetHeaderSectionItem(section, sectionItem);

            return GetViewForHeader(section, headerSectionItem)!;
        }

        protected virtual UITableViewHeaderFooterView? GetViewForHeader(nint section, object sectionItem)
        {
            var reuseIdentifier = GetHeaderReuseIdentifier(sectionItem);

            if (reuseIdentifier == null)
            {
                return null;
            }

            var headerView = TableView.DequeueReusableHeaderFooterView(reuseIdentifier);

            if (headerView != null)
            {
                WillDisplayHeader(section, headerView, sectionItem);
            }

            return headerView;
        }

        protected virtual void WillDisplayHeader(nint section, UITableViewHeaderFooterView view, object item)
        {
            /* Nothing to do */
        }

        public sealed override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            var sectionItem = GetSectionItem(section);

            if (sectionItem == null)
            {
                throw new InvalidOperationException($"Can not get section item for section {section}.");
            }

            var footerSectionItem = GetFooterSectionItem(section, sectionItem);

            return GetViewForFooter(section, footerSectionItem)!;
        }

        protected virtual UITableViewHeaderFooterView? GetViewForFooter(nint section, object sectionItem)
        {
            var reuseIdentifier = GetFooterReuseIdentifier(sectionItem);

            if (reuseIdentifier == null)
            {
                return null;
            }
 
            var footerView = TableView.DequeueReusableHeaderFooterView(reuseIdentifier);

            if (footerView != null)
            {
                WillDisplayFooter(section, footerView, sectionItem);
            }

            return footerView;
        }

        protected virtual void WillDisplayFooter(nint section, UITableViewHeaderFooterView view, object item)
        {
            /* Nothing to do */
        }

        public sealed override string TitleForHeader(UITableView tableView, nint section)
        {
            if (GetViewForHeader(tableView, section) is UITableViewHeaderFooterView headerFooterView)
            {
                return headerFooterView.TextLabel.Text!;
            }

            return null!;
        }

        public sealed override string TitleForFooter(UITableView tableView, nint section)
        {
            if (GetViewForFooter(tableView, section) is UITableViewHeaderFooterView headerFooterView)
            {
                return headerFooterView.TextLabel.Text!;
            }

            return null!;
        }

        public void AddExpandableCellController<TCell>(IExpandableCellController expandableCellController)
            where TCell : UITableViewCell
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (expandableCellController == null)
            {
                throw new ArgumentNullException(nameof(expandableCellController));
            }

            _expandableCellControllers[typeof(TCell)] = expandableCellController;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                ClearSubscriptions();

                _sectionsSubscription = null;

                IsDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}