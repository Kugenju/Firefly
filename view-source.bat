@echo off
chcp 65001 >nul

REM 杀戮尖塔2 反编译源码快速查看脚本
REM 用法: view-source.bat <类名或命名空间>

set DNSpyPath=F:\dnSpy\dnSpy\dnSpy.Console.exe
set GameDllPath=G:\SteamLibrary\steamapps\common\Slay the Spire 2\data_sts2_windows_x86_64\sts2.dll
set OutputDir=%TEMP%\sts2-decompiled

if "%~1"=="" (
    echo 用法: view-source.bat ^<类名或命名空间^>
    echo.
    echo 示例:
    echo   view-source.bat CombatCreature    ^(查看战斗生物基类^)
    echo   view-source.bat BlockInfo         ^(查看格挡相关^)
    echo   view-source.bat PowerModel        ^(查看Power基类^)
    echo   view-source.bat SlayTheSpire2.Combat.Powers  ^(查看整个Power命名空间^)
    exit /b 1
)

if not exist "%DNSpyPath%" (
    echo 错误: 找不到dnSpy.Console.exe
    echo 请确认路径: %DNSpyPath%
    exit /b 1
)

if not exist "%GameDllPath%" (
    echo 错误: 找不到sts2.dll
    echo 请确认路径: %GameDllPath%
    exit /b 1
)

echo 正在反编译 %~1 ...
echo 输出目录: %OutputDir%

if not exist "%OutputDir%" mkdir "%OutputDir%"

REM 使用dnSpy.Console导出指定类型的源码
"%DNSpyPath%" --output-dir "%OutputDir%" "%GameDllPath%" 2>nul

REM 查找并显示匹配的文件
set FoundCount=0
for /r "%OutputDir%" %%f in (*.cs) do (
    findstr /m /i "%~1" "%%f" >nul 2>&1
    if !errorlevel! equ 0 (
        set /a FoundCount+=1
        echo.
        echo ============================================
        echo 找到匹配: %%f
        echo ============================================
        type "%%f" | more
        if !FoundCount! geq 5 (
            echo.
            echo 已显示5个匹配结果，可能有更多...
            goto :end
        )
    )
)

if %FoundCount% equ 0 (
    echo 未找到包含 "%~1" 的源文件
    echo 尝试导出完整命名空间...
    "%DNSpyPath%" --output-dir "%OutputDir%" "%GameDllPath%"
)

:end
echo.
echo 反编译文件保存在: %OutputDir%
pause
