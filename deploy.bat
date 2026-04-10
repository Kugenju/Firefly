@echo off
chcp 65001 >nul
echo =========================================
echo  Firefly Mod 构建与部署
echo =========================================
echo.

set PROJECT_DIR=F:\personal\game_t\firefly
set GODOT_EXE=F:\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe
set MOD_DIR=G:\SteamLibrary\steamapps\common\Slay the Spire 2\mods\Firefly

echo [步骤 1] 尝试使用 dotnet build -t:ExportPck 编译并导出...
echo.

cd /d "%PROJECT_DIR%"
dotnet build -t:ExportPck

if %errorlevel% equ 0 (
    echo.
    echo =========================================
    echo  部署成功！
    echo =========================================
    goto :show_status
) else (
    echo.
    echo [警告] dotnet build 失败，尝试使用 Godot 直接构建...
    echo.
    goto :godot_build
)

:godot_build
echo [步骤 2] 使用 Godot 构建解决方案...
"%GODOT_EXE%" --headless --build-solutions "%PROJECT_DIR%\project.godot"
if %errorlevel% neq 0 (
    echo [错误] Godot 构建失败！
    exit /b 1
)
echo [OK] 构建成功!
echo.

echo [步骤 3] 复制 DLL 和 JSON...
if not exist "%MOD_DIR%" mkdir "%MOD_DIR%"
copy /Y "%PROJECT_DIR%\.godot\mono\temp\bin\ExportRelease\win-x64\Firefly.dll" "%MOD_DIR%\Firefly.dll" >nul
copy /Y "%PROJECT_DIR%\Firefly.json" "%MOD_DIR%\Firefly.json" >nul
echo [OK] 文件复制完成!
echo.

echo [步骤 4] 导出 PCK 资源包...
"%GODOT_EXE%" --headless --export-pack "Windows Desktop" "%MOD_DIR%\Firefly.pck" "%PROJECT_DIR%\project.godot"
echo [OK] PCK 导出完成!
echo.

:show_status
echo =========================================
echo  部署状态:
echo =========================================
dir /-c "%MOD_DIR%" | findstr "Firefly"
echo.
echo Mod 目录: %MOD_DIR%
echo.
echo 构建完成！启动游戏查看效果。
pause
