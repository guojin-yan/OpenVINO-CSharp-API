using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenVinoSharp
{
    public class Model
    {
        public IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
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
        ~Model() { dispose(); }
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

        public string get_friendly_name() 
        {

            IntPtr s_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_get_friendly_name(m_ptr, ref s_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model get_type error : {0}!", status.ToString());
            }
            string ss = Marshal.PtrToStringAnsi(s_ptr);

            return ss;
        }

        public Node get_const_input()
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_input(m_ptr, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model get_type error!");
            }
            return new Node(port_ptr,Node.NodeType.e_const);
        }
        public Node get_const_input(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_model_const_input_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model get_type error!");
            }
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        public Node get_const_input(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_input_by_index(m_ptr, index, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model get_type error!");
            }
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        public Node get_const_output()
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_output(m_ptr, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model get_type error!");
            }
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        public Node get_const_output(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_model_const_output_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model get_type error!");
            }
            return new Node(port_ptr, Node.NodeType.e_const);
        }
        public Node get_const_output(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_output_by_index(m_ptr, index, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model get_type error!");
            }
            return new Node(port_ptr, Node.NodeType.e_const);
        }

        /// <summary>
        /// Get a single output of model, which only support single output model.
        /// </summary>
        /// <returns>The output of model.</returns>
        public Output output()
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_output(m_ptr, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model output error!");
            }
            return new Output(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }
        /// <summary>
        /// Get an output of model by index.
        /// </summary>
        /// <param name="index">output index</param>
        /// <returns>The output of model.</returns>
        public Output output(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_output_by_index(m_ptr, index, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model output error!");
            }
            return new Output(new Node(port_ptr, Node.NodeType.e_nomal), index);
        }
        /// <summary>
        /// Get an output of model by name.
        /// </summary>
        /// <param name="tensor_name">output name.</param>
        /// <returns>The output of model.</returns>
        public Output output(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_model_output_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model output error!");
            }
            return new Output(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }

        /// <summary>
        /// Get a single const output of model, which only support single output model.
        /// </summary>
        /// <returns>The const output of model.</returns>
        public Output const_output()
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_output(m_ptr, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model const_output error!");
            }
            return new Output(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }

        /// <summary>
        /// Get an const output of model by index.
        /// </summary>
        /// <param name="index">output index</param>
        /// <returns>The const output of model.</returns>
        public Output const_output(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_output_by_index(m_ptr, index, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model const_output error!");
            }
            return new Output(new Node(port_ptr, Node.NodeType.e_nomal), index);
        }

        /// <summary>
        /// Get an const output of model by name.
        /// </summary>
        /// <param name="tensor_name">output name.</param>
        /// <returns>The const output of model.</returns>
        public Output const_output(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_model_const_output_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model const_output error!");
            }
            return new Output(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }


        /// <summary>
        /// Get a single input of model, which only support single input model.
        /// </summary>
        /// <returns>The input of model.</returns>
        public Input input()
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_input(m_ptr, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model input error!");
            }
            return new Input(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }
        /// <summary>
        /// Get an input of model by index.
        /// </summary>
        /// <param name="index">input index</param>
        /// <returns>The input of model.</returns>
        public Input input(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_input_by_index(m_ptr, index, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model input error!");
            }
            return new Input(new Node(port_ptr, Node.NodeType.e_nomal), index);
        }
        /// <summary>
        /// Get an input of model by name.
        /// </summary>
        /// <param name="tensor_name">input name.</param>
        /// <returns>The input of model.</returns>
        public Input input(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_model_input_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model input error!");
            }
            return new Input(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }
        /// <summary>
        /// Get a single const input of model, which only support single const input model.
        /// </summary>
        /// <returns>The const input of model.</returns>
        public Input const_input()
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_input(m_ptr, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model const_input error!");
            }
            return new Input(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }
        /// <summary>
        /// Get an const input of model by index.
        /// </summary>
        /// <param name="index">input index</param>
        /// <returns>The const input of model.</returns>
        public Input const_input(ulong index)
        {
            IntPtr port_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_model_const_input_by_index(m_ptr, index, ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model const_input error!");
            }
            return new Input(new Node(port_ptr, Node.NodeType.e_nomal), index);
        }
        /// <summary>
        /// Get an const input of model by name.
        /// </summary>
        /// <param name="tensor_name">input name.</param>
        /// <returns>The const input of model.</returns>
        public Input const_input(string tensor_name)
        {
            IntPtr port_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = NativeMethods.ov_model_const_input_by_name(m_ptr, ref c_tensor_name[0], ref port_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Model const_input error!");
            }
            return new Input(new Node(port_ptr, Node.NodeType.e_nomal), 0);
        }

    }

    
}


