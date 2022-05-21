using System;

namespace Buform
{
    public sealed class FormViewNotFoundException : Exception
    {
        public FormViewNotFoundException(object item) : base($"No view registered for {item.GetType()}.")
        {
            /* Required constructor */
        }
    }
}