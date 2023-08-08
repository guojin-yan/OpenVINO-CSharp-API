using System;
using System.Collections.Generic;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenVinoSharp;
using OpenVinoSharp.model.Yolov8;
using System.Runtime.InteropServices;
using System.Drawing;

namespace AlxBoard_deploy_yolov8
{
    static void Main(string[] args)
    {
        // -------- Get OpenVINO runtime version --------

        OpenVinoSharp.Version version = Ov.get_openvino_version();

        Console.WriteLine("---- OpenVINO INFO----");
        Console.WriteLine("Description : {0}", version.description);
        Console.WriteLine("Build number: {0}", version.buildNumber);

        if (args.Length < 2)
        {
            Console.WriteLine("Please enter the complete command parameters: <type> <>model_path> <image_path> <device_name> <lable_path>");
        }
        string device_name = "AUTO";
        if (args.Length > 3)
        {
            device_name = args[3];
            Console.WriteLine("Set inference device  {0}.", args[3]);
        }
        else
        {
            Console.WriteLine("No inference device specified, default device set to AUTO.");
        }
        string lable = String.Empty;
        if (args.Length > 4)
        {
            lable = args[4];
        }

        if (args[0] == "det" || args[0] == "seg" || args[0] == "pose" || args[0] == "cls")
        {
            yolov8_infer(args[0], args[1], args[2], device_name, lable);
        }
        else
        {
            Console.WriteLine("Please specify the model prediction type, such as 'det'、'seg'、'pose'、'cls'");
        }

    }

    static void yolov8_infer(string flg, string model_path, string image_path, string device, string classer_path)
    {
        // -------- Step 1. Initialize OpenVINO Runtime Core --------
        Core core = new Core();
        // -------- Step 2. Read a model --------
        Console.WriteLine("[INFO] Loading model files: {0}", model_path);
        Model model = core.read_model(model_path);
        print_model_info(model);

        // -------- Step 3. Loading a model to the device --------
        CompiledModel compiled_model = core.compiled_model(model, device);

        // -------- Step 4. Create an infer request --------
        InferRequest infer_request = compiled_model.create_infer_request();
        // -------- Step 5. Process input images --------
        Console.WriteLine("[INFO] Read image  files: {0}", image_path);
        Mat image = new Mat(image_path); // Read image by opencvsharp
        int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
        Mat max_image = Mat.Zeros(new OpenCvSharp.Size(max_image_length, max_image_length), MatType.CV_8UC3);
        Rect roi = new Rect(0, 0, image.Cols, image.Rows);
        image.CopyTo(new Mat(max_image, roi));
        float[] factors = new float[4];
        factors[0] = factors[1] = (float)(max_image_length / 640.0);
        factors[2] = image.Rows;
        factors[3] = image.Cols;

        // -------- Step 6. Set up input --------
        Tensor input_tensor = infer_request.get_input_tensor();
        Shape input_shape = input_tensor.get_shape();
        Mat input_mat = CvDnn.BlobFromImage(max_image, 1.0 / 255.0, new Size(input_shape[2], input_shape[3]), 0, true, false);
        float[] input_data = new float[input_shape[1] * input_shape[2] * input_shape[3]];
        Marshal.Copy(input_mat.Ptr(0), input_data, 0, input_data.Length);
        input_tensor.set_data<float>(input_data);


        // -------- Step 7. Do inference synchronously --------

        infer_request.infer();

        // -------- Step 9. Process output --------
        Console.WriteLine();
        if (flg == "det")
        {
            Tensor output_tensor = infer_request.get_output_tensor();
            int output_length = (int)output_tensor.get_size();
            float[] output_data = output_tensor.get_data<float>(output_length);

            ResultProcess process = new ResultProcess(factors, 80);
            Result result = process.process_det_result(output_data);
            process.read_class_names(classer_path);

            process.print_result(result);

            if (classer_path != String.Empty)
            {
                process.read_class_names(classer_path);
                Mat result_image = process.draw_det_result(result, image);
                Cv2.ImShow("result", result_image);
                Cv2.WaitKey(0);
            }

        }
        else if (flg == "seg")
        {
            Tensor output_tensor_det = infer_request.get_tensor("output0");
            int output_length_det = (int)output_tensor_det.get_size();
            float[] output_data_det = output_tensor_det.get_data<float>(output_length_det);

            Tensor output_tensor_pro = infer_request.get_tensor("output1");
            int output_length_pro = (int)output_tensor_pro.get_size();
            float[] output_data_pro = output_tensor_pro.get_data<float>(output_length_pro);

            ResultProcess process = new ResultProcess(factors, 80);
            Result result = process.process_seg_result(output_data_det, output_data_pro);

            process.print_result(result);

            if (classer_path != String.Empty)
            {
                process.read_class_names(classer_path);
                Mat result_image = process.draw_seg_result(result, image);
                Cv2.ImShow("result", result_image);
                Cv2.WaitKey(0);
            }

        }
        else if (flg == "pose")
        {
            Tensor output_tensor = infer_request.get_output_tensor();
            int output_length = (int)output_tensor.get_size();
            float[] output_data = output_tensor.get_data<float>(output_length);

            ResultProcess process = new ResultProcess(factors, 80);
            Result result = process.process_pose_result(output_data);


            Mat result_image = process.draw_pose_result(result, image, 0.2);
            process.print_result(result);
            Cv2.ImShow("result", result_image);
            Cv2.WaitKey(0);
        }
        else if (flg == "cls")
        {
            Tensor output_tensor = infer_request.get_output_tensor();
            int output_length = (int)output_tensor.get_size();
            float[] output_data = output_tensor.get_data<float>(output_length);

            ResultProcess process = new ResultProcess(factors, 80);
            KeyValuePair<int, float>[] result = process.process_cls_result(output_data);

            process.print_result(result);

        }
    }

    /// <summary>
    /// Output relevant information of the model
    /// </summary>
    /// <param name="model"> Model class</param>
    static void print_model_info(Model model)
    {
        Console.WriteLine("[INFO] model name: {0}", model.get_friendly_name());

        Node input_node = model.get_const_input(0);
        Console.WriteLine("[INFO]    inputs:");
        Console.WriteLine("[INFO]      input name: {0}", input_node.get_name());
        Console.WriteLine("[INFO]      input type: {0}", input_node.get_type().to_string());
        Console.WriteLine("[INFO]      input shape: {0}", input_node.get_shape().to_string());
        input_node.dispose();
        Node output_node = model.get_const_output(0);
        Console.WriteLine("[INFO]    outputs:");
        Console.WriteLine("[INFO]      output name: {0}", output_node.get_name());
        Console.WriteLine("[INFO]      output type: {0}", output_node.get_type().to_string());
        Console.WriteLine("[INFO]      output shape: {0}", output_node.get_shape().to_string());
        output_node.dispose();
    }
}
