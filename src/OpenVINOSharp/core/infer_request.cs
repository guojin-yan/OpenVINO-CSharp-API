using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static OpenVinoSharp.Ov;

namespace OpenVinoSharp
{
    /// <summary>
    /// This is a class of infer request that can be run in asynchronous or synchronous manners.
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    public class InferRequest
    {
        /// <summary>
        /// [private]InferRequest class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]InferRequest class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Constructs InferRequest from the initialized IntPtr.
        /// </summary>
        /// <param name="ptr"></param>
        public InferRequest(IntPtr ptr)
        {
            this.m_ptr = ptr;
        }
        /// <summary>
        /// InferRequest's destructor
        /// </summary>
        ~InferRequest()
        {
            dispose();
        }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_core_free(m_ptr);

            m_ptr = IntPtr.Zero;
        }
        /// <summary>
        ///  Sets an input/output tensor to infer on.
        /// </summary>
        /// <param name="tensor_name">Name of the input or output tensor.</param>
        /// <param name="tensor">Reference to the tensor. The element_type and shape of the tensor must match 
        /// the model's input/output element_type and size.</param>
        public void set_tensor(string tensor_name,Tensor tensor) 
        {
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_infer_request_set_tensor(
                m_ptr, ref c_tensor_name[0], tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_tensor error : " + status.ToString());
            }
        }
        /// <summary>
        /// Sets an input/output tensor to infer.
        /// </summary>
        /// <param name="node">Node of the input or output tensor.</param>
        /// <param name="tensor"></param>
        public void set_tensor(Node node, Tensor tensor)
        {
            ExceptionStatus status;
            if (node.node_type == Node.NodeType.e_const) 
            {
                status = NativeMethods.ov_infer_request_set_tensor_by_const_port(
               m_ptr, node.Ptr, tensor.Ptr);
            }
            else {
                status = NativeMethods.ov_infer_request_set_tensor_by_port(
                m_ptr, node.Ptr, tensor.Ptr);
            }
     
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_tensor error : " + status.ToString());
            }
        }
        /// <summary>
        /// Sets an input/output tensor to infer.
        /// </summary>
        /// <param name="port">
        /// Port of the input or output tensor. Use the following methods to get the ports:
        /// - Model.input() 
        /// - Model.const_input()   
        /// - Model.inputs()    
        /// - Model.const_inputs()  
        /// - Model.output()    
        /// - Model.const_output()  
        /// - Model.outputs()   
        /// - Model.const_outputs()    
        /// - CompiledModel.input() 
        /// - CompiledModel.const_input() 
        /// - CompiledModel.inputs()    
        /// - CompiledModel.const_inputs()    
        /// - CompiledModel.output()  
        /// - CompiledModel.const_output()   
        /// - CompiledModel.outputs()   
        /// - CompiledModel.const_outputs()  
        /// </param>
        /// <param name="tensor">Reference to a tensor. The element_type and shape of a tensor must match 
        /// the model's input/output element_type and size.</param>
        public void set_tensor(Output port, Tensor tensor)
        {
            ExceptionStatus status;
            if (port.get_node().node_type == Node.NodeType.e_const)
            {
                status = NativeMethods.ov_infer_request_set_tensor_by_const_port(
               m_ptr, port.get_node().Ptr, tensor.Ptr);
            }
            else
            {
                status = NativeMethods.ov_infer_request_set_tensor_by_port(
                m_ptr, port.get_node().Ptr, tensor.Ptr);
            }

            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_tensor error : " + status.ToString());
            }
        }

        /// <summary>
        /// Sets an input tensor to infer.
        /// </summary>
        /// <param name="index">Index of the input tensor. If @p idx is greater than the number of model inputs, 
        ///  an exception is thrown.</param>
        /// <param name="tensor">Reference to the tensor. The element_type and shape of the tensor must match 
        /// the model's input/output element_type and size.</param>
        public void set_input_tensor(ulong index, Tensor tensor)
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_set_input_tensor_by_index(
                m_ptr, index, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_input_tensor error : " + status.ToString());
            }
        }

        /// <summary>
        /// Sets an input tensor to infer models with single input.
        /// </summary>
        /// <exception cref="">If model has several inputs, an exception is thrown.</exception>
        /// <param name="tensor">Reference to the input tensor.</param>
        public void set_input_tensor(Tensor tensor)
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_set_input_tensor(
                m_ptr, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_input_tensor error : " + status.ToString());
            }
        }
        /// <summary>
        /// Sets an output tensor to infer.
        /// Index of the input preserved accross Model, CompiledModel, and InferRequest.
        /// </summary>
        /// <param name="index">Index of the output tensor.</param>
        /// <param name="tensor">Reference to the output tensor. The type of the tensor must match the model 
        /// output element type and shape.</param>
        public void set_output_tensor(ulong index, Tensor tensor)
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_set_output_tensor_by_index(
                m_ptr, index, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_output_tensor error : " + status.ToString());
            }
        }
        /// <summary>
        /// Sets an output tensor to infer models with single output.
        /// </summary>
        /// <exception cref="">If model has several outputs, an exception is thrown.</exception>
        /// <param name="tensor">Reference to the output tensor.</param>
        public void set_output_tensor(Tensor tensor)
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_set_output_tensor(
                m_ptr, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_output_tensor error : " + status.ToString());
            }
        }

      

        /// <summary>
        ///  Gets an input/output tensor for inference by tensor name.
        /// </summary>
        /// <param name="tensor_name">Name of a tensor to get.</param>
        /// <returns>The tensor with name @p tensor_name. If the tensor is not found, an exception is thrown.</returns>
        public Tensor get_tensor(string tensor_name) 
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_infer_request_get_tensor(m_ptr, ref c_tensor_name[0], ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// Gets an input/output tensor for inference by node.
        /// </summary>
        /// <exception cref="">If the tensor with the specified @n node is not found, an exception is thrown.</exception>
        /// <param name="node">Node of the tensor to get.</param>
        /// <returns>Tensor for the node @n node.</returns>
        public Tensor get_tensor(Node node)
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionStatus status;
            if (node.node_type == Node.NodeType.e_const)
            {
                status = NativeMethods.ov_infer_request_get_tensor_by_const_port(
               m_ptr, node.Ptr, ref tensor_ptr);
            }
            else
            {
                status = NativeMethods.ov_infer_request_get_tensor_by_port(
                m_ptr, node.Ptr, ref tensor_ptr);
            }

            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor get_tensor error : " + status.ToString());
            }
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// Gets an input/output tensor for inference.
        /// </summary>
        /// <exception cref="">If the tensor with the specified @p port is not found, an exception is thrown.</exception>
        /// <param name="port">Port of the tensor to get.</param>
        /// <returns>Tensor for the port @p port.</returns>
        public Tensor get_tensor(Output port)
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionStatus status;
            if (port.get_node().node_type == Node.NodeType.e_const)
            {
                status = NativeMethods.ov_infer_request_get_tensor_by_const_port(
               m_ptr, port.get_node().Ptr, ref tensor_ptr);
            }
            else
            {
                status = NativeMethods.ov_infer_request_get_tensor_by_port(
                m_ptr, port.get_node().Ptr, ref tensor_ptr);
            }

            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor get_tensor error : " + status.ToString());
            }
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// Gets an input tensor for inference.
        /// </summary>
        /// <param name="index">Index of the tensor to get.</param>
        /// <returns>Tensor with the input index @p idx. If the tensor with the specified @p idx is not found, 
        /// an exception is thrown.</returns>
        public Tensor get_input_tensor(ulong index)
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_infer_request_get_input_tensor_by_index(
                m_ptr, index, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_input_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// Gets an input tensor for inference.
        /// </summary>
        /// <returns>The input tensor for the model. If model has several inputs, an exception is thrown.</returns>
        public Tensor get_input_tensor()
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            
            ExceptionStatus status = NativeMethods.ov_infer_request_get_input_tensor(
                m_ptr, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_input_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// Gets an output tensor for inference.
        /// </summary>
        /// <param name="index">Index of the tensor to get.</param>
        /// <returns>Tensor with the output index @p idx. If the tensor with the specified @p idx is not found, 
        /// an exception is thrown.</returns>
        public Tensor get_output_tensor(ulong index)
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_infer_request_get_output_tensor_by_index(
                m_ptr, index, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_output_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// Gets an output tensor for inference.
        /// </summary>
        /// <returns>Output tensor for the model. If model has several outputs, an exception is thrown.</returns>
        public Tensor get_output_tensor()
        {
            IntPtr tensor_ptr = IntPtr.Zero;

            ExceptionStatus status = NativeMethods.ov_infer_request_get_output_tensor(
                m_ptr, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_output_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }
        /// <summary>
        /// Infers specified input(s) in synchronous mode.
        /// </summary>
        /// <note>
        /// It blocks all methods of InferRequest while request is ongoing (running or waiting in a queue).
        /// Calling any method leads to throwning the ov::Busy exception.
        /// </note>
        public void infer()
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_infer(m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest infer() error : " + status.ToString());
            }
        }

        /// <summary>
        /// Cancels inference request.
        /// </summary>
        public void cancel() 
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_cancel(m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest cancel() error : " + status.ToString());
            }
        }
        /// <summary>
        /// Starts inference of specified input(s) in asynchronous mode.
        /// </summary>
        /// <note>
        /// It returns immediately. Inference starts also immediately.
        /// Calling any method while the request in a running state leads to throwning the ov::Busy exception.
        /// </note>
        public void start_async() 
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_start_async(m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest start_async() error : " + status.ToString());
            }
        }
        /// <summary>
        /// Waits for the result to become available. Blocks until the result becomes available.
        /// </summary>
        public void wait()
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_wait(m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest wait() error : " + status.ToString());
            }
        }

        /// <summary>
        /// Waits for the result to become available. Blocks until the specified timeout has elapsed or the result 
        /// becomes available, whichever comes first.
        /// </summary>
        /// <param name="timeout">Maximum duration, in milliseconds, to block for.</param>
        /// <returns>True if inference request is ready and false, otherwise.</returns>
        public bool wait_for(long timeout)
        {
            ExceptionStatus status = NativeMethods.ov_infer_request_wait_for(m_ptr, timeout);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest wait_for() error : " + status.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Queries performance measures per layer to identify the most time consuming operation.
        /// </summary>
        /// <remarks>Not all plugins provide meaningful data.</remarks>
        /// <returns>List of profiling information for operations in a model.</returns>
        public List<Ov.ProfilingInfo> get_profiling_info()
        {
            ov_profiling_info_list profiling_info_list = new ov_profiling_info_list();
            ExceptionStatus status = NativeMethods.ov_infer_request_get_profiling_info(m_ptr, ref profiling_info_list);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_profiling_info() error : " + status.ToString());
            }
            IntPtr[] profiling_infos_ptr = new IntPtr[profiling_info_list.size];
            Marshal.Copy(profiling_info_list.profiling_infos, profiling_infos_ptr, 0, (int)profiling_info_list.size);
            List<Ov.ProfilingInfo> profiling_infos = new List<ProfilingInfo>();
            for (int i = 0; i < (int)profiling_info_list.size; ++i) 
            {
                var temp = Marshal.PtrToStructure(profiling_infos_ptr[i], typeof(Ov.ProfilingInfo));
                Ov.ProfilingInfo profiling_info = (Ov.ProfilingInfo)temp;
                profiling_infos.Add(profiling_info);
            }
            return profiling_infos;
        }
    }
}
