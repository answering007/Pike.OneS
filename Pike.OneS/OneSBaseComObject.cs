using System;
using System.Runtime.InteropServices;

namespace Pike.OneS
{
    /// <summary>
    /// Base disposable COM object
    /// </summary>
    public abstract class OneSBaseComObject: IDisposable
    {
        /// <summary>
        /// COM object
        /// </summary>
        public dynamic ComObject { get; protected set; }

        /// <summary>
        /// True if <see cref="ComObject"/> is not null; otherwise false
        /// </summary>
        public bool HasObject => ComObject != null;

        /// <summary>
        /// <see cref="IDisposable"/> interface implementation
        /// </summary>
        public virtual void Dispose()
        {
            if (!HasObject) return;

            Marshal.FinalReleaseComObject(ComObject);
            ComObject = null;
        }
    }
}
