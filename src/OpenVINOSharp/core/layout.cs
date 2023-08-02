using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.core
{
    public class Layout
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        public Layout(string layout_desc)
        {
            sbyte[] c_layout_desc = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(layout_desc));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_layout_create(ref c_layout_desc[0], ref m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Layout init error : {0}!", status.ToString());
            }
        }
        ~Layout() { dispose(); }
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_layout_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        public string to_string()
        {
            return NativeMethods.ov_layout_to_string(m_ptr);
        }
    }
}
