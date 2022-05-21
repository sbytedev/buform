using System;
using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    public sealed class TextFormGroupHeader : FormHeaderFooter<TextFormGroup>
    {
        public TextFormGroupHeader()
        {
            /* Required constructor */
        }

        public TextFormGroupHeader(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }

        private void UpdateLabel()
        {
            var tableView = GetTableView();

            tableView?.BeginUpdates();

            TextLabel.Text = Group?.HeaderLabel;

            tableView?.EndUpdates();
        }

        protected override void OnGroupSet()
        {
            UpdateLabel();
        }

        protected override void OnGroupPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Group.HeaderLabel):
                    UpdateLabel();
                    break;
            }
        }
    }
}