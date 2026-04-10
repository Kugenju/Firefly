# Firefly Mod 构建与部署指南

## 快速开始（推荐方式）

### 方法一：使用 dotnet build（推荐）

这是最简单的方式，会自动处理编译和 PCK 导出：

```batch
# 完整部署（编译 + 导出 PCK）
dotnet build -t:ExportPck

# 或者使用 Deploy 目标（相同效果）
dotnet build -t:Deploy
```

### 方法二：使用批处理脚本

双击运行项目根目录的 `deploy.bat`，它会：
1. 首先尝试 `dotnet build -t:ExportPck`
2. 如果失败，自动回退到 Godot 构建方式

### 方法三：分步手动构建

如果上述方法都失败，可以分步执行：

#### 步骤 1：使用 Godot 构建

```powershell
& "F:\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe" `
  --headless --build-solutions "F:\personal\game_t\firefly\project.godot"
```

#### 步骤 2：复制文件

```powershell
$modDir = "G:\SteamLibrary\steamapps\common\Slay the Spire 2\mods\Firefly"

# 复制 DLL
Copy-Item "F:\personal\game_t\firefly\.godot\mono\temp\bin\ExportRelease\win-x64\Firefly.dll" `
  "$modDir\Firefly.dll" -Force

# 复制 JSON
Copy-Item "F:\personal\game_t\firefly\Firefly.json" `
  "$modDir\Firefly.json" -Force
```

#### 步骤 3：导出 PCK

```powershell
& "F:\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe" `
  --headless --export-pack "Windows Desktop" `
  "G:\SteamLibrary\steamapps\common\Slay the Spire 2\mods\Firefly\Firefly.pck" `
  "F:\personal\game_t\firefly\project.godot"
```

---

## 项目配置

### firefly.csproj 关键配置

```xml
<Project Sdk="Godot.NET.Sdk/4.5.1">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    
    <!-- 游戏目录配置 -->
    <Sts2Dir>G:\SteamLibrary\steamapps\common\Slay the Spire 2</Sts2Dir>
    <Sts2DataDir>$(Sts2Dir)\data_sts2_windows_x86_64</Sts2DataDir>
    
    <!-- Godot 路径 -->
    <GodotExe>F:\Godot\Godot_v4.5.1-stable_mono_win64\...</GodotExe>
  </PropertyGroup>

  <ItemGroup>
    <!-- 游戏引用 -->
    <Reference Include="sts2">
      <HintPath>$(Sts2DataDir)\sts2.dll</HintPath>
    </Reference>
    <Reference Include="0Harmony">
      <HintPath>$(Sts2DataDir)\0Harmony.dll</HintPath>
    </Reference>
    
    <!-- BaseLib NuGet 包 -->
    <PackageReference Include="Alchyr.Sts2.BaseLib" Version="0.2.8" />
  </ItemGroup>
</Project>
```

---

## 部署结构

构建完成后，游戏目录 `Slay the Spire 2/mods/Firefly/` 应包含：

| 文件 | 说明 | 来源 |
|------|------|------|
| `Firefly.dll` | 编译后的 C# 代码 | 构建输出 |
| `Firefly.json` | Mod 配置文件 | 项目根目录 |
| `Firefly.pck` | 资源包（本地化、脚本等） | Godot 导出 |

---

## 常见问题

### 问题 1：dotnet build 失败

**现象**：`dotnet build -t:ExportPck` 报错

**解决**：使用 `deploy.bat` 自动回退到 Godot 构建，或手动执行分步构建。

### 问题 2：找不到导出模板

**现象**：PCK 导出失败，提示缺少导出模板

**解决**：
1. 打开 Godot 编辑器
2. 项目 -> 导出
3. 添加 "Windows Desktop" 配置
4. 不需要设置导出路径（命令行会覆盖）

### 问题 3：代码修改后没有生效

**解决**：
1. 删除 `.godot/mono/temp/bin` 和 `.godot/mono/temp/obj` 目录
2. 重新运行构建命令

### 问题 4：找不到 Firefly.dll

Godot 默认构建到以下路径：

```
.godot/mono/temp/bin/ExportRelease/win-x64/Firefly.dll
```

---

## 参考文档

- [Godot 命令行教程](https://docs.godotengine.org/zh-cn/4.x/tutorials/editor/command_line_tutorial.html)
- [Godot .NET 导出](https://docs.godotengine.org/zh-cn/4.x/tutorials/scripting/c_sharp/c_sharp_basics.html)
- [BaseLib Modding Framework](https://github.com/Alchyr/BaseLib)
