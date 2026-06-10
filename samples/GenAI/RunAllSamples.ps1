# Copyright (c) 2026 Guojin Yan
# Licensed under the Apache-2.0 License.

<#
.SYNOPSIS
Runs all OpenVINO GenAI C# samples and writes one log per sample.

.DESCRIPTION
Use this script after preparing the native GenAI runtime, LLM model, Whisper
model, VLM model, WAV audio, and RGB BMP/PPM image. It executes every GenAI
sample in a deterministic order and writes one log file per scenario.

准备好 GenAI 原生运行时、LLM 模型、Whisper 模型、VLM 模型、WAV 音频和 RGB
BMP/PPM 图片后运行该脚本。脚本会按固定顺序执行全部 GenAI 示例，并为每个场景保存
一份日志。
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$RuntimeDir,

    [Parameter(Mandatory = $true)]
    [string]$LlmModelDir,

    [Parameter(Mandatory = $true)]
    [string]$WhisperModelDir,

    [Parameter(Mandatory = $true)]
    [string]$VlmModelDir,

    [Parameter(Mandatory = $true)]
    [string]$AudioPath,

    [Parameter(Mandatory = $true)]
    [string]$ImagePath,

    [string]$Device = "CPU",

    [string]$OutputDir = "out/genai-samples-validation"
)

$ErrorActionPreference = "Stop"
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..")
$outputRoot = Join-Path $repoRoot $OutputDir
New-Item -ItemType Directory -Force -Path $outputRoot | Out-Null

foreach ($path in @($RuntimeDir, $LlmModelDir, $WhisperModelDir, $VlmModelDir, $AudioPath, $ImagePath)) {
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Required path does not exist: $path"
    }
}

$env:OPENVINO_GENAI_RUNTIME_DIR = (Resolve-Path -LiteralPath $RuntimeDir).Path
$env:OPENVINO_GENAI_LLM_MODEL_DIR = (Resolve-Path -LiteralPath $LlmModelDir).Path
$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = (Resolve-Path -LiteralPath $WhisperModelDir).Path
$env:OPENVINO_GENAI_VLM_MODEL_DIR = (Resolve-Path -LiteralPath $VlmModelDir).Path
$env:OPENVINO_GENAI_AUDIO_PATH = (Resolve-Path -LiteralPath $AudioPath).Path
$env:OPENVINO_GENAI_IMAGE_PATH = (Resolve-Path -LiteralPath $ImagePath).Path
$env:OPENVINO_GENAI_DEVICE = $Device
Remove-Item Env:OPENVINO_GENAI_C_LIBRARY -ErrorAction SilentlyContinue

function Invoke-Sample {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [string[]]$Arguments,

        [string[]]$InputLines = @()
    )

    $logPath = Join-Path $outputRoot "$Name.log"
    "===== $Name =====" | Tee-Object -FilePath $logPath
    "dotnet $($Arguments -join ' ')" | Tee-Object -FilePath $logPath -Append

    if ($InputLines.Count -gt 0) {
        $InputLines | & dotnet @Arguments 2>&1 | Tee-Object -FilePath $logPath -Append
    }
    else {
        & dotnet @Arguments 2>&1 | Tee-Object -FilePath $logPath -Append
    }

    if ($LASTEXITCODE -ne 0) {
        throw "$Name failed with exit code $LASTEXITCODE. See $logPath"
    }
}

Push-Location $repoRoot
try {
    Invoke-Sample "01-greedy" @(
        "run", "--project", "samples/GenAI/TextGeneration/Greedy/Greedy.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "What is OpenVINO?",
        "--device", $Device,
        "--max-new-tokens", "8"
    )

    Invoke-Sample "02-beam-search" @(
        "run", "--project", "samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "OpenVINO is",
        "--device", $Device,
        "--max-new-tokens", "8",
        "--beams", "2"
    )

    Invoke-Sample "03-multinomial" @(
        "run", "--project", "samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "OpenVINO helps developers",
        "--device", $Device,
        "--max-new-tokens", "8",
        "--temperature", "0.7",
        "--top-p", "0.9",
        "--top-k", "20",
        "--seed", "7"
    )

    Invoke-Sample "04-streaming" @(
        "run", "--project", "samples/GenAI/TextGeneration/Streaming/Streaming.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "List one OpenVINO benefit.",
        "--device", $Device,
        "--max-new-tokens", "8"
    )

    Invoke-Sample "05-benchmark" @(
        "run", "--project", "samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "OpenVINO is",
        "--device", $Device,
        "--max-new-tokens", "8",
        "--iterations", "1",
        "--warmup", "0"
    )

    Invoke-Sample "06-chat" @(
        "run", "--project", "samples/GenAI/TextGeneration/Chat/Chat.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--device", $Device,
        "--max-new-tokens", "8"
    ) -InputLines @("What is OpenVINO?", "/exit")

    Invoke-Sample "07-whisper" @(
        "run", "--project", "samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_WHISPER_MODEL_DIR,
        "--audio", $env:OPENVINO_GENAI_AUDIO_PATH,
        "--device", $Device,
        "--language", "en",
        "--task", "transcribe",
        "--timestamps", "true"
    )

    Invoke-Sample "08-vlm-single" @(
        "run", "--project", "samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_VLM_MODEL_DIR,
        "--image", $env:OPENVINO_GENAI_IMAGE_PATH,
        "--device", $Device,
        "--prompt", "What colors are visible?",
        "--max-new-tokens", "8"
    )

    Invoke-Sample "09-vlm-interactive" @(
        "run", "--project", "samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj", "--framework", "net8.0", "--",
        "--model", $env:OPENVINO_GENAI_VLM_MODEL_DIR,
        "--image", $env:OPENVINO_GENAI_IMAGE_PATH,
        "--device", $Device,
        "--interactive", "true",
        "--max-new-tokens", "8"
    ) -InputLines @("Describe the image.", "/exit")
}
finally {
    Pop-Location
}

Write-Host "All GenAI samples completed. Logs: $outputRoot"
