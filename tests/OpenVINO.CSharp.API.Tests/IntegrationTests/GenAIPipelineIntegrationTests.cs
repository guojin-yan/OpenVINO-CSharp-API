// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using OpenVinoSharp.GenAI;
using Xunit;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// OpenVINO GenAI Pipeline 集成测试 / Integration tests for OpenVINO GenAI pipelines.
    /// </summary>
    public class GenAIPipelineIntegrationTests
    {
        private const string WhisperModelEnvironmentVariable = "OPENVINO_GENAI_WHISPER_MODEL_DIR";
        private const string VlmModelEnvironmentVariable = "OPENVINO_GENAI_VLM_MODEL_DIR";

        /// <summary>
        /// Whisper Pipeline 应能在提供模型时创建、配置并对短静音输入执行一次生成。
        /// Whisper pipeline should create, configure, and run one short-silence generation when a model is provided.
        /// </summary>
        [OpenVINOGenAIModelFact(WhisperModelEnvironmentVariable, "Whisper")]
        public void WhisperPipeline_CanGenerateShortSilence_WhenModelIsProvided()
        {
            string modelPath = OpenVINOGenAIModelFactAttribute.GetModelPath(WhisperModelEnvironmentVariable);

            using (var pipeline = new WhisperPipeline(modelPath, "CPU"))
            using (var config = pipeline.GetGenerationConfig())
            {
                config
                    .SetTask("transcribe")
                    .SetReturnTimestamps(false);
                pipeline.SetGenerationConfig(config);

                float[] silence = new float[16000];
                using (WhisperDecodedResults results = pipeline.Generate(silence, config))
                {
                    Assert.NotNull(results);
                    Assert.NotNull(results.GetString());
                }
            }
        }

        /// <summary>
        /// VLM Pipeline 应能在提供模型时创建、读写配置，并验证图像 Tensor 数组。
        /// VLM pipeline should create, read/write config, and validate the image tensor array when a model is provided.
        /// </summary>
        [OpenVINOGenAIModelFact(VlmModelEnvironmentVariable, "VLM")]
        public void VLMPipeline_CanCreateConfigureAndValidateImages_WhenModelIsProvided()
        {
            string modelPath = OpenVINOGenAIModelFactAttribute.GetModelPath(VlmModelEnvironmentVariable);

            using (var pipeline = new VLMPipeline(modelPath, "CPU"))
            using (GenerationConfig config = pipeline.GetGenerationConfig())
            {
                config.SetMaxNewTokens(1);
                pipeline.SetGenerationConfig(config);

                Assert.Throws<ArgumentException>(() => pipeline.Generate("Describe this image.", new Tensor[] { null! }, config));

                using (var history = new ChatHistory().AddUserMessage("Describe this image."))
                {
                    Assert.Throws<ArgumentException>(() => pipeline.GenerateWithHistory(history, new Tensor[] { null! }, config, null!));
                }
            }
        }
    }
}
