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
        /// Define whether the Node is const
        /// </summary>
        public enum NodeType
        {
            /// <summary>
            /// const type
            /// </summary>
            e_const = 0,
            /// <summary>
            /// nomal type
            /// </summary>
            e_nomal = 1
        };

        /// <summary>
        /// [private]Node class pointer.
        /// </summary>
        private IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]Node class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        /// <summary>
        /// Node type
        /// </summary>
        public NodeType node_type { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="ptr">A pointer to the Node.</param>
        /// <param name="type">Node type</param>
        public Node(IntPtr ptr, NodeType type)
        {
            Ptr = ptr;
            this.node_type = type;
        }
        /// <summary>
        /// Default deconstruction
        /// </summary>
        ~Node() {
            dispose();
        }
        /// <summary>
        /// Memory Release
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
        /// Checks that there is exactly one output and returns its shape
        /// </summary>
        /// <returns>The node shape.</returns>
        public Shape get_shape()
        {
            ExceptionStatus status;
            int l = Marshal.SizeOf(typeof(Shape.ov_shape));
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
        /// Get the partial shape for output
        /// </summary>
        /// <returns>The partial shape for output</returns>
        public PartialShape get_partial_shape()
        {
            ExceptionStatus status;
            int l = Marshal.SizeOf(typeof(PartialShape.ov_partial_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            if (node_type == NodeType.e_const)
            {
                return new PartialShape(shape_ptr);
            }
            else
            {
                status = NativeMethods.ov_port_get_partial_shape(m_ptr, shape_ptr);
            }
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
        /// <returns> A const reference to the node's unique name.</returns>
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
        /// Checks that there is exactly one output and returns its element type
        /// </summary>
        /// <returns>element type</returns>
        public element.Type get_type() 
        {
            ExceptionStatus status = 0;
            uint data_type = 0;
            if (node_type == NodeType.e_const)
            {
                status = NativeMethods.ov_port_get_element_type(m_ptr, ref data_type);
            }
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Node get_type error : {0}!", status.ToString());
            }
            return new element.Type((element.Type_t)data_type);
        }
    }
}
