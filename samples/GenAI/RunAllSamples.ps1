# Copyright (c) 2026 Guojin Yan
# Licensed under the Apache-2.0 License.

<#
.SYNOPSIS
Runs all OpenVINO GenAI C# samples and writes one log per sample.

.DESCRIPTION
Use this script after preparing the LLM model, Whisper model, VLM model, WAV
audio, and RGB BMP/PPM image. By default, samples restore the published GenAI
runtime NuGet package. Pass -RuntimeDir only when validating a local native
runtime build. The script executes every GenAI sample in a deterministic order
and writes one log file per scenario.

准备好 LLM 模型、Whisper 模型、VLM 模型、WAV 音频和 RGB BMP/PPM 图片后运行该脚本。
默认情况下，示例会还原已发布的 GenAI runtime NuGet 包；只有验证本地 native runtime
构建时才需要传入 -RuntimeDir。脚本会按固定顺序执行全部 GenAI 示例，并为每个场景保存
一份日志。
#>

[CmdletBinding()]
param(
    [string]$RuntimeDir = "",

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

    [string]$OutputDir = "out/genai-samples-validation",

    [string]$Configuration = "Release",

    [string]$RuntimeIdentifier = "win-x64"
)

$ErrorActionPreference = "Stop"
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..")
$outputRoot = Join-Path $repoRoot $OutputDir
if (Test-Path -LiteralPath $outputRoot) {
    Remove-Item -LiteralPath $outputRoot -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $outputRoot | Out-Null
$publishRoot = Join-Path $outputRoot "publish"

$requiredPaths = @($LlmModelDir, $WhisperModelDir, $VlmModelDir, $AudioPath, $ImagePath)
if (-not [string]::IsNullOrWhiteSpace($RuntimeDir)) {
    $requiredPaths = @($RuntimeDir) + $requiredPaths
}

foreach ($path in $requiredPaths) {
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Required path does not exist: $path"
    }
}

if (-not [string]::IsNullOrWhiteSpace($RuntimeDir)) {
    $env:OPENVINO_GENAI_RUNTIME_DIR = (Resolve-Path -LiteralPath $RuntimeDir).Path
}
else {
    Remove-Item Env:OPENVINO_GENAI_RUNTIME_DIR -ErrorAction SilentlyContinue
}

$env:OPENVINO_GENAI_LLM_MODEL_DIR = (Resolve-Path -LiteralPath $LlmModelDir).Path
$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = (Resolve-Path -LiteralPath $WhisperModelDir).Path
$env:OPENVINO_GENAI_VLM_MODEL_DIR = (Resolve-Path -LiteralPath $VlmModelDir).Path
$env:OPENVINO_GENAI_AUDIO_PATH = (Resolve-Path -LiteralPath $AudioPath).Path
$env:OPENVINO_GENAI_IMAGE_PATH = (Resolve-Path -LiteralPath $ImagePath).Path
$env:OPENVINO_GENAI_DEVICE = $Device
Remove-Item Env:OPENVINO_GENAI_C_LIBRARY -ErrorAction SilentlyContinue

function ConvertFrom-Utf8Base64 {
    param([Parameter(Mandatory = $true)][string]$Value)
    return [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($Value))
}

$promptGreedyZh = ConvertFrom-Utf8Base64 "6K+355So5Lit5paH55So5Lik5Y+l6K+d5LuL57uNIE9wZW5WSU5P44CC"
$promptChatZh = ConvertFrom-Utf8Base64 "6K+355So5Lit5paH5YiX5Ye65LiJ5LiqIE9wZW5WSU5PIOWFs+mUruivjeOAgg=="
$promptVlmZh = ConvertFrom-Utf8Base64 "6K+355So5Lit5paH5o+P6L+w6L+Z5byg5Zu+54mH44CC"

function Invoke-Sample {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [string]$Project,

        [Parameter(Mandatory = $true)]
        [string[]]$Arguments,

        [string[]]$InputLines = @()
    )

    $logPath = Join-Path $outputRoot "$Name.log"
    $samplePublishDir = Join-Path $publishRoot $Name
    New-Item -ItemType Directory -Force -Path $samplePublishDir | Out-Null

    "===== $Name =====" | Tee-Object -FilePath $logPath
    "dotnet publish $Project --framework net8.0 -c $Configuration -r $RuntimeIdentifier --self-contained false -o $samplePublishDir" | Tee-Object -FilePath $logPath -Append
    & dotnet publish $Project --framework net8.0 -c $Configuration -r $RuntimeIdentifier --self-contained false -o $samplePublishDir 2>&1 | Tee-Object -FilePath $logPath -Append
    if ($LASTEXITCODE -ne 0) {
        throw "$Name publish failed with exit code $LASTEXITCODE. See $logPath"
    }

    $exeName = [System.IO.Path]::GetFileNameWithoutExtension($Project)
    $exePath = Join-Path $samplePublishDir "$exeName.exe"
    if (-not (Test-Path -LiteralPath $exePath)) {
        $dllPath = Join-Path $samplePublishDir "$exeName.dll"
        if (-not (Test-Path -LiteralPath $dllPath)) {
            throw "$Name publish output was not found in $samplePublishDir"
        }
        $command = "dotnet"
        $invokeArguments = @($dllPath) + $Arguments
    }
    else {
        $command = $exePath
        $invokeArguments = $Arguments
    }

    "$command $($Arguments -join ' ')" | Tee-Object -FilePath $logPath -Append

    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    try {
        if ($InputLines.Count -gt 0) {
            $InputLines | & $command @invokeArguments 2>&1 | Tee-Object -FilePath $logPath -Append
        }
        else {
            & $command @invokeArguments 2>&1 | Tee-Object -FilePath $logPath -Append
        }
    }
    finally {
        $ErrorActionPreference = $previousErrorActionPreference
    }

    if ($LASTEXITCODE -ne 0) {
        $logText = Get-Content -LiteralPath $logPath -Raw
        if ($logText -match "0x800711C7") {
            "Published executable was blocked by application control policy; retrying with dotnet run." | Tee-Object -FilePath $logPath -Append
            $runArguments = @("run", "--project", $Project, "--framework", "net8.0", "-c", $Configuration, "--") + $Arguments
            "dotnet $($runArguments -join ' ')" | Tee-Object -FilePath $logPath -Append

            $previousErrorActionPreference = $ErrorActionPreference
            $ErrorActionPreference = "Continue"
            try {
                if ($InputLines.Count -gt 0) {
                    $InputLines | & dotnet @runArguments 2>&1 | Tee-Object -FilePath $logPath -Append
                }
                else {
                    & dotnet @runArguments 2>&1 | Tee-Object -FilePath $logPath -Append
                }
            }
            finally {
                $ErrorActionPreference = $previousErrorActionPreference
            }
        }

        if ($LASTEXITCODE -ne 0) {
            throw "$Name failed with exit code $LASTEXITCODE. See $logPath"
        }
    }
}

Push-Location $repoRoot
try {
    Invoke-Sample "01-greedy" "samples/GenAI/TextGeneration/Greedy/Greedy.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "What is OpenVINO?",
        "--device", $Device,
        "--max-new-tokens", "8"
    )

    Invoke-Sample "02-beam-search" "samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "OpenVINO is",
        "--device", $Device,
        "--max-new-tokens", "8",
        "--beams", "2"
    )

    Invoke-Sample "03-multinomial" "samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "OpenVINO helps developers",
        "--device", $Device,
        "--max-new-tokens", "8",
        "--temperature", "0.7",
        "--top-p", "0.9",
        "--top-k", "20",
        "--seed", "7"
    )

    Invoke-Sample "04-streaming" "samples/GenAI/TextGeneration/Streaming/Streaming.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "List one OpenVINO benefit.",
        "--device", $Device,
        "--max-new-tokens", "8"
    )

    Invoke-Sample "05-benchmark" "samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", "OpenVINO is",
        "--device", $Device,
        "--max-new-tokens", "8",
        "--iterations", "1",
        "--warmup", "0"
    )

    Invoke-Sample "06-chat" "samples/GenAI/TextGeneration/Chat/Chat.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--device", $Device,
        "--max-new-tokens", "64",
        "--turn", "What is OpenVINO?"
    )

    Invoke-Sample "07-greedy-zh" "samples/GenAI/TextGeneration/Greedy/Greedy.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--prompt", $promptGreedyZh,
        "--device", $Device,
        "--max-new-tokens", "96"
    )

    Invoke-Sample "08-chat-zh" "samples/GenAI/TextGeneration/Chat/Chat.csproj" @(
        "--model", $env:OPENVINO_GENAI_LLM_MODEL_DIR,
        "--device", $Device,
        "--max-new-tokens", "96",
        "--turn", $promptChatZh
    )

    Invoke-Sample "09-whisper" "samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj" @(
        "--model", $env:OPENVINO_GENAI_WHISPER_MODEL_DIR,
        "--audio", $env:OPENVINO_GENAI_AUDIO_PATH,
        "--device", $Device,
        "--language", "en",
        "--task", "transcribe",
        "--timestamps", "true"
    )

    Invoke-Sample "10-vlm-single" "samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj" @(
        "--model", $env:OPENVINO_GENAI_VLM_MODEL_DIR,
        "--image", $env:OPENVINO_GENAI_IMAGE_PATH,
        "--device", $Device,
        "--prompt", "What colors are visible in this image? Answer with color names only.",
        "--max-new-tokens", "48"
    )

    Invoke-Sample "11-vlm-interactive" "samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj" @(
        "--model", $env:OPENVINO_GENAI_VLM_MODEL_DIR,
        "--image", $env:OPENVINO_GENAI_IMAGE_PATH,
        "--device", $Device,
        "--interactive", "true",
        "--max-new-tokens", "48"
    ) -InputLines @("What colors are visible in this image? Answer with color names only.", "/exit")

    Invoke-Sample "12-vlm-zh" "samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj" @(
        "--model", $env:OPENVINO_GENAI_VLM_MODEL_DIR,
        "--image", $env:OPENVINO_GENAI_IMAGE_PATH,
        "--device", $Device,
        "--prompt", $promptVlmZh,
        "--max-new-tokens", "96",
        "--allow-empty", "false"
    )
}
finally {
    Pop-Location
}

Write-Host "All GenAI samples completed. Logs: $outputRoot"
