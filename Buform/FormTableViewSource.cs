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
            if (!Buform.TryGetHeaderReuseIdentifier(item.GetType(), out var reuseIdentifier))
            {
                throw new FormViewNotFoundException(item);
            }

            return reuseIdentifier;
        }

        protected override string? GetFooterReuseIdentifier(object item)
        {
            if (!Buform.TryGetFooterReuseIdentifier(item.GetType(), out var reuseIdentifier))
            {
                throw new FormViewNotFoundException(item);
            }

            return reuseIdentifier;
        }

        protected override string? GetCellReuseIdentifier(object item)
        {
            if (!Buform.TryGetReuseIdentifier(item.GetType(), out var reuseIdentifier))
            {
                throw new FormViewNotFoundException(item);
            }

            return reuseIdentifier;
        }

        protected override string? GetExpandedCellReuseIdentifier(object item)
        {
            if (!Buform.TryGetExpandedReuseIdentifier(item.GetType(), out var reuseIdentifier))
            {
                throw new FormViewNotFoundException(item);
            }

            return reuseIdentifier;
        }

        protected override void Update(UIView view, object item)
        {
            switch (itemView: view, item)
            {
                case (FormCell formCell, IFormItem formItem):
                    formCell.SetItem(formItem);
                    break;
                case (FormHeaderFooterView headerFooterView, IFormGroup formGroup):
                    headerFooterView.SetGroup(formGroup);
                    break;
            }
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