using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// This class represents a compiled model.
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    /// <remarks>
    /// A model is compiled by a specific device by applying multiple optimization 
    /// transformations, then mapping to compute kernels.
    /// </remarks>
    public class CompiledModel : IDisposable
    {
        /// <summary>
        /// [private]CompiledModel class pointer.
        /// </summary>
        private IntPtr m_ptr;
        /// <summary>
        /// [private]CompiledModel class pointer.
        /// </summary>
        public IntPtr Ptr 
        { 
            get { return m_ptr; } 
            set { m_ptr = value; } 
        }

        /// <summary>
        /// Constructs CompiledModel from the initialized ptr.
        /// </summary>
        /// <param name="ptr"></param>
        public CompiledModel(IntPtr ptr) 
        {
            this.m_ptr = ptr;
        }
        /// <summary>
        /// CompiledModel()'s destructor
        /// </summary>
        ~CompiledModel()
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
        /// Creates an inference request object used to infer the compiled model.
        /// The created request has allocated input and output tensors (which can be changed later).
        /// </summary>
        /// <returns>InferRequest object</returns>
        public InferRequest create_infer_request()
        {
            IntPtr infer_request_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_compiled_model_create_infer_request(m_ptr, ref infer_request_ptr));
            return new InferRequest(infer_request_ptr);
        }

        /// <summary>
        /// Get a const single input port of compiled_model, which only support single input compiled_model.
        /// </summary>
        /// <returns>The input port of compiled_model.</returns>
        public Node get_input()
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_compiled_model_input(m_ptr, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }

        /// <summary>
        /// Get a const input port of compiled_model by name.
        /// </summary>
        /// <param name="tensor_name">input tensor name (string).</param>
        /// <returns>The input port of compiled_model.</returns>
        public Node get_input(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            HandleException.handler(
                NativeMethods.ov_compiled_model_input_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }

        /// <summary>
        /// Get a const input port of compiled_model by port index.
        /// </summary>
        /// <param name="index">input tensor index.</param>
        /// <returns>The input port of compiled_model.</returns>
        public Node get_input(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_compiled_model_input_by_index(m_ptr, index, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }

        /// <summary>
        /// Get a const single output port of compiled_model, which only support single output model.
        /// </summary>
        /// <returns>The output port of compiled_model.</returns>
        public Node get_output()
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(NativeMethods.ov_compiled_model_output(m_ptr, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        /// <summary>
        /// Get a const output port of compiled_model by name.
        /// </summary>
        /// <param name="tensor_name">output tensor name (string).</param>
        /// <returns>The output port of compiled_model.</returns>
        public Node get_output(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            HandleException.handler(
                NativeMethods.ov_compiled_model_output_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        /// <summary>
        /// Get a const output port of compiled_model by port index.
        /// </summary>
        /// <param name="index">input tensor index.</param>
        /// <returns>The output port of compiled_model.</returns>
        public Node get_output(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_compiled_model_output_by_index(m_ptr, index, ref port_ptr));
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        /// <summary>
        /// Get the input size of compiled_model.
        /// </summary>
        /// <returns>The input size of compiled_model.</returns>
        public ulong get_inputs_size()
        {
            ulong input_size = 0;
            HandleException.handler(
                NativeMethods.ov_compiled_model_inputs_size(m_ptr, ref input_size));
            return input_size;
        }
        /// <summary>
        /// Get the output size of compiled_model.
        /// </summary>
        /// <returns>The output size.</returns>
        public ulong get_outputs_size()
        {
            ulong output_size = 0;
            HandleException.handler(
                NativeMethods.ov_compiled_model_outputs_size(m_ptr, ref output_size));
            return output_size;
        }

        /// <summary>
        /// Gets a single input of a compiled model.
        /// </summary>
        /// <remarks>
        /// The input is represented as an output of the ov::op::v0::Parameter operation. 
        /// The input contains information about input tensor such as tensor shape, names, and element type.
        /// </remarks>
        /// <returns>Compiled model input.</returns>
        /// <exception cref="">If a model has more than one input, this method throws ov::Exception.</exception>
        public Input input()
        {
            Node node = get_input();
            return new Input(node, 0);
        }
        /// <summary>
        /// Gets input of a compiled model identified by @p index.
        /// </summary>
        /// <remarks>The input contains information about input tensor such as tensor shape, names, and element type.</remarks>
        /// <param name="index">Index of input.</param>
        /// <returns>Compiled model input.</returns>
        /// <exception cref="">The method throws ov::Exception if input with the specified index @p i is not found.</exception>
        public Input input(ulong index)
        {
            Node node = get_input(index);
            return new Input(node, index);
        }
        /// <summary>
        /// Gets input of a compiled model identified by @p tensor_name.
        /// </summary>
        /// <remarks>The input contains information about input tensor such as tensor shape, names, and element type.</remarks>
        /// <param name="tensor_name">Output tensor name.</param>
        /// <returns>Compiled model input.</returns>
        /// <exception cref="">The method throws ov::Exception if input with the specified tensor name @p tensor_name is not found.</exception>
        public Input input(string tensor_name)
        {
            Node node = get_input(tensor_name);
            return new Input(node, 0);
        }

        /// <summary>
        /// Gets a single output of a compiled model.
        /// </summary>
        /// <remarks>
        /// The output is represented as an output from the ov::op::v0::Result operation. 
        /// The output contains information about output tensor such as tensor shape, names, and element type.
        /// </remarks>
        /// <returns>Compiled model output.</returns>
        /// <exception cref="">If a model has more than one output, this method throws ov::Exception.</exception>
        public Output output()
        {
            Node node = get_output();
            return new Output(node, 0);
        }
        /// <summary>
        /// Gets output of a compiled model identified by @p index.
        /// </summary>
        /// <remarks> The output contains information about output tensor such as tensor shape, names, and element type.</remarks>
        /// <param name="index">Index of output.</param>
        /// <returns>Compiled model output.</returns>
        /// <exception cref="">The method throws ov::Exception if output with the specified index @p index is not found.</exception>
        public Output output(ulong index)
        {
            Node node = get_output(index);
            return new Output(node, index);
        }
        /// <summary>
        /// Gets output of a compiled model identified by @p tensor_name.
        /// </summary>
        /// <remarks>The output contains information about output tensor such as tensor shape, names, and element type.</remarks>
        /// <param name="tensor_name">Output tensor name.</param>
        /// <returns>Compiled model output.</returns>
        /// <exception cref="">The method throws ov::Exception if output with the specified tensor name @p tensor_name is not found.</exception>
        public Output output(string tensor_name)
        {
            Node node = get_output(tensor_name);
            return new Output(node, 0);
        }

        /// <summary>
        /// Gets all inputs of a compiled model.
        /// </summary>
        /// <remarks>
        /// Inputs are represented as a vector of outputs of the ov::op::v0::Parameter operations. 
        /// They contain information about input tensors such as tensor shape, names, and element type.
        /// </remarks>
        /// <returns>List of model inputs.</returns>
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
        /// Get all outputs of a compiled model.
        /// </summary>
        /// <remarks>
        /// Outputs are represented as a vector of output from the ov::op::v0::Result operations. 
        /// Outputs contain information about output tensors such as tensor shape, names, and element type.
        /// </remarks>
        /// <returns>List of model outputs.</returns>
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
        /// Gets runtime model information from a device.
        /// </summary>
        /// <remarks>
        /// This object represents an internal device-specific model that is optimized for a particular 
        /// accelerator. It contains device-specific nodes, runtime information and can be used only 
        /// to understand how the source model is optimized and which kernels, element types, and layouts 
        /// are selected for optimal inference.
        /// </remarks>
        /// <returns></returns>
        public Model get_runtime_model()
        { 
            IntPtr model_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_compiled_model_get_runtime_model(m_ptr, ref model_ptr));
            return new Model(model_ptr);
        }

        /// <summary>
        /// Exports the current compiled model to an output model_path. 
        /// The exported model can also be imported via the ov::Core::import_model method.
        /// </summary>
        /// <param name="model_path">Output path to store the model to.</param>
        public void export_model(string model_path) 
        {
            sbyte[] c_model_path = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(model_path));
            HandleException.handler(
                NativeMethods.ov_compiled_model_export_model(m_ptr, ref c_model_path[0]));
        }

        /// <summary>
        /// Sets properties for the current compiled model.
        /// </summary>
        /// <param name="properties">Map of pairs: (property name, property value).</param>
        public void set_property(KeyValuePair<string, string> properties) 
        {
            IntPtr property_key = Marshal.StringToHGlobalAnsi(properties.Key);
            IntPtr property_value = Marshal.StringToHGlobalAnsi(properties.Value);
            HandleException.handler(
                NativeMethods.ov_compiled_model_set_property(m_ptr, property_key, property_value));
        }
        /// <summary>
        /// Gets properties for current compiled model
        /// </summary>
        /// <remarks>
        /// The method is responsible for extracting information that affects compiled model inference. 
        /// The list of supported configuration values can be extracted via CompiledModel::get_property 
        /// with the ov::supported_properties key, but some of these keys cannot be changed dynamically, 
        /// for example, ov::device::id cannot be changed if a compiled model has already been compiled 
        /// for a particular device.
        /// </remarks>
        /// <param name="property_key">Property key, can be found in openvino/runtime/properties.hpp.</param>
        /// <returns>Property value.</returns>
        public string get_property(string property_key) 
        {
            sbyte[] c_property_key = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(property_key));
            IntPtr property_value_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_compiled_model_get_property(m_ptr, ref c_property_key[0], 
                ref property_value_ptr));
            return Marshal.PtrToStringAnsi(property_value_ptr);
        }

        /// <summary>
        /// Returns pointer to device-specific shared context on a remote accelerator device that was used 
        /// to create this CompiledModel.
        /// </summary>
        /// <returns>A context.</returns>
        public RemoteContext get_context() {
            IntPtr context_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_compiled_model_get_context(m_ptr, ref context_ptr));
            return new RemoteContext(context_ptr);
        }
    }
}
