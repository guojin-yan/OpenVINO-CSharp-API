using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// Nodes are the backbone of the graph of Value dataflow. Every node has 
    /// zero or more nodes as arguments and one value, which is either a tensor 
    /// or a (possibly empty) tuple of values.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// The node type.
        /// </summary>
        public enum NodeType
        {
            /// <summary>
            /// Const type.
            /// </summary>
            e_const = 0,
            /// <summary>
            /// Nomal type.
            /// </summary>
            e_nomal = 1
        };
        /// <summary>
        /// [private]Node class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]Node class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Specify the format type of the node.
        /// </summary>
        public NodeType node_type { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="ptr">The pointer of node.</param>
        /// <param name="type">The type of node.</param>
        public Node(IntPtr ptr, NodeType type)
        {
            Ptr = ptr;
            this.node_type = type;
        }
        /// <summary>
        /// Default deconstruction.
        /// </summary>
        ~Node() {
            dispose();
        }
        /// <summary>
        /// Release unmanaged resources.
        /// </summary>
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            if (node_type == NodeType.e_const)
            {
                NativeMethods.ov_output_const_port_free(m_ptr);
            }
            else
            {
                NativeMethods.ov_output_port_free(m_ptr);
            }
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Get the shape.
        /// </summary>
        /// <returns>Returns the shape.</returns>
        public Shape get_shape()
        {
            ExceptionStatus status;

            int l = Marshal.SizeOf(typeof(Ov.ov_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            if (node_type == NodeType.e_const)
            {
                status = NativeMethods.ov_const_port_get_shape(m_ptr, shape_ptr);
            }
            else
            {
                status = NativeMethods.ov_port_get_shape(m_ptr, shape_ptr);
            }
            if (status != 0)
            {
                shape_ptr = IntPtr.Zero;
                System.Diagnostics.Debug.WriteLine("Node get_shape error : {0}!", status.ToString());
            }
            return new Shape(shape_ptr);
        }

        /// <summary>
        /// Get the partial shape.
        /// </summary>
        /// <returns>Returns the partial shape.</returns>
        public PartialShape get_partial_shape()
        {
            int l = Marshal.SizeOf(typeof(Ov.ov_partial_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            ExceptionStatus status = NativeMethods.ov_port_get_partial_shape(m_ptr, shape_ptr);
            if (status != 0)
            {
                shape_ptr = IntPtr.Zero;
                System.Diagnostics.Debug.WriteLine("Node get_partial_shape error : {0}!", status.ToString());
            }
            return new PartialShape(shape_ptr);
        }

        /// <summary>
        /// Get the unique name of the node.
        /// </summary>
        /// <returns>A const reference to the node's unique name.</returns>
        public string get_name()
        {
            ExceptionStatus status;
            IntPtr s_ptr = IntPtr.Zero;
            status = NativeMethods.ov_port_get_any_name(m_ptr, ref s_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Node get_name error : {0}!", status.ToString());
            }
            string ss = Marshal.PtrToStringAnsi(s_ptr);
            return ss;
        }
        /// <summary>
        /// Checks that there is exactly one output and returns its element type. 
        /// </summary>
        /// <remarks>
        /// TODO: deprecate in favor of node->get_output_element_type(0) with a suitable check in 
        /// the calling code, or updates to the calling code if it is making an invalid assumption 
        /// of only one output.
        /// </remarks>
        /// <returns>Data type.</returns>
        public OvType get_element_type() 
        {
            ExceptionStatus status = 0;
            uint data_type = 0;

            status = NativeMethods.ov_port_get_element_type(m_ptr, ref data_type);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Node get_type error : {0}!", status.ToString());
            }
            return new OvType((ElementType)data_type);
        }
    }
}
