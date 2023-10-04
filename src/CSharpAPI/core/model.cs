using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static OpenCvSharp.FileStorage;

namespace OpenVinoSharp
{
    /// <summary>
    /// A user-defined model
    /// </summary>
    public class Model : IDisposable
    {
        /// <summary>
        /// [private]Model class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]Model class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="ptr">Model pointer.</param>
        public Model(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("Model init error : ptr is null!");
                return;
            }
            Ptr = ptr;
        }
        /// <summary>
        /// Model's destructor
        /// </summary>
        ~Model()
        {
            Dispose();
        }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_core_free(m_ptr);

            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Gets the friendly name for a model.
        /// </summary>
        /// <returns>The friendly name for a model.</returns>
        public string get_friendly_name() 
        {

            IntPtr s_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_get_friendly_name(m_ptr, ref s_ptr));
            string ss = Marshal.PtrToStringAnsi(s_ptr);

            return ss;
        }
        /// <summary>
        /// Get single input port of model, which only support single input model.
        /// </summary>
        /// <returns>The input port of model.</returns>
        public Node get_input()
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_input(m_ptr, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_nomal);
        }

        /// <summary>
        /// Get an input port of model by name.
        /// </summary>
        /// <param name="tensor_name">input tensor name (string).</param>
        /// <returns>The input port of model.</returns>
        public Node get_input(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            HandleException.handler(
                NativeMethods.ov_model_input_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_nomal);
        }

        /// <summary>
        /// Get an input port of model by port index.
        /// </summary>
        /// <param name="index">input tensor index.</param>
        /// <returns>The input port of model.</returns>
        public Node get_input(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler( 
                NativeMethods.ov_model_input_by_index(m_ptr, index, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_nomal);
        }

        /// <summary>
        /// Get an single output port of model, which only support single output model.
        /// </summary>
        /// <returns>The output port of model.</returns>
        public Node get_output()
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_output(m_ptr, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_nomal);
        }
        /// <summary>
        /// Get an output port of model by name.
        /// </summary>
        /// <param name="tensor_name">output tensor name (string).</param>
        /// <returns>The output port of model.</returns>
        public Node get_output(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            HandleException.handler(
                NativeMethods.ov_model_output_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_nomal);
        }
        /// <summary>
        /// Get an output port of model by port index.
        /// </summary>
        /// <param name="index">input tensor index.</param>
        /// <returns>The output port of model.</returns>
        public Node get_output(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_output_by_index(m_ptr, index, ref port_ptr));

            return new Node(port_ptr, Node.NodeType.e_nomal);
        }
        /// <summary>
        /// Get a const single input port of model, which only support single input model.
        /// </summary>
        /// <returns>The const input port of model.</returns>
        public Node get_const_input()
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_const_input(m_ptr, ref port_ptr));
            return new Node(port_ptr,Node.NodeType.e_const);
        }
        /// <summary>
        /// Get a const input port of model by name.
        /// </summary>
        /// <param name="tensor_name">input tensor name (string).</param>
        /// <returns>The const input port of model.</returns>
        public Node get_const_input(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            HandleException.handler(
                NativeMethods.ov_model_const_input_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        /// <summary>
        /// Get a const input port of model by port index.
        /// </summary>
        /// <param name="index">input tensor index.</param>
        /// <returns>The const input port of model.</returns>
        public Node get_const_input(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_const_input_by_index(m_ptr, index, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        /// <summary>
        /// Get a single const output port of model, which only support single output model..
        /// </summary>
        /// <returns>The const output port of model.</returns>
        public Node get_const_output()
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_const_output(m_ptr, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        /// <summary>
        /// Get a const output port of model by port index.
        /// </summary>
        /// <param name="tensor_name">output tensor name (string).</param>
        /// <returns>The const output port of model.</returns>
        public Node get_const_output(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            HandleException.handler(
                NativeMethods.ov_model_const_output_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        /// <summary>
        /// Get a const output port of model by name.
        /// </summary>
        /// <param name="index">output tensor index.</param>
        /// <returns>The const output port of model.</returns>
        public Node get_const_output(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_model_const_output_by_index(m_ptr, index, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }

        /// <summary>
        /// Get single input of model, which only support single input model.
        /// </summary>
        /// <returns>The input of model.</returns>
        public Input input() 
        {
            Node node = get_input();
            return new Input(node, 0);
        }
        /// <summary>
        /// Get an input of model by port index.
        /// </summary>
        /// <param name="index">input tensor index.</param>
        /// <returns>The input of model.</returns>
        public Input input(ulong index) 
        {
            Node node = get_input(index);
            return new Input(node, index);
        }
        /// <summary>
        /// Get an input of model by name.
        /// </summary>
        /// <param name="tensor_name">input tensor name (string).</param>
        /// <returns>The input of model.</returns>
        public Input input(string tensor_name)
        {
            Node node = get_input(tensor_name);
            return new Input(node, 0);
        }

        /// <summary>
        /// Get single const input of model, which only support single input model.
        /// </summary>
        /// <returns>The const input of model.</returns>
        public Input const_input()
        {
            Node node = get_const_input();
            return new Input(node, 0);
        }
        /// <summary>
        /// Get an const input of model by port index.
        /// </summary>
        /// <param name="index">input tensor index.</param>
        /// <returns>The const input of model.</returns>
        public Input const_input(ulong index)
        {
            Node node = get_const_input(index);
            return new Input(node, index);
        }
        /// <summary>
        /// Get an const input of model by name.
        /// </summary>
        /// <param name="tensor_name">input tensor name (string).</param>
        /// <returns>The const input of model.</returns>
        public Input const_input(string tensor_name)
        {
            Node node = get_const_input(tensor_name);
            return new Input(node, 0);
        }



        /// <summary>
        /// Get single input of model, which only support single input model.
        /// </summary>
        /// <returns>The input of model.</returns>
        public Output output()
        {
            Node node = get_output();
            return new Output(node, 0);
        }
        /// <summary>
        /// Get an output of model by port index.
        /// </summary>
        /// <param name="index">output tensor index.</param>
        /// <returns>The output of model.</returns>
        public Output output(ulong index)
        {
            Node node = get_output(index);
            return new Output(node, index);
        }
        /// <summary>
        /// Get an output of model by name.
        /// </summary>
        /// <param name="tensor_name">output tensor name (string).</param>
        /// <returns>The output of model.</returns>
        public Output output(string tensor_name)
        {
            Node node = get_output(tensor_name);
            return new Output(node, 0);
        }

        /// <summary>
        /// Get single const output of model, which only support single output model.
        /// </summary>
        /// <returns>The const output of model.</returns>
        public Output const_output()
        {
            Node node = get_const_output();
            return new Output(node, 0);
        }
        /// <summary>
        /// Get an const output of model by port index.
        /// </summary>
        /// <param name="index">output tensor index.</param>
        /// <returns>The const output of model.</returns>
        public Output const_output(ulong index)
        {
            Node node = get_const_output(index);
            return new Output(node, index);
        }
        /// <summary>
        /// Get an const output of model by name.
        /// </summary>
        /// <param name="tensor_name">output tensor name (string).</param>
        /// <returns>The const output of model.</returns>
        public Output const_output(string tensor_name)
        {
            Node node = get_const_output(tensor_name);
            return new Output(node, 0);
        }
        /// <summary>
        /// Get the input size of model.
        /// </summary>
        /// <returns>The input size.</returns>
        public ulong get_inputs_size() 
        {
            ulong input_size = 0;
            HandleException.handler(
                NativeMethods.ov_model_inputs_size(m_ptr, ref input_size));
            return input_size;
        }
        /// <summary>
        /// Get the output size of model.
        /// </summary>
        /// <returns>The output size.</returns>
        public ulong get_outputs_size()
        {
            ulong output_size = 0;
            HandleException.handler(
                NativeMethods.ov_model_outputs_size(m_ptr, ref output_size));
            return output_size;
        }

        /// <summary>
        /// Get all input of model.
        /// </summary>
        /// <returns>All input of model.</returns>
        public List<Input> inputs() 
        {
            ulong input_size = get_inputs_size();
            List<Input> inputs = new List<Input>();
            for (ulong index = 0; index < input_size; ++index) 
            {
                inputs.Add(input(index));
            }
            return inputs;
        }
        /// <summary>
        /// Get all output of model
        /// </summary>
        /// <returns>All output of model</returns>
        public List<Output> outputs()
        {
            ulong output_size = get_outputs_size();
            List<Output> outputs = new List<Output>();
            for (ulong index = 0; index < output_size; ++index)
            {
                outputs.Add(output(index));
            }
            return outputs;
        }

        /// <summary>
        /// Get all const input of model.
        /// </summary>
        /// <returns>All input of model.</returns>
        public List<Input> const_inputs()
        {
            ulong input_size = get_inputs_size();
            List<Input> inputs = new List<Input>();
            for (ulong index = 0; index < input_size; ++index)
            {
                inputs.Add(const_input(index));
            }
            return inputs;
        }

        /// <summary>
        /// Get all const output of model
        /// </summary>
        /// <returns>All output of model</returns>
        public List<Output> const_outputs()
        {
            ulong output_size = get_outputs_size();
            List<Output> outputs = new List<Output>();
            for (ulong index = 0; index < output_size; ++index)
            {
                outputs.Add(const_output(index));
            }
            return outputs;
        }
        /// <summary>
        /// The ops defined in the model is dynamic shape.
        /// </summary>
        /// <returns>true if any of the ops defined in the model is dynamic shape..</returns>
        public bool is_dynamic() 
        {
            return NativeMethods.ov_model_is_dynamic(m_ptr);
        }


        /// <summary>
        /// Do reshape in model with partial shape for a specified name.
        /// </summary>
        /// <param name="partial_shapes">The list of input tensor names and PartialShape.</param>
        public void reshape(Dictionary<string, PartialShape> partial_shapes)
        {
            if (1 != partial_shapes.Count)
            {
                IntPtr[] tensor_names_ptr = new IntPtr[partial_shapes.Count];
                Ov.ov_partial_shape[] shapes = new Ov.ov_partial_shape[partial_shapes.Count];
                int i = 0;
                foreach (var partial_shape in partial_shapes)
                {
                    IntPtr p = Marshal.StringToHGlobalAnsi(partial_shape.Key);
                    tensor_names_ptr[i] = p;
                    shapes[i] = partial_shape.Value.get_partial_shape();
                }
                HandleException.handler(
                    NativeMethods.ov_model_reshape(m_ptr, tensor_names_ptr,
                    ref shapes[0], (ulong)partial_shapes.Count));
            }
            else 
            {
                foreach (var partial_shape in partial_shapes) 
                {
                    sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(partial_shape.Key));
                    Ov.ov_partial_shape shape = partial_shape.Value.get_partial_shape();
                    HandleException.handler(
                        NativeMethods.ov_model_reshape_input_by_name(m_ptr, ref c_tensor_name[0],
                        shape));
                }

            }
        }
        /// <summary>
        /// Do reshape in model for one node(port 0).
        /// </summary>
        /// <param name="partial_shape">A PartialShape.</param>
        public void reshape(PartialShape partial_shape) 
        {
            HandleException.handler(
                NativeMethods.ov_model_reshape_single_input(m_ptr, partial_shape.get_partial_shape()));
        }
        /// <summary>
        /// Do reshape in model with a list of (port id, partial shape).
        /// </summary>
        /// <param name="partial_shapes">The list of input port id and PartialShape.</param>
        public void reshape(Dictionary<ulong, PartialShape> partial_shapes)
        {
            ulong[] indexs = new ulong[partial_shapes.Count];
            Ov.ov_partial_shape[] shapes = new Ov.ov_partial_shape[partial_shapes.Count];
            int i = 0;
            foreach (var partial_shape in partial_shapes)
            {
                indexs[i] = partial_shape.Key;
                shapes[i] = partial_shape.Value.get_partial_shape();
            }
            HandleException.handler(NativeMethods.ov_model_reshape_by_port_indexes(m_ptr, ref indexs[0],
                ref shapes[0], (ulong)partial_shapes.Count));
        }
        /// <summary>
        /// Do reshape in model with a list of (ov_output_port_t, partial shape).
        /// </summary>
        /// <param name="partial_shapes">The list of input node and PartialShape.</param>
        public void reshape(Dictionary<Node, PartialShape> partial_shapes)
        {
            IntPtr[] nodes_ptr = new IntPtr[partial_shapes.Count];
            Ov.ov_partial_shape[] shapes = new Ov.ov_partial_shape[partial_shapes.Count];
            int i = 0;
            foreach (var partial_shape in partial_shapes)
            {
                nodes_ptr[i] = partial_shape.Key.Ptr; 
                shapes[i] = partial_shape.Value.get_partial_shape();
            }
            HandleException.handler(NativeMethods.ov_model_reshape_by_ports(m_ptr, ref nodes_ptr[0],
                ref shapes[0], (ulong)partial_shapes.Count));
        }

    }

    
}


