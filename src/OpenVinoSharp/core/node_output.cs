using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// A handle for one of a node's outputs.
    /// </summary>
    /// <typeparam name="Node"></typeparam>
    public class Output {
        private Node m_node;
        private ulong m_index = 0;
        /// <summary>
        /// Constructs a Output.
        /// </summary>
        /// <param name="node">The node for the output handle.</param>
        /// <param name="index">The index of the output.</param>
        public Output(Node node, ulong index)
        {
            m_node = node;
            m_index = index;
        }
        /// <summary>
        /// Get the node referred to by this output handle.
        /// </summary>
        /// <returns>The ouput node</returns>
        public Node get_node() { return m_node; }
        /// <summary>
        /// The index of the output referred to by this output handle.
        /// </summary>
        /// <returns>The index of the output.</returns>
        public ulong get_index() { return m_index; }
        /// <summary>
        /// The element type of the output referred to by this output handle.
        /// </summary>
        /// <returns>The element type of the output.</returns>
        public OvType get_element_type() { return m_node.get_type() as OvType; }
        /// <summary>
        /// The shape of the output referred to by this output handle.
        /// </summary>
        /// <returns>The shape of the output .</returns>
        public Shape get_shape(){ return m_node.get_shape(); }
        /// <summary>
        /// Any tensor names associated with this output
        /// </summary>
        /// <returns>tensor names</returns>
        public string get_any_name() { return m_node.get_name(); }
        /// <summary>
        /// The partial shape of the output referred to by this output handle.
        /// </summary>
        /// <returns>The partial shape of the output</returns>
        public PartialShape get_partial_shape() { return m_node.get_partial_shape(); }
    }
}
