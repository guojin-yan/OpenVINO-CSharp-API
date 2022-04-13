using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openvinosharp_text
{
    internal class Core
    {
        private IntPtr ptr = new IntPtr();

        // @brief core类默认初始化方法
        public Core() { }
        // @brief core类参数输入初始化方法
        // @param model_file 本地推理模型地址路径
        // @param device_name 设备名称
        public Core(string model_file, string device_name)
        {
            // 初始化推理核心
            ptr = NativeMethods.core_init(model_file, device_name);
        }
        // @brief 设置推理模型的输入节点的大小
        // @param input_node_name 输入节点名
        // @param input_size 输入形状大小数组
        public void set_input_sharp(string input_node_name, ulong[] input_size)
        {
            // 获取输入数组长度
            int length = input_size.Length;
            if (length == 4)
            {
                // 长度为4，判断为设置图片输入的输入参数，调用设置图片形状方法
                ptr = NativeMethods.set_input_image_sharp(ptr, input_node_name, ref input_size[0]);
            }
            else if (length == 2)
            {
                // 长度为2，判断为设置普通数据输入的输入参数，调用设置普通数据形状方法
                ptr = NativeMethods.set_input_data_sharp(ptr, input_node_name, ref input_size[0]);
            }
            else
            {
                // 为防止输入发生异常，直接返回
                return;
            }
        }

        // @brief 加载推理数据
        // @param input_node_name 输入节点名
        // @param input_data 输入数据数组
        public void load_input_data(string input_node_name, float[] input_data)
        {
            ptr = NativeMethods.load_input_data(ptr, input_node_name, ref input_data[0]);
        }
        // @brief 加载图片推理数据
        // @param input_node_name 输入节点名
        // @param image_data 图片矩阵
        // @param image_size 图片矩阵长度
        public void load_input_data(string input_node_name, byte[] image_data, ulong image_size)
        {
            ptr = NativeMethods.load_image_input_data(ptr, input_node_name, ref image_data[0], image_size);
        }
        // @brief 模型推理
        public void infer()
        {
            ptr = NativeMethods.core_infer(ptr);
        }
        // @brief 读取推理结果数据
        // @param output_node_name 输出节点名
        // @param data_size 输出数据长度
        // @return 推理结果数组
        public T[] read_infer_result<T>(string output_node_name, int data_size)
        {
            // 获取设定类型
            string t = typeof(T).ToString();
            // 新建返回值数组
            T[] result = new T[data_size];
            if (t == "System.Int32")
            { // 读取数据类型为整形数据
                int[] inference_result = new int[data_size];
                NativeMethods.read_infer_result_I32(ptr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
            else
            { // 读取数据类型为浮点型数据
                float[] inference_result = new float[data_size];
                NativeMethods.read_infer_result_F32(ptr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
        }
        // @brief 删除创建的地址
        public void delet()
        {
            NativeMethods.core_delet(ptr);
        }
    }
}
