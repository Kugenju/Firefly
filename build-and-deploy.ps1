# =============================================
# Firefly Mod 构建与部署脚本
# =============================================
# 使用方法:
#   .\build-and-deploy.ps1          - 完整构建并部署 DLL + PCK
#   .\build-and-deploy.ps1 -Quick   - 仅构建并部署 DLL
#   .\build-and-deploy.ps1 -Export  - 仅导出 PCK
# =============================================

param(
    [switch]$Quick,   # 快速模式：仅编译 DLL
    [switch]$Export   # 仅导出 PCK
)

$ErrorActionPreference = "Stop"

# 配置路径
$GodotExe = "F:\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe"
$ProjectDir = "F:\personal\game_t\firefly"
$ProjectFile = "$ProjectDir\project.godot"
$Sts2Dir = "G:\SteamLibrary\steamapps\common\Slay the Spire 2"
$ModDir = "$Sts2Dir\mods\Firefly"

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  Firefly Mod 构建与部署" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host ""

# 检查 Godot 是否存在
if (-not (Test-Path $GodotExe)) {
    Write-Error "找不到 Godot: $GodotExe"
    exit 1
}

# 创建 mod 目录
if (-not (Test-Path $ModDir)) {
    Write-Host "创建 Mod 目录: $ModDir" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $ModDir -Force | Out-Null
}

# 步骤 1: 构建解决方案 (除非仅导出 PCK)
if (-not $Export) {
    Write-Host "[1/3] 正在构建 .NET 解决方案..." -ForegroundColor Green
    & $GodotExe --headless --build-solutions "$ProjectFile" 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "构建失败！"
        exit 1
    }
    Write-Host "      构建成功!" -ForegroundColor Green
    Write-Host ""
    
    # 步骤 2: 复制 DLL 和 JSON
    Write-Host "[2/3] 正在部署到游戏目录..." -ForegroundColor Green
    
    $BuildOutput = "$ProjectDir\.godot\mono\temp\bin\Debug\firefly.dll"
    $JsonFile = "$ProjectDir\Firefly.json"
    
    if (Test-Path $BuildOutput) {
        Copy-Item $BuildOutput -Destination "$ModDir\Firefly.dll" -Force
        Write-Host "      已复制: Firefly.dll" -ForegroundColor Gray
    } else {
        Write-Warning "找不到构建输出: $BuildOutput"
    }
    
    if (Test-Path $JsonFile) {
        Copy-Item $JsonFile -Destination "$ModDir\Firefly.json" -Force
        Write-Host "      已复制: Firefly.json" -ForegroundColor Gray
    } else {
        Write-Warning "找不到: $JsonFile"
    }
    
    Write-Host "      部署完成!" -ForegroundColor Green
    Write-Host ""
}

# 步骤 3: 导出 PCK (除非快速模式)
if (-not $Quick) {
    Write-Host "[3/3] 正在导出 PCK 资源包..." -ForegroundColor Green
    
    $Env:IsInnerGodotExport = "true"
    $Env:MSBUILDDISABLENODEREUSE = "1"
    
    & $GodotExe --headless --export-pack "Windows Desktop" "$ModDir\Firefly.pck" "$ProjectFile" 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "      PCK 导出成功!" -ForegroundColor Green
    } else {
        Write-Warning "PCK 导出可能失败，请检查输出"
    }
    Write-Host ""
}

# 显示最终状态
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  部署状态:" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

$files = @("Firefly.dll", "Firefly.json", "Firefly.pck")
foreach ($file in $files) {
    $path = "$ModDir\$file"
    if (Test-Path $path) {
        $time = (Get-Item $path).LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
        Write-Host "  ✓ $file" -ForegroundColor Green -NoNewline
        Write-Host " ($time)" -ForegroundColor Gray
    } else {
        Write-Host "  ✗ $file (不存在)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Mod 目录: $ModDir" -ForegroundColor Yellow
Write-Host ""
Write-Host "构建完成！启动游戏查看效果。" -ForegroundColor Cyan
