using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Buform
{
    [Preserve(AllMembers = true)]
    public sealed class FormGroupRegistry
    {
        private enum HolderType
        {
            Class,
            Nib
        }

        private sealed class Holder
        {
            public Type HeaderType { get; }
            public Type FooterType { get; }

            public HolderType Type { get; }

            public Holder(Type headerType, Type footerType, HolderType type)
            {
                HeaderType = headerType ?? throw new ArgumentNullException(nameof(headerType));
                FooterType = footerType ?? throw new ArgumentNullException(nameof(footerType));
                Type = type;
            }
        }

        private readonly IDictionary<Type, Holder> _holders;

        public FormGroupRegistry()
        {
            _holders = new Dictionary<Type, Holder>();
        }

        private bool TryGetHolder(Type groupType, out Holder? holder)
        {
            if (_holders.TryGetValue(groupType, out holder))
            {
                return true;
            }

            var interfaceTypes = groupType.GetInterfaces().Except(
                groupType.GetInterfaces().SelectMany(item => item.GetInterfaces())
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

        public void RegisterHeaderClass<TGroup, TGroupHeader, TGroupFooter>()
            where TGroup : class, IFormGroup
            where TGroupHeader : FormHeaderFooter<TGroup>
            where TGroupFooter : FormHeaderFooter<TGroup>
        {
            _holders[typeof(TGroup)] = new Holder(
                typeof(TGroupHeader),
                typeof(TGroupFooter),
                HolderType.Class
            );
        }

        public void RegisterHeaderNib<TGroup, TGroupHeader, TGroupFooter>()
            where TGroup : class, IFormGroup
            where TGroupHeader : FormHeaderFooter<TGroup>
            where TGroupFooter : FormHeaderFooter<TGroup>
        {
            _holders[typeof(TGroup)] = new Holder(
                typeof(TGroupHeader),
                typeof(TGroupFooter),
                HolderType.Nib
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
                switch(holder.Type)
                {
                    case HolderType.Class:
                        tableView.RegisterClassForHeaderFooterViewReuse(holder.HeaderType, holder.HeaderType.Name);
                        tableView.RegisterClassForHeaderFooterViewReuse(holder.FooterType, holder.FooterType.Name);
                        break;
                    case HolderType.Nib:
                        tableView.RegisterNibForHeaderFooterViewReuse(
                            UINib.FromName(holder.HeaderType.Name, NSBundle.MainBundle),
                            holder.HeaderType.Name
                        );
                        tableView.RegisterNibForHeaderFooterViewReuse(
                            UINib.FromName(holder.FooterType.Name, NSBundle.MainBundle),
                            holder.FooterType.Name
                        );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool TryGetHeaderReuseIdentifier(Type groupType, out string? reuseIdentifier)
        {
            if (groupType == null)
            {
                throw new ArgumentNullException(nameof(groupType));
            }

            var result = TryGetHolder(groupType, out var holder);

            if (result)
            {
                reuseIdentifier = holder!.HeaderType.Name;

                return true;
            }

            reuseIdentifier = null;

            return false;
        }

        public bool TryGetFooterReuseIdentifier(Type groupType, out string? reuseIdentifier)
        {
            if (groupType == null)
            {
                throw new ArgumentNullException(nameof(groupType));
            }

            var result = TryGetHolder(groupType, out var holder);

            if (result)
            {
                reuseIdentifier = holder!.FooterType.Name;

                return true;
            }

            reuseIdentifier = null;

            return false;
        }
    }
}