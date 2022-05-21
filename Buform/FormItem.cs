using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Buform
{
    public abstract class FormItem<TValue> : IFormItem
    {
        private PropertyInfo? _propertyInfo;
        private Expression<Func<TValue>>? _property;
        private TValue? _value;
        private bool _isReadOnly;
        private bool _shouldSkipValueChangedCallback;

        protected Form? Form { get; private set; }

        public object? Target { get; private set; }

        public string? PropertyName { get; }

        public virtual bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;

                NotifyPropertyChanged();
            }
        }

        object? IFormItem.Value
        {
            get => Value;
            set => Value = (TValue?)value;
        }

        public TValue? Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        protected virtual bool IsValueChanged { get; set; }

        public virtual Action<Form, TValue?>? ValueChangedCallback { get; set; }

        public event EventHandler<FormValueChangedEventArgs>? ValueChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected FormItem(Expression<Func<TValue>> property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));

            PropertyName = _property.GetMemberName();
        }

        protected FormItem(TValue value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private TValue? GetValue()
        {
            if (_propertyInfo == null)
            {
                return _value;
            }

            return Target == null ? default : (TValue?)_propertyInfo.GetValue(Target);
        }

        private void SetValue(TValue? value)
        {
            if (Target == null)
            {
                return;
            }

            if (!Equals(value, Value))
            {
                IsValueChanged = true;
            }

            _shouldSkipValueChangedCallback = true;

            _propertyInfo?.SetValue(Target, value);

            _shouldSkipValueChangedCallback = false;

            OnValueChanged();
        }

        private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != PropertyName && !string.IsNullOrWhiteSpace(e.PropertyName))
            {
                return;
            }

            if (_shouldSkipValueChangedCallback)
            {
                return;
            }

            OnValueChanged();
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnValueChanged()
        {
            if (Form == null)
            {
                return;
            }

            NotifyPropertyChanged(nameof(Value));

            ValueChanged?.Invoke(this, new FormValueChangedEventArgs(PropertyName ?? string.Empty));

            ValueChangedCallback?.Invoke(Form, Value);
        }

        public void Initialize(Form form, object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            Form = form ?? throw new ArgumentNullException(nameof(form));

            if (Target is INotifyPropertyChanged oldNotifyPropertyChanged)
            {
                oldNotifyPropertyChanged.PropertyChanged -= OnTargetPropertyChanged;
            }

            Target = target;

            if (Target is INotifyPropertyChanged newNotifyPropertyChanged)
            {
                newNotifyPropertyChanged.PropertyChanged += OnTargetPropertyChanged;
            }

            _propertyInfo = Target?.GetType().GetProperty(PropertyName ?? string.Empty);

            OnValueChanged();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing)
            {
                return;
            }

            _propertyInfo = null;
            _property = null;

            if (Target is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= OnTargetPropertyChanged;
            }

            Target = null;

            _value = default;
        }
    }
}