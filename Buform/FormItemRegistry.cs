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
        private enum RegistrationType
        {
            Class,
            Nib
        }

        private sealed class Holder
        {
            public Type CellType { get; }
            public Type? ExpandedCellType { get; }

            public RegistrationType RegistrationType { get; }

            public Holder(Type cellType, Type? expandedCellType, RegistrationType registrationType)
            {
                CellType = cellType ?? throw new ArgumentNullException(nameof(cellType));
                ExpandedCellType = expandedCellType;
                RegistrationType = registrationType;
            }
        }

        private readonly IDictionary<Type, Holder> _holders;

        public FormItemRegistry()
        {
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

        public void RegisterItemClass<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            _holders[typeof(TItem)] = new Holder(
                typeof(TItemView),
                null,
                RegistrationType.Class
            );
        }

        public void RegisterItemClass<TItem, TItemView, TExpandedItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
            where TExpandedItemView : FormCell<TItem>
        {
            _holders[typeof(TItem)] = new Holder(
                typeof(TItemView),
                typeof(TExpandedItemView),
                RegistrationType.Class
            );
        }

        public void RegisterItemNib<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            _holders[typeof(TItem)] = new Holder(
                typeof(TItemView),
                null,
                RegistrationType.Nib
            );
        }

        public void RegisterItemNib<TItem, TItemView, TExpandedItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
            where TExpandedItemView : FormCell<TItem>
        {
            _holders[typeof(TItem)] = new Holder(
                typeof(TItemView),
                typeof(TExpandedItemView),
                RegistrationType.Nib
            );
        }

        public void Register(UITableView tableView)
        {
            if (tableView == null)
            {
                throw new ArgumentNullException(nameof(tableView));
            }

            foreach (var holder in _holders.Values)
            {
                switch(holder.RegistrationType)
                {
                    case RegistrationType.Class:
                        tableView.RegisterClassForCellReuse(holder.CellType, holder.CellType.Name);
                        if (holder.ExpandedCellType != null)
                        {
                            tableView.RegisterClassForCellReuse(holder.ExpandedCellType, holder.ExpandedCellType.Name);
                        }
                        break;
                    case RegistrationType.Nib:
                        tableView.RegisterNibForCellReuse(
                            UINib.FromName(holder.CellType.Name, NSBundle.MainBundle),
                            holder.CellType.Name
                        );
                        if (holder.ExpandedCellType != null)
                        {
                            tableView.RegisterNibForCellReuse(
                                UINib.FromName(holder.ExpandedCellType.Name, NSBundle.MainBundle),
                                holder.ExpandedCellType.Name
                            );
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
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
                reuseIdentifier = holder!.CellType.Name;

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
                reuseIdentifier = holder!.ExpandedCellType?.Name;

                return reuseIdentifier != null;
            }

            reuseIdentifier = null;

            return false;
        }
    }
}