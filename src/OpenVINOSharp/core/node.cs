using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public class Node
    {
        public enum NodeType
        {
            e_const = 0,
            e_nomal = 1
        };

        public IntPtr ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public NodeType node_type { get; set; }
        public Node(IntPtr ptr, NodeType type)
        {
            Ptr = ptr;
            this.node_type = type;
        }
        ~Node() {
            dispose();
        }

        public void dispose()
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }
            if (node_type == NodeType.e_const)
            {
                NativeMethods.ov_output_const_port_free(ptr);
            }
            else
            {
                NativeMethods.ov_output_port_free(ptr);
            }
            ptr = IntPtr.Zero;
        }
        public Shape get_shape()
        {
            ExceptionStatus status;

            int l = Marshal.SizeOf(typeof(Ov.ov_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            if (node_type == NodeType.e_const)
            {
                status = (ExceptionStatus)NativeMethods.ov_const_port_get_shape(ptr, shape_ptr);
            }
            else
            {
                status = (ExceptionStatus)NativeMethods.ov_port_get_shape(ptr, shape_ptr);
            }
            if (status != 0)
            {
                shape_ptr = IntPtr.Zero;
                System.Diagnostics.Debug.WriteLine("Node get_shape error : {0}!", status.ToString());
            }
            return new Shape(shape_ptr);
        }

        public PartialShape get_partial_shape()
        {
            int l = Marshal.SizeOf(typeof(Ov.ov_partial_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_port_get_partial_shape(ptr, shape_ptr);
            if (status != 0)
            {
                shape_ptr = IntPtr.Zero;
                System.Diagnostics.Debug.WriteLine("Node get_partial_shape error : {0}!", status.ToString());
            }
            return new PartialShape(shape_ptr);
        }

        public string get_name()
        {
            ExceptionStatus status;
            IntPtr s_ptr = IntPtr.Zero;
            status = (ExceptionStatus)NativeMethods.ov_port_get_any_name(ptr, ref s_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Node get_name error : {0}!", status.ToString());
            }
            string ss = Marshal.PtrToStringAnsi(s_ptr);
            return ss;
        }

        public element.Type get_type() 
        {
            ExceptionStatus status = 0;
            uint data_type = 0;
            if (node_type == NodeType.e_const)
            {
                status = (ExceptionStatus)NativeMethods.ov_port_get_element_type(ptr, ref data_type);
            }
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Node get_type error : {0}!", status.ToString());
            }
            return new element.Type((element.Type_t)data_type);
        }
    }
}
