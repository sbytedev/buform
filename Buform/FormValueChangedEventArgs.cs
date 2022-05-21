using System;

namespace Buform
{
    public sealed class FormValueChangedEventArgs : EventArgs
    {
        public string PropertyName { get; }

        public FormValueChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }
    }
}