using System;
using System.Collections.Specialized;

namespace Buform
{
    public sealed class WeakEventSubscription : IDisposable
    {
        private readonly WeakReference<INotifyCollectionChanged> _weakReference;
        private readonly NotifyCollectionChangedEventHandler _eventHandler;

        private bool _isSubscribed;

        public WeakEventSubscription(
            INotifyCollectionChanged target,
            NotifyCollectionChangedEventHandler eventHandler
        )
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (eventHandler == null)
            {
                throw new ArgumentNullException(nameof(eventHandler));
            }
            
            _weakReference = new WeakReference<INotifyCollectionChanged>(target);
            _eventHandler = eventHandler;

            Subscribe();
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed)
            {
                return;
            }

            if (!_weakReference.TryGetTarget(out var notifyCollectionChanged))
            {
                return;
            }

            notifyCollectionChanged.CollectionChanged -= _eventHandler;

            _isSubscribed = false;
        }

        private void Subscribe()
        {
            if (_isSubscribed)
            {
                return;
            }

            if (!_weakReference.TryGetTarget(out var notifyCollectionChanged))
            {
                return;
            }

            notifyCollectionChanged.CollectionChanged += _eventHandler;

            _isSubscribed = true;
        }

        public void Dispose()
        {
            Unsubscribe();

            // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(this);
        }
    }
}