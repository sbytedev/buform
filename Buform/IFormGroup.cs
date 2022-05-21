using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Buform
{
    public interface IFormGroup : IEnumerable<IFormItem>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        event EventHandler<FormValueChangedEventArgs>? ValueChanged;
    }
}