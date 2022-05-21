using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    public sealed class FormItemRegistry
    {
        private sealed class Holder
        {
            public string ReuseIdentifier { get; }
            public string? ExpandedReuseIdentifier { get; }

            public Holder(string reuseIdentifier, string? expandedReuseIdentifier = default)
            {
                ReuseIdentifier = reuseIdentifier ?? throw new ArgumentNullException(nameof(reuseIdentifier));

                ExpandedReuseIdentifier = expandedReuseIdentifier;
            }
        }

        private readonly UITableView _tableView;

        private readonly IDictionary<Type, Holder> _holders;

        public FormItemRegistry(UITableView tableView)
        {
            _tableView = tableView ?? throw new ArgumentNullException(nameof(tableView));

            _holders = new Dictionary<Type, Holder>();
        }

        private bool TryGetHolder(Type itemType, out Holder? holder)
        {
            if (_holders.TryGetValue(itemType, out holder))
            {
                return true;
            }

            var interfaceTypes = itemType.GetInterfaces().Except(
                itemType.GetInterfaces().SelectMany(item => item.GetInterfaces())
            );

            foreach (var interfaceType in interfaceTypes)
            {
                if (_holders.TryGetValue(interfaceType, out holder))
                {
                    return true;
                }
            }

            return false;
        }

        public void RegisterClass<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            var holder = new Holder(typeof(TItemView).Name);

            _tableView.RegisterClassForCellReuse(typeof(TItemView), holder.ReuseIdentifier);

            _holders[typeof(TItem)] = holder;
        }

        public void RegisterClass<TItem, TItemView, TExpandedItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
            where TExpandedItemView : FormCell<TItem>
        {
            var holder = new Holder(typeof(TItemView).Name, typeof(TExpandedItemView).Name);

            _tableView.RegisterClassForCellReuse(typeof(TItemView), holder.ReuseIdentifier);
            _tableView.RegisterClassForCellReuse(typeof(TExpandedItemView), holder.ExpandedReuseIdentifier!);

            _holders[typeof(TItem)] = holder;
        }

        public void RegisterNib<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            var holder = new Holder(typeof(TItemView).Name);

            _tableView.RegisterNibForCellReuse(
                UINib.FromName(typeof(TItemView).Name, NSBundle.MainBundle),
                holder.ReuseIdentifier
            );

            _holders[typeof(TItem)] = holder;
        }

        public void RegisterNib<TItem, TItemView, TExpandedItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
            where TExpandedItemView : FormCell<TItem>
        {
            var holder = new Holder(typeof(TItemView).Name, typeof(TExpandedItemView).Name);

            _tableView.RegisterNibForCellReuse(
                UINib.FromName(typeof(TItemView).Name, NSBundle.MainBundle),
                holder.ReuseIdentifier
            );

            _tableView.RegisterNibForCellReuse(
                UINib.FromName(typeof(TExpandedItemView).Name, NSBundle.MainBundle),
                holder.ExpandedReuseIdentifier!
            );

            _holders[typeof(TItem)] = holder;
        }

        public bool TryGetReuseIdentifier(Type itemType, out string? reuseIdentifier)
        {
            if (itemType == null)
            {
                throw new ArgumentNullException(nameof(itemType));
            }

            var result = TryGetHolder(itemType, out var holder);

            if (result)
            {
                reuseIdentifier = holder!.ReuseIdentifier;

                return true;
            }

            reuseIdentifier = null;

            return false;
        }

        public bool TryGetExpandedReuseIdentifier(Type itemType, out string? reuseIdentifier)
        {
            if (itemType == null)
            {
                throw new ArgumentNullException(nameof(itemType));
            }

            var result = TryGetHolder(itemType, out var holder);

            if (result)
            {
                reuseIdentifier = holder!.ExpandedReuseIdentifier;

                return reuseIdentifier != null;
            }

            reuseIdentifier = null;

            return false;
        }
    }
}