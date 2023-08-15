using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenVinoSharp
{
    /// <summary>
    /// This class represents an abstraction for remote (non-CPU) accelerator device-specific inference context.
    /// Such context represents a scope on the device within which compiled models and remote memory tensors can exist, 
    /// function, and exchange data.
    /// </summary>
    public class RemoteContext
    {
        /// <summary>
        /// [private]RemoteContext class pointer.
        /// </summary>
        private IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]RemoteContext class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="ptr">RemoteContext pointer.</param>
        public RemoteContext(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("RemoteContext init error : ptr is null!");
                return;
            }
            Ptr = ptr;
        }
    }
}
