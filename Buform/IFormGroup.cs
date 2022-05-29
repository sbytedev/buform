using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Buform
{
    public interface IFormGroup : IEnumerable<IFormItem>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        IEnumerable<IFormItem> HiddenItems { get; }
        
        event EventHandler<FormValueChangedEventArgs>? ValueChanged;

        IFormItem? GetItem(string propertyName);
    }
}