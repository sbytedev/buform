namespace Buform
{
    public sealed class TextFormGroup : FormGroup<IFormItem>
    {
        private string? _headerLabel;
        private string? _footerLabel;

        public string? HeaderLabel
        {
            get => _headerLabel;
            set
            {
                _headerLabel = value;

                NotifyPropertyChanged();
            }
        }

        public string? FooterLabel
        {
            get => _footerLabel;
            set
            {
                _footerLabel = value;

                NotifyPropertyChanged();
            }
        }

        public TextFormGroup(string? headerLabel = null, string? footerLabel = null)
        {
            HeaderLabel = headerLabel;
            FooterLabel = footerLabel;
        }
    }
}