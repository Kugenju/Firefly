# 环境配置检查报告

> 对照教程：https://glitchedreme.github.io/SlayTheSpire2ModdingTutorials/docs/01-env-setup/
> 检查日期：2026-04-09

---

## 1. 必需工具安装状态

| 工具 | 版本要求 | 状态 | 说明 |
|------|----------|------|------|
| Godot | 4.5.1 .NET (Mono) | ⚠️ 需手动安装 | 项目已配置，但需用户本地安装 |
| .NET SDK | 9.0+ | ⚠️ 需手动安装 | 项目使用 net9.0 目标框架 |
| IDE | VS Code / Rider / VS | ⚠️ 需手动安装 | 推荐使用 Rider 或 VS Code |

### 检查 Godot 安装
```powershell
# 验证 Godot 版本
& "F:\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe" --version
```

### 检查 .NET SDK 安装
```powershell
dotnet --version  # 应显示 9.x.x
```

---

## 2. 项目文件检查

### ✅ Firefly.json (Mod 清单文件)
**位置**: `F:\personal\game_t\firefly\Firefly.json`

**状态**: ✅ 已正确配置

```json
{
  "id": "Firefly",
  "name": "流萤角色 Mod",
  "author": "Kugenju",
  "description": "添加流萤角色 - 来自《崩坏：星穹铁道》的格拉默铁骑。包含独特的灼烧机制和完全燃烧形态。需要 BaseLib 作为依赖。",
  "version": "1.0.0",
  "has_pck": true,
  "has_dll": true,
  "dependencies": ["BaseLib"],
  "affects_gameplay": true
}
```

**检查项**:
- [x] `id` 与项目名一致 (Firefly)
- [x] `has_dll` 设置为 true
- [x] `has_pck` 设置为 true
- [x] 声明了依赖 (BaseLib)

---

### ✅ firefly.csproj (项目配置文件)
**位置**: `F:\personal\game_t\firefly\firefly.csproj`

**状态**: ✅ 已正确配置，但有需要用户修改的部分

```xml
<Project Sdk="Godot.NET.Sdk/4.5.1">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>true</ImplicitUsings>
    <LangVersion>12.0</LangVersion>
    <Nullable>enable</Nullable>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <EnableDynamicLoading>true</EnableDynamicLoading>

    <!-- ⚠️ 需要修改: 杀戮尖塔2目录 - 请根据你的安装路径修改 -->
    <Sts2Dir>G:\SteamLibrary\steamapps\common\Slay the Spire 2</Sts2Dir>
    <Sts2DataDir>$(Sts2Dir)\data_sts2_windows_x86_64</Sts2DataDir>
  </PropertyGroup>

  <ItemGroup>
    <!-- 游戏引用 -->
    <Reference Include="sts2">
      <HintPath>$(Sts2DataDir)\sts2.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="0Harmony">
      <HintPath>$(Sts2DataDir)\0Harmony.dll</HintPath>
      <Private>false</Private>
    </Reference>
    
    <!-- BaseLib NuGet 包 -->
    <PackageReference Include="Alchyr.Sts2.BaseLib" Version="*" />
  </ItemGroup>

  <!-- ✅ 自动复制dll和json到游戏目录 -->
  <Target Name="CopyMod" AfterTargets="PostBuildEvent">
    <Message Text="Installing mod to $(Sts2Dir)\mods\Firefly..." Importance="high" />
    <MakeDir Directories="$(Sts2Dir)\mods\Firefly" />
    <Copy SourceFiles="$(TargetPath)" DestinationFolder="$(Sts2Dir)\mods\Firefly\" />
    <Copy SourceFiles="Firefly.json" DestinationFolder="$(Sts2Dir)\mods\Firefly\" />
    <Message Text="Mod installed successfully!" Importance="high" />
  </Target>
</Project>
```

**需要修改的部分**:
- [ ] `<Sts2Dir>` 需要改为你的实际游戏安装路径
  - 当前: `G:\SteamLibrary\steamapps\common\Slay the Spire 2`
  - 示例路径: `C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2`

**检查项**:
- [x] Sdk 版本正确 (Godot.NET.Sdk/4.5.1)
- [x] TargetFramework 正确 (net9.0)
- [x] 引用了 sts2.dll
- [x] 引用了 0Harmony.dll
- [x] 添加了 BaseLib NuGet 包
- [x] 配置了自动复制到 mods 文件夹

---

### ✅ Entry.cs (Mod 入口类)
**位置**: `F:\personal\game_t\firefly\Scripts\Entry.cs`

**状态**: ✅ 已正确配置

```csharp
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace Firefly.Scripts;

[ModInitializer("Init")]
public static class Entry
{
    public static void Init()
    {
        Log.Info("========================================");
        Log.Info("Firefly Mod - 流萤角色正在初始化...");
        Log.Info("========================================");

        try
        {
            // 启用 Harmony Patch
            var harmony = new Harmony("firefly.character.mod");
            harmony.PatchAll();
            Log.Info("Harmony patches applied.");

            // 注册脚本
            ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
            Log.Info("Scripts registered.");

            Log.Info("流萤角色 Mod 初始化完成!");
        }
        catch (Exception ex)
        {
            Log.Error($"流萤角色 Mod 初始化失败: {ex.Message}");
            Log.Error(ex.StackTrace ?? "No stack trace available");
        }
    }
}
```

**检查项**:
- [x] 使用了 `[ModInitializer("Init")]` 属性
- [x] 命名空间正确 (Firefly.Scripts)
- [x] 初始化了 Harmony
- [x] 调用了 ScriptManagerBridge.LookupScriptsInAssembly
- [x] 包含错误处理

---

### ✅ project.godot (Godot 项目配置)
**位置**: `F:\personal\game_t\firefly\project.godot`

**状态**: ✅ 已正确配置

```ini
config_version=5

[application]
config/name="Firefly"
config/features=PackedStringArray("4.5", "Mobile")
config/icon="res://icon.svg"

[dotnet]
project/assembly_name="Firefly"

[rendering]
renderer/rendering_method="mobile"
```

**检查项**:
- [x] Godot 版本正确 (4.5)
- [x] 渲染器使用 Mobile (与游戏一致)
- [x] 配置了 assembly_name

---

## 3. 缺失的配置（与教程对比）

### ⚠️ 可选增强：Godot 命令行导出 PCK 配置

教程中提到了可以通过命令行导出 PCK，当前项目未配置。如果需要，可以添加到 `.csproj`:

```xml
<PropertyGroup>
  <!-- 添加 Godot 可执行文件路径 -->
  <GodotExe>F:\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe</GodotExe>
</PropertyGroup>

<!-- 导出 PCK 目标 -->
<Target Name="ExportPck">
  <Message Text="Exporting PCK..." Importance="high" />
  <Exec Command="&quot;$(GodotExe)&quot; --headless --export-pack &quot;Windows Desktop&quot; &quot;$(Sts2Dir)/mods/Firefly/Firefly.pck&quot;"
    EnvironmentVariables="IsInnerGodotExport=true;MSBUILDDISABLENODEREUSE=1"
    ContinueOnError="WarnAndContinue" />
</Target>
```

使用方法:
```bash
dotnet build -t:ExportPck  # 导出 PCK
dotnet build               # 仅编译 DLL
```

---

## 4. 项目模板使用

教程提到可以使用官方模板:
```bash
dotnet new install Alchyr.Sts2.Templates
```

当前项目 **未使用模板**，而是手动配置。这是可行的，因为项目结构已经完整。

---

## 5. 环境配置检查清单

### 必须完成（首次使用前）

- [ ] **安装 Godot 4.5.1 Mono**
  - 下载地址: https://godotengine.org/download/windows
  - 注意选择 .NET 版本

- [ ] **安装 .NET 9.0 SDK**
  ```powershell
  winget install Microsoft.DotNet.SDK.9
  # 或访问 https://dotnet.microsoft.com/download
  ```

- [ ] **修改 firefly.csproj 中的 Sts2Dir 路径**
  ```xml
  <Sts2Dir>你的实际游戏路径</Sts2Dir>
  ```

- [ ] **确保游戏已安装并找到安装路径**
  - 默认 Steam 路径: `C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2`

### 可选配置

- [ ] **安装 IDE 插件**
  - VS Code: C# Dev Kit, Godot Tools
  - Rider: 内置 Godot 支持

- [ ] **配置 Godot 命令行导出（如需自动导出 PCK）**

---

## 6. 构建和测试步骤

### 首次构建
```powershell
# 1. 进入项目目录
cd F:\personal\game_t\firefly

# 2. 还原 NuGet 包
dotnet restore

# 3. 构建项目（会自动复制到游戏 mods 目录）
dotnet build
```

### 验证安装
构建成功后，检查以下文件是否存在:
```
Slay the Spire 2/mods/Firefly/
├── Firefly.dll       # 编译的代码
├── Firefly.json      # Mod 清单
└── Firefly.pck       # Godot 资源包（如有资源）
```

### 启动游戏测试
1. 运行游戏
2. 首次启动会提示是否开启 Mod，选择"是"
3. 游戏会关闭，再次启动
4. 检查右下角是否显示"已加载模组"

---

## 7. 常见问题

### Q: 找不到 sts2.dll
**解决**: 检查 `Sts2Dir` 路径是否正确，确保路径使用 `\` 或 `/`

### Q: BaseLib 包还原失败
**解决**: 确保有网络连接，或手动添加 NuGet 源

### Q: 游戏没有加载 Mod
**解决**:
1. 检查 `Firefly.json` 中的 `id` 是否为 "Firefly"
2. 检查 mods 文件夹结构是否正确
3. 查看游戏日志: `%APPDATA%/SlayTheSpire2/logs/`

### Q: Godot 版本不匹配
**解决**: 必须使用 Godot 4.5.1，其他版本打包的 .pck 会被拒绝

---

## 总结

| 检查项 | 状态 |
|--------|------|
| Firefly.json | ✅ 完整 |
| firefly.csproj | ⚠️ 需要修改 Sts2Dir 路径 |
| Entry.cs | ✅ 完整 |
| project.godot | ✅ 完整 |
| Godot 4.5.1 | ⚠️ 需手动安装 |
| .NET 9.0 SDK | ⚠️ 需手动安装 |

**项目结构已经完整，只需要:**
1. 安装 Godot 4.5.1 Mono
2. 安装 .NET 9.0 SDK
3. 修改 `firefly.csproj` 中的 `Sts2Dir` 为你自己的游戏路径
4. 运行 `dotnet build` 构建
