using System;
using System.Linq;
using Foundation;
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
    }
}