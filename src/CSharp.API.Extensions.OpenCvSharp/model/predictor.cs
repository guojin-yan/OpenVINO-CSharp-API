using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model
{
    /// <summary>
    /// Default model inference engine.
    /// </summary>
    public abstract class Predictor
    {
        protected Core m_core;
        protected Model m_model;
        protected CompiledModel m_compiled_model;
        protected InferRequest m_infer_request;

        protected bool m_use_gpu;
        public Predictor() { }
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="model_path">Inference model path</param>
        public Predictor(string model_path)
        {
            m_core = new Core();
            m_model = m_core.read_model(model_path);
            m_compiled_model = m_core.compile_model(m_model);
            m_infer_request = m_compiled_model.create_infer_request();
        }
        /// <summary>
        /// Parameter constructor.
        /// </summary>
        /// <param name="model_path">Inference model path.</param>
        /// <param name="device">Inference model path device.</param>
        /// <param name="cache_dir">The read-write property(string) to set/get the directory which will be used to store any data cached by plugins.</param>
        /// <param name="input_size"></param>
        /// <param name="use_gpu"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Predictor(string model_path, string device, string? cache_dir = null, bool? use_gpu = false, long[]? input_size = null)
        {
            string cache = cache_dir ?? null;
            m_use_gpu = use_gpu ?? false;
            m_core = new Core();
            if (cache != null)
            {
                m_core.set_property(device, Ov.cache_dir(cache_dir));
            }
            m_model = m_core.read_model(model_path);
            if (m_use_gpu)
            {
                if (input_size == null)
                {
                    throw new ArgumentNullException("input_size");
                }
                m_model.reshape(new PartialShape(new Shape(input_size)));
            }
            m_compiled_model = m_core.compile_model(m_model, device);
            m_infer_request = m_compiled_model.create_infer_request();
        }

        /// <summary>
        /// The default model inference method is only applicable to single input and single output models.
        /// </summary>
        /// <param name="input_data">The input data.</param>
        /// <param name="shape">The input shape.</param>
        /// <returns>Infer result.</returns>
        protected float[] infer(float[] input_data, long[] shape = null)
        {
            Tensor input_tensor = m_infer_request.get_input_tensor();
            if (shape != null)
                input_tensor.set_shape(new Shape(shape));
            input_tensor.set_data<float>(input_data);
            m_infer_request.infer();

            Tensor output_tensor = m_infer_request.get_output_tensor();
            string s =output_tensor.get_shape().to_string();
            float[] result = output_tensor.get_data<float>((int)output_tensor.get_size());
            return result;

        }
    }
}
