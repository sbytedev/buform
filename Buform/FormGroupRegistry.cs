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
        private sealed class Holder
        {
            public string HeaderReuseIdentifier { get; }
            public string FooterReuseIdentifier { get; }

            public Holder(string headerReuseIdentifier, string footerReuseIdentifier)
            {
                HeaderReuseIdentifier = headerReuseIdentifier ?? throw new ArgumentNullException(nameof(headerReuseIdentifier));
                FooterReuseIdentifier = footerReuseIdentifier ?? throw new ArgumentNullException(nameof(footerReuseIdentifier));
            }
        }

        private readonly UITableView _tableView;

        private readonly IDictionary<Type, Holder> _holders;

        public FormGroupRegistry(UITableView tableView)
        {
            _tableView = tableView ?? throw new ArgumentNullException(nameof(tableView));

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
            var holder = new Holder(
                typeof(TGroupHeader).Name,
                typeof(TGroupFooter).Name
            );

            _tableView.RegisterClassForHeaderFooterViewReuse(typeof(TGroupHeader), holder.HeaderReuseIdentifier);
            _tableView.RegisterClassForHeaderFooterViewReuse(typeof(TGroupFooter), holder.FooterReuseIdentifier);

            _holders[typeof(TGroup)] = holder;
        }

        public void RegisterHeaderNib<TGroup, TGroupHeader, TGroupFooter>()
            where TGroup : class, IFormGroup
            where TGroupHeader : FormHeaderFooter<TGroup>
            where TGroupFooter : FormHeaderFooter<TGroup>
        {
            var holder = new Holder(
                typeof(TGroupHeader).Name,
                typeof(TGroupFooter).Name
            );

            _tableView.RegisterNibForHeaderFooterViewReuse(
                UINib.FromName(typeof(TGroupHeader).Name, NSBundle.MainBundle),
                holder.HeaderReuseIdentifier
            );

            _tableView.RegisterNibForHeaderFooterViewReuse(
                UINib.FromName(typeof(TGroupFooter).Name, NSBundle.MainBundle),
                holder.FooterReuseIdentifier
            );

            _holders[typeof(TGroup)] = holder;
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
                reuseIdentifier = holder!.HeaderReuseIdentifier;

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
                reuseIdentifier = holder!.FooterReuseIdentifier;

                return true;
            }

            reuseIdentifier = null;

            return false;
        }
    }
}