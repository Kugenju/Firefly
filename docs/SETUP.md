# 杀戮尖塔2 Mod 开发环境配置指南

## 1. 安装必要工具

### 1.1 Godot 4.5.1 .NET版本
1. 访问 https://godotengine.org/
2. 下载 **Godot 4.5.1** 的 **.NET** (Mono) 版本
3. 解压到 `F:\Godot\Godot_v4.5.1-stable_mono_win64\`
4. **注意**: 必须使用 4.5.1 版本，新版打包的.pck会被游戏拒绝

> 本项目已配置 Godot 路径: `F:\Godot\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe`

### 1.2 .NET 9.0 SDK
1. 访问 https://dotnet.microsoft.com/
2. 下载并安装 **.NET 9.0 SDK**

### 1.3 IDE (推荐)
- **JetBrains Rider** (最佳Godot C#体验)
- **Visual Studio 2022** (带Godot扩展)
- **VS Code** (带C# Dev Kit和Godot扩展)

## 2. 配置项目

### 2.1 设置环境变量
设置 `STS2_PATH` 环境变量指向游戏目录：

```powershell
# Windows PowerShell (临时)
$env:STS2_PATH = "C:\Program Files (x86)\Steam\steamapps\common\SlayTheSpire2"

# 或在系统设置中永久设置
```

### 2.2 修改 csproj 文件
编辑 `MySts2Mod.csproj`，确保引用路径正确：

```xml
<Reference Include="sts2">
  <HintPath>$(STS2_PATH)\SlayTheSpire2_Data\Managed\sts2.dll</HintPath>
</Reference>
```

或者使用绝对路径：

```xml
<Reference Include="sts2">
  <HintPath>C:\Program Files (x86)\Steam\steamapps\common\SlayTheSpire2\SlayTheSpire2_Data\Managed\sts2.dll</HintPath>
</Reference>
```

## 3. 创建 mods 目录

在杀戮尖塔2游戏目录下创建 `mods` 文件夹：

```
SlayTheSpire2/
├── SlayTheSpire2.exe
├── mods/                    <-- 创建这个文件夹
│   └── MySts2Mod/          <-- 你的Mod文件夹
│       ├── MySts2Mod.json  <-- manifest文件
│       ├── MySts2Mod.dll   <-- 编译后的代码
│       └── MySts2Mod.pck   <-- Godot资源包
└── ...
```

## 4. 构建和安装

### 4.1 使用 .NET CLI

```bash
# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 发布 (Release)
dotnet publish -c Release
```

### 4.2 使用 Godot 编辑器打包 .pck

1. 打开 Godot 4.5.1 编辑器
2. 创建新项目或导入现有项目
3. 将你的资源文件（图片、音频等）放入项目
4. 导出为 `.pck` 文件：
   - Project -> Export
   - 选择 "Export PCK/ZIP"
   - 输出到 `export/MySts2Mod.pck`

### 4.3 手动安装

构建完成后，复制以下文件到游戏 mods 目录：

```bash
# DLL 文件
bin/Debug/net9.0/MySts2Mod.dll -> mods/MySts2Mod/MySts2Mod.dll

# Manifest 文件
MySts2Mod.json -> mods/MySts2Mod/MySts2Mod.json

# PCK 文件 (如果有)
export/MySts2Mod.pck -> mods/MySts2Mod/MySts2Mod.pck
```

## 5. 调试技巧

### 5.1 查看游戏日志
游戏日志位置：
```
%APPDATA%/SlayTheSpire2/logs/
```

### 5.2 使用 Steam 启动参数
在 Steam 中右键游戏 -> 属性 -> 启动选项：

```
--nomods          # 禁用所有Mod（排查崩溃用）
--fastmp          # 快速多人模式测试
--seed 12345      # 指定随机种子
--log-file "C:\logs\sts2.log"  # 自定义日志路径
```

### 5.3 反编译游戏代码
使用 ILSpy 或 dnSpyEx 打开：
```
SlayTheSpire2_Data/Managed/sts2.dll
```

游戏源码未做混淆，可以直接查看类和方法。

## 6. 常见问题

### Q: Mod 没有加载？
- 检查 `MySts2Mod.json` 中的 `id` 是否与文件夹名一致
- 检查 `has_dll` 和 `has_pck` 设置是否正确
- 查看游戏日志中的错误信息

### Q: 引用 sts2.dll 出错？
- 确保路径正确（使用绝对路径或设置环境变量）
- 确保 `Private="false"` 属性存在（不要将游戏DLL复制到输出）

### Q: Godot 版本不兼容？
- **必须使用 Godot 4.5.1**，其他版本打包的 .pck 会被拒绝

### Q: 如何调试代码？
- 使用 `MegaCrit.Sts2.Core.Logging.Log` 类输出日志
- 示例: `Log.Info("调试信息");`

## 7. 推荐资源

- [STS2FirstMod](https://github.com/jiegec/STS2FirstMod) - 最小工作示例
- [ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2) - 项目模板
- [BaseLib-StS2](https://github.com/Alchyr/BaseLib-StS2) - 基础功能库
