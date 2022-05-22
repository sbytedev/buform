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

            Register();

            FormComponentRegistry.Register();
        }

        private static void Register()
        {
            RegisterHeaderClass<TextFormGroup, TextFormGroupHeader, TextFormGroupFooter>();
            RegisterHeaderClass<IListFormGroup, ListFormGroupHeader, ListFormGroupFooter>();

            RegisterClass<ButtonFormItem, ButtonFormCell>();
            RegisterClass<DateTimeFormItem, DateTimeFormCell>();
            RegisterClass<IAsyncPickerFormItem, AsyncPickerFormCell>();
            RegisterClass<ICallbackPickerFormItem, CallbackPickerFormCell>();
            RegisterClass<IListFormItem, ListFormCell>();
            RegisterClass<IMultilineTextFormItem, MultilineTextFormCell>();
            RegisterClass<IMultiValuePickerFormItem, MultiValuePickerFormCell>();
            RegisterClass<IPickerFormItem, PickerFormCell>();
            RegisterClass<ISegmentsFormItem, SegmentsFormCell>();
            RegisterClass<ITextFormItem, TextFormCell>();
            RegisterClass<SliderFormItem, SliderFormCell>();
            RegisterClass<StepperFormItem, StepperFormCell>();
            RegisterClass<SwitchFormItem, SwitchFormCell>();
        }

        public static void RegisterHeaderClass<TGroup, TGroupHeader, TGroupFooter>()
            where TGroup : class, IFormGroup
            where TGroupHeader : FormHeaderFooter<TGroup>
            where TGroupFooter : FormHeaderFooter<TGroup>
        {
            GroupRegistry.RegisterHeaderClass<TGroup, TGroupHeader, TGroupFooter>();
        }

        public static void RegisterHeaderNib<TGroup, TGroupHeader, TGroupFooter>()
            where TGroup : class, IFormGroup
            where TGroupHeader : FormHeaderFooter<TGroup>
            where TGroupFooter : FormHeaderFooter<TGroup>
        {
            GroupRegistry.RegisterHeaderNib<TGroup, TGroupHeader, TGroupFooter>();
        }
        
        public static void RegisterClass<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            ItemRegistry.RegisterClass<TItem, TItemView>();
        }

        public static void RegisterClass<TItem, TItemView, TExpandedItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
            where TExpandedItemView : FormCell<TItem>
        {
            ItemRegistry.RegisterClass<TItem, TItemView, TExpandedItemView>();
        }

        public static void RegisterNib<TItem, TItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
        {
            ItemRegistry.RegisterNib<TItem, TItemView>();
        }

        public static void RegisterNib<TItem, TItemView, TExpandedItemView>()
            where TItem : class, IFormItem
            where TItemView : FormCell<TItem>
            where TExpandedItemView : FormCell<TItem>
        {
            ItemRegistry.RegisterNib<TItem, TItemView, TExpandedItemView>();
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