using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Buform
{
    public class FormCollection<TItem> : ObservableCollection<TItem>, IDisposable
    {
        private bool _isDisposed;

        public event EventHandler<FormValueChangedEventArgs>? ValueChanged;

        protected virtual void NotifyValueChanged(FormValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }
    }
}