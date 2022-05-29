using System;
using System.Linq;
using Foundation;
using SByteDev.Common.Extensions;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    public sealed class FormTableViewSource : TableViewSource
    {
        public Form? Form
        {
            get => Items as Form;
            set => Items = value;
        }

        public FormTableViewSource(UITableView tableView) : base(tableView)
        {
            Buform.Register(TableView);
        }

        private IFormItem? GetItem(NSIndexPath indexPath)
        {
            return Form?.ElementAtOrDefault(indexPath.Section)?.ElementAtOrDefault(indexPath.Row);
        }

        private IFormGroup? GetGroup(nint section)
        {
            return Form?.ElementAtOrDefault((int)section);
        }

        private FormCell? FindCell(NSIndexPath indexPath)
        {
            var formItem = GetItem(indexPath);

            if (formItem == null)
            {
                return GetCell(TableView, indexPath) as FormCell;
            }

            if (TableView.CellAt(indexPath) is not FormCell formCell)
            {
                return GetCell(TableView, indexPath) as FormCell;
            }

            if (!ReferenceEquals(formCell.FormItem, formItem))
            {
                return GetCell(TableView, indexPath) as FormCell;
            }

            return formCell;
        }

        protected override string? GetHeaderReuseIdentifier(object item)
        {
            return Buform.TryGetHeaderReuseIdentifier(item.GetType(), out var reuseIdentifier) ? reuseIdentifier : null;
        }

        protected override string? GetFooterReuseIdentifier(object item)
        {
            return Buform.TryGetFooterReuseIdentifier(item.GetType(), out var reuseIdentifier) ? reuseIdentifier : null;
        }

        protected override string GetCellReuseIdentifier(object item)
        {
            if (!Buform.TryGetReuseIdentifier(item.GetType(), out var reuseIdentifier))
            {
                throw new FormViewNotFoundException(item);
            }

            return reuseIdentifier!;
        }

        protected override string GetExpandedCellReuseIdentifier(object item)
        {
            if (!Buform.TryGetExpandedReuseIdentifier(item.GetType(), out var reuseIdentifier))
            {
                throw new FormViewNotFoundException(item);
            }

            return reuseIdentifier!;
        }

        protected override UITableViewCell GetCell(NSIndexPath indexPath, object item)
        {
            var cell = base.GetCell(indexPath, item);

            if (cell is not FormCell formCell)
            {
                return cell;
            }

            if (item is not IFormItem formItem)
            {
                return cell;
            }

            formCell.Initialize(formItem);

            return cell;
        }

        protected override void WillDisplayHeader(nint section, UITableViewHeaderFooterView view, object item)
        {
            base.WillDisplayHeader(section, view, item);

            if (view is not FormHeaderFooterView headerFooterView)
            {
                return;
            }

            if (item is not IFormGroup formGroup)
            {
                return;
            }

            headerFooterView.Initialize(formGroup);
        }

        protected override void WillDisplayFooter(nint section, UITableViewHeaderFooterView view, object item)
        {
            base.WillDisplayFooter(section, view, item);

            if (view is not FormHeaderFooterView headerFooterView)
            {
                return;
            }

            if (item is not IFormGroup formGroup)
            {
                return;
            }

            headerFooterView.Initialize(formGroup);
        }

        protected override NSIndexPath WillSelectRow(NSIndexPath indexPath, object item)
        {
            return FindCell(indexPath)?.IsSelectable ?? false ? indexPath : null!;
        }

        protected override bool ShouldBeAutomaticallyDeselected(NSIndexPath indexPath, object item)
        {
            return FindCell(indexPath)?.IsSelectable ?? false;
        }

        protected override void RowSelected(NSIndexPath indexPath, object item)
        {
            FindCell(indexPath)?.OnSelected();
        }

        protected override bool CanEditRow(NSIndexPath indexPath, object item)
        {
            if (item is not IFormItem formItem)
            {
                return false;
            }

            var group = GetGroup(indexPath.Section);

            if (group == null)
            {
                return false;
            }

            if (group.RemoveCommand.SafeCanExecute(formItem.Value))
            {
                return true;
            }

            if (group.InsertCommand.SafeCanExecute(formItem.Value))
            {
                return true;
            }

            return false;
        }

        protected override UITableViewCellEditingStyle EditingStyleForRow(NSIndexPath indexPath, object item)
        {
            if (item is not IFormItem formItem)
            {
                return UITableViewCellEditingStyle.None;
            }

            var group = GetGroup(indexPath.Section);

            if (group == null)
            {
                return UITableViewCellEditingStyle.None;
            }

            if (group.RemoveCommand.SafeCanExecute(formItem.Value))
            {
                return UITableViewCellEditingStyle.Delete;
            }

            if (group.InsertCommand.SafeCanExecute(formItem.Value))
            {
                return UITableViewCellEditingStyle.Insert;
            }

            return UITableViewCellEditingStyle.None;
        }

        protected override void CommitEditingStyle(UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath, object item)
        {
            if (item is not IFormItem formItem)
            {
                return;
            }

            var group = GetGroup(indexPath.Section);

            if (group == null)
            {
                return;
            }

            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.None:
                    return;
                case UITableViewCellEditingStyle.Delete:
                    group.RemoveCommand.SafeExecute(formItem.Value);
                    break;
                case UITableViewCellEditingStyle.Insert:
                    group.InsertCommand.SafeExecute(formItem.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(editingStyle), editingStyle, null);
            }
        }

        protected override bool CanMoveRow(NSIndexPath indexPath, object item)
        {
            if (item is not IFormItem formItem)
            {
                return false;
            }

            var group = GetGroup(indexPath.Section);

            return group != null && group.MoveCommand.SafeCanExecute(formItem.Value);
        }

        protected override void MoveRow(NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath, object item)
        {
            GetGroup(sourceIndexPath.Section)?.MoveCommand.SafeExecute((sourceIndexPath.Row, destinationIndexPath.Row));
        }
    }
}