using System;
using CoreGraphics;
using UIKit;

namespace Buform
{
    public static class Buform
    {
        private static readonly FormGroupRegistry GroupRegistry;
        private static readonly FormItemRegistry ItemRegistry;

        static Buform()
        {
            GroupRegistry = new FormGroupRegistry();
            ItemRegistry = new FormItemRegistry();

            FormComponentRegistry.Register();
        }

        public static void RegisterGroupHeaderClass<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            GroupRegistry.RegisterGroupHeaderClass<TGroup, TGroupView>();
        }

        public static void RegisterGroupFooterClass<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            GroupRegistry.RegisterGroupFooterClass<TGroup, TGroupView>();
        }

        public static void RegisterGroupHeaderNib<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            GroupRegistry.RegisterGroupHeaderNib<TGroup, TGroupView>();
        }

        public static void RegisterGroupFooterNib<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormHeaderFooter<TGroup>
        {
            GroupRegistry.RegisterGroupFooterNib<TGroup, TGroupView>();
        }

        public static void RegisterItemClass<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            ItemRegistry.RegisterItemClass<TItem, TItemView>();
        }

        public static void RegisterItemNib<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            ItemRegistry.RegisterItemNib<TItem, TItemView>();
        }

        public static void Register(UITableView tableView)
        {
            GroupRegistry.Register(tableView);
            ItemRegistry.Register(tableView);
        }

        public static bool TryGetHeaderReuseIdentifier(Type groupType, out string? reuseIdentifier)
        {
            return GroupRegistry.TryGetHeaderReuseIdentifier(groupType, out reuseIdentifier);
        }

        public static bool TryGetFooterReuseIdentifier(Type groupType, out string? reuseIdentifier)
        {
            return GroupRegistry.TryGetFooterReuseIdentifier(groupType, out reuseIdentifier);
        }

        public static bool TryGetReuseIdentifier(Type itemType, out string? reuseIdentifier)
        {
            return ItemRegistry.TryGetReuseIdentifier(itemType, out reuseIdentifier);
        }

        public static bool TryGetExpandedReuseIdentifier(Type itemType, out string? reuseIdentifier)
        {
            return ItemRegistry.TryGetExpandedReuseIdentifier(itemType, out reuseIdentifier);
        }

        public static class Texts
        {
            public static string Clear = "Clear";
            public static string Cancel = "Cancel";
        }

        public static class Picker
        {
            public static CGSize MinimumPopUpSize = new(240, 320);
        }
    }
}