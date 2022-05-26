using System;
using System.ComponentModel;

namespace Buform
{
    public interface IFormItem : INotifyPropertyChanged, IDisposable
    {
        string? PropertyName { get; }

        bool IsReadOnly { get; set; }
        bool IsVisible { get; set; }

        object? Value { get; set; }

        event EventHandler<FormValueChangedEventArgs>? ValueChanged;
        event EventHandler? VisibilityChanged;

        void Initialize(Form form, object target);
    }
}