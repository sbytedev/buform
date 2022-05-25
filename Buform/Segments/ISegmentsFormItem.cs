using System.Collections.Generic;

namespace Buform
{
    public interface ISegmentsFormItem : IValidatableFormItem
    {
        string? Label { get; }

        IEnumerable<ISegmentsOptionFormItem> Items { get; }
    }
}