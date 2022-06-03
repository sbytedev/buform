using System;

namespace Buform
{
    public static class BuformForms
    {
        private static readonly FormGroupRegistry GroupRegistry;

        static BuformForms()
        {
            GroupRegistry = new FormGroupRegistry();

            FormComponentRegistry.Register();
        }

        public static void RegisterGroupHeaderClass<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormGroupView<TGroup>
        {
            GroupRegistry.RegisterGroupHeaderClass<TGroup, TGroupView>();
        }

        public static void RegisterGroupFooterClass<TGroup, TGroupView>()
            where TGroup : class, IFormGroup
            where TGroupView : FormGroupView<TGroup>
        {
            GroupRegistry.RegisterGroupFooterClass<TGroup, TGroupView>();
        }

        public static bool TryGetHeaderViewType(Type groupType, out Type? viewType)
        {
            return GroupRegistry.TryGetHeaderViewType(groupType, out viewType);
        }

        public static bool TryGetFooterViewType(Type groupType, out Type? viewType)
        {
            return GroupRegistry.TryGetFooterViewType(groupType, out viewType);
        }
    }
}