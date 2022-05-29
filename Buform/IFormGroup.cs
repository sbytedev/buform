using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace Buform
{
    public interface IFormGroup : IEnumerable<IFormItem>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        ICommand? RemoveCommand { get; }
        ICommand? MoveCommand { get; }
        ICommand? InsertCommand { get; }

        IEnumerable<IFormItem> HiddenItems { get; }
        
        event EventHandler<FormValueChangedEventArgs>? ValueChanged;

        IFormItem? GetItem(string propertyName);
    }
}