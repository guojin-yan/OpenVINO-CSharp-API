// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.preprocess;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// PrePostProcessor 预处理测试 / PrePostProcessor tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class PrePostProcessorTests
    {
        static PrePostProcessorTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void PrePostProcessor_Build_WithValidModel_ReturnsModel()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var originalModel = core.read_model("model/yolo26n.xml");
            
            using var preprocessor = new PrePostProcessor(originalModel);
            var inputInfo = preprocessor.get_input_info();
            var tensorInfo = inputInfo.get_tensor_info();
            tensorInfo.set_layout(Layout.NHWC);

            // Act
            using var processedModel = preprocessor.build();

            // Assert
            Assert.NotNull(processedModel);
            Assert.True(processedModel.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void PreprocessSteps_Scale_AddsScaleOperation()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var originalModel = core.read_model("model/yolo26n.xml");
            using var preprocessor = new PrePostProcessor(originalModel);
            
            var inputInfo = preprocessor.get_input_info();
            var steps = inputInfo.get_preprocess_steps();

            // Act - 不应该抛出异常 / Should not throw
            steps.scale(1.0f / 255.0f);

            // Assert
            Assert.True(true, "缩放操作应该成功添加 / Scale operation should be added successfully");
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void PreprocessSteps_Mean_AddsMeanOperation()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var originalModel = core.read_model("model/yolo26n.xml");
            using var preprocessor = new PrePostProcessor(originalModel);
            
            var inputInfo = preprocessor.get_input_info();
            var steps = inputInfo.get_preprocess_steps();

            // Act - 不应该抛出异常 / Should not throw
            steps.mean(0.5f);

            // Assert
            Assert.True(true, "均值操作应该成功添加 / Mean operation should be added successfully");
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void PreprocessSteps_Resize_AddsResizeOperation()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var originalModel = core.read_model("model/yolo26n.xml");
            using var preprocessor = new PrePostProcessor(originalModel);
            
            var inputInfo = preprocessor.get_input_info();
            var steps = inputInfo.get_preprocess_steps();

            // Act - 不应该抛出异常 / Should not throw
            steps.resize(ResizeAlgorithm.RESIZE_LINEAR);

            // Assert
            Assert.True(true, "缩放操作应该成功添加 / Resize operation should be added successfully");
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void PreprocessSteps_MultiChannelScale_AddsOperation()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var originalModel = core.read_model("model/yolo26n.xml");
            using var preprocessor = new PrePostProcessor(originalModel);
            
            var inputInfo = preprocessor.get_input_info();
            var steps = inputInfo.get_preprocess_steps();

            // Act - 多通道缩放 / Multi-channel scale
            steps.scale_multi_channels(new float[] { 1.0f / 255.0f, 1.0f / 255.0f, 1.0f / 255.0f });

            // Assert
            Assert.True(true, "多通道缩放操作应该成功添加 / Multi-channel scale should be added");
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void PreprocessSteps_Pad_AddsPadOperation()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var originalModel = core.read_model("model/yolo26n.xml");
            using var preprocessor = new PrePostProcessor(originalModel);
            
            var inputInfo = preprocessor.get_input_info();
            var steps = inputInfo.get_preprocess_steps();

            // Act - 填充操作 / Padding operation
            steps.pad(
                pads_begin: new int[] { 0, 0, 1, 1 },
                pads_end: new int[] { 0, 0, 1, 1 },
                value: 0.0f,
                mode: PaddingMode.CONSTANT);

            // Assert
            Assert.True(true, "填充操作应该成功添加 / Pad operation should be added");
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void InputTensorInfo_SetColorFormat_SetsFormat()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var originalModel = core.read_model("model/yolo26n.xml");
            using var preprocessor = new PrePostProcessor(originalModel);
            
            var inputInfo = preprocessor.get_input_info();
            var tensorInfo = inputInfo.get_tensor_info();

            // Act - 不应该抛出异常 / Should not throw
            tensorInfo.set_color_format(ColorFormat.RGB);

            // Assert
            Assert.True(true, "颜色格式应该成功设置 / Color format should be set");
        }
    }
}
