using System.Threading;
using System.Threading.Tasks;

namespace Buform
{
    public interface IAsyncPickerFormItem : IPickerFormItemBase
    {
        AsyncPickerLoadingState State { get; }

        Task LoadItemsAsync(CancellationToken cancellationToken);
    }
}