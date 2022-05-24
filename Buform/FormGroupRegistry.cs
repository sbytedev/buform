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
        private enum RegistrationType
        {
            Class,
            Nib
        }

        private enum HolderType
        {
            Header,
            Footer
        }

        private sealed class Holder
        {
            public Type ViewType { get; }
            public RegistrationType RegistrationType { get; }

            public Holder(Type viewType, RegistrationType registrationType)
            {
                ViewType = viewType ?? throw new ArgumentNullException(nameof(viewType));
                RegistrationType = registrationType;
            }
        }

        private readonly IDictionary<(Type, HolderType), Holder> _holders;

        public FormGroupRegistry()
        {
            _holders = new Dictionary<(Type, HolderType), Holder>();
        }

        private bool TryGetHolder(Type groupType, HolderType holderType, out Holder? holder)
        {
            if (_holders.TryGetValue((groupType, holderType), out holder))
            {
                return true;
            }

            var interfaceTypes = groupType.GetInterfaces().Except(
                groupType.GetInterfaces().SelectMany(item => item.GetInterfaces())
            );

            foreach (var interfaceType in interfaceTypes)
            {
                if (_holders.TryGetValue((interfaceType, holderType), out holder))
                {
                    return true;
                }
            }

            return false;
        }

        public void RegisterGroupHeaderClass<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            _holders[(typeof(TGroup), HolderType.Header)] = new Holder(
                typeof(TGroupView),
                RegistrationType.Class
            );
        }

        public void RegisterGroupFooterClass<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            _holders[(typeof(TGroup), HolderType.Footer)] = new Holder(
                typeof(TGroupView),
                RegistrationType.Class
            );
        }

        public void RegisterGroupHeaderNib<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            _holders[(typeof(TGroup), HolderType.Header)] = new Holder(
                typeof(TGroupView),
                RegistrationType.Nib
            );
        }

        public void RegisterGroupFooterNib<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            _holders[(typeof(TGroup), HolderType.Footer)] = new Holder(
                typeof(TGroupView),
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
                        tableView.RegisterClassForHeaderFooterViewReuse(holder.ViewType, holder.ViewType.Name);
                        break;
                    case RegistrationType.Nib:
                        tableView.RegisterNibForHeaderFooterViewReuse(
                            UINib.FromName(holder.ViewType.Name, NSBundle.MainBundle),
                            holder.ViewType.Name
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

            var result = TryGetHolder(groupType, HolderType.Header, out var holder);

            if (result)
            {
                reuseIdentifier = holder!.ViewType.Name;

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

            var result = TryGetHolder(groupType, HolderType.Footer, out var holder);

            if (result)
            {
                reuseIdentifier = holder!.ViewType.Name;

                return true;
            }

            reuseIdentifier = null;

            return false;
        }
    }
}