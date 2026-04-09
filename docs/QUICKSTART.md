# 杀戮尖塔2 Mod开发 - 快速开始指南

## 环境要求

| 工具 | 版本 | 下载链接 |
|------|------|----------|
| .NET SDK | 9.0+ | https://dotnet.microsoft.com/download |
| Godot | 4.5.1 .NET | https://godotengine.org/download/windows |
| 游戏本体 | Steam版 | Steam库中安装 |

> ⚠️ **重要**: 必须使用 Godot 4.5.1，其他版本打包的 .pck 文件会被游戏拒绝！

---

## 快速配置（推荐）

### 方式1: 使用自动化脚本（推荐）

**双击运行（英文脚本，无编码问题）:**
```batch
# 配置环境
scripts\setup.bat

# 构建并安装
scripts\build.bat
```

脚本会自动：
- ✅ 检查 .NET SDK 是否安装
- ✅ 检查 Godot 是否安装
- ✅ 查找杀戮尖塔2游戏目录
- ✅ 创建 `mods` 文件夹
- ✅ 设置 `STS2_PATH` 环境变量

---

## 手动配置

如果自动化脚本无法工作，按以下步骤手动配置：

### 步骤1: 安装 .NET 9.0 SDK

```powershell
# 使用 winget (Windows 10/11)
winget install Microsoft.DotNet.SDK.9

# 验证安装
dotnet --version  # 应显示 9.x.x
```

### 步骤2: 安装 Godot 4.5.1

1. 访问 https://godotengine.org/download/windows
2. 下载 **Godot 4.5.1 - .NET 版本** (约 120MB)
3. 解压到任意目录，例如 `C:\Godot\`
4. **可选**: 将 Godot 目录添加到 PATH 环境变量

### 步骤3: 设置环境变量

```powershell
# 设置游戏路径环境变量（修改为你的实际路径）
[Environment]::SetEnvironmentVariable("STS2_PATH", "C:\Program Files (x86)\Steam\steamapps\common\SlayTheSpire2", "User")
```

或者手动设置：
1. Win+R 输入 `sysdm.cpl` 回车
2. 高级 → 环境变量
3. 用户变量 → 新建
4. 变量名: `STS2_PATH`
5. 变量值: 你的游戏路径

### 步骤4: 创建游戏 mods 目录

```powershell
$sts2Path = "你的游戏路径"
New-Item -ItemType Directory -Path "$sts2Path\mods" -Force
```

---

## 验证配置

运行以下命令验证环境：

```powershell
# 检查 .NET
dotnet --version

# 检查环境变量
echo $env:STS2_PATH

# 检查游戏文件
Test-Path "$env:STS2_PATH\SlayTheSpire2_Data\Managed\sts2.dll"
```

---

## 第一个构建

### 构建项目

```powershell
# 切换到项目目录
cd F:\personal\game_t\slaymod

# 还原依赖
dotnet restore

# 构建
dotnet build
```

### 使用一键构建脚本（英文，无编码问题）

```batch
# 双击运行
scripts\build.bat

# Release 版本
scripts\build.bat -Release

# 仅构建不安装
scripts\build.bat -NoInstall
```

---

## 项目结构说明

```
slaymod/
├── MySts2Mod.csproj          # 项目文件（需要配置引用路径）
├── MySts2Mod.json            # Mod清单文件
├── src/                      # 源代码
│   ├── ModEntry.cs           # Mod入口
│   ├── cards/                # 卡牌代码
│   ├── relics/               # 遗物代码
│   ├── characters/           # 角色代码
│   └── utils/                # 工具类
├── assets/                   # 美术资源（需要自己准备）
│   ├── cards/
│   ├── relics/
│   └── characters/
├── export/                   # 导出目录（PCK文件）
└── scripts/                  # 辅助脚本
    ├── setup-environment.ps1     # 环境配置
    ├── build-and-install.ps1     # 构建安装
    └── create-pck.bat            # 创建PCK包
```

---

## 配置项目引用

编辑 `MySts2Mod.csproj`，确保引用路径正确：

```xml
<Project Sdk="Godot.NET.Sdk/4.5.1">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
  </PropertyGroup>

  <ItemGroup>
    <!-- 方式1: 使用环境变量 (推荐) -->
    <Reference Include="sts2">
      <HintPath>$(STS2_PATH)\SlayTheSpire2_Data\Managed\sts2.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="Godot">
      <HintPath>$(STS2_PATH)\SlayTheSpire2_Data\Managed\Godot.dll</HintPath>
      <Private>false</Private>
    </Reference>

    <!-- 方式2: 使用绝对路径 -->
    <!--
    <Reference Include="sts2">
      <HintPath>C:\Program Files (x86)\Steam\steamapps\common\SlayTheSpire2\SlayTheSpire2_Data\Managed\sts2.dll</HintPath>
    </Reference>
    -->
  </ItemGroup>
</Project>
```

---

## 安装Mod到游戏

### 方式1: 自动安装（推荐）

```powershell
powershell -ExecutionPolicy Bypass -File scripts\build-and-install.ps1
```

### 方式2: 手动复制

```powershell
# 源文件
$source = "F:\personal\game_t\slaymod\bin\Debug\net9.0\MySts2Mod.dll"
$manifest = "F:\personal\game_t\slaymod\MySts2Mod.json"

# 目标目录
$dest = "$env:STS2_PATH\mods\MySts2Mod"

# 复制
Copy-Item $source $dest -Force
Copy-Item $manifest $dest -Force
```

### 方式3: 直接拖拽

1. 打开 `bin\Debug\net9.0\`
2. 复制 `MySts2Mod.dll`
3. 粘贴到 `Steam\steamapps\common\SlayTheSpire2\mods\MySts2Mod\`
4. 同时复制 `MySts2Mod.json`

---

## 启动游戏测试

```powershell
# 直接启动
& "$env:STS2_PATH\SlayTheSpire2.exe"

# 或者使用Steam启动（会自动应用启动参数）
start steam://rungameid/2577044760
```

Steam启动参数（可选）：
```
--nomods          # 禁用所有Mod（排查问题用）
--fastmp          # 快速多人模式
--seed 12345      # 指定种子
```

---

## 常见问题

### Q: 找不到 .NET SDK
```powershell
# 检查是否安装
dotnet --info

# 如果没有，通过winget安装
winget install Microsoft.DotNet.SDK.9
```

### Q: 找不到 sts2.dll
```powershell
# 手动指定路径到项目文件
# 编辑 MySts2Mod.csproj，使用绝对路径
```

### Q: 构建成功但Mod不加载
- 检查 `MySts2Mod.json` 中的 `id` 是否与文件夹名一致
- 检查 `has_dll` 是否为 `true`
- 查看游戏日志: `%APPDATA%/SlayTheSpire2/logs/`

### Q: 游戏崩溃
- 使用 `--nomods` 启动游戏，确认是Mod问题
- 检查代码是否有空引用异常
- 使用 `Log.Info()` 添加日志排查

---

## 下一步

环境配置完成后，你可以：

1. **阅读 `README.md`** - 了解Mod开发基础
2. **阅读 `CHARACTER_GUIDE.md`** - 学习制作新角色
3. **打开项目** - 使用 VS / Rider / VS Code 编辑代码
4. **反编译学习** - 用 ILSpy 打开 `sts2.dll` 查看游戏源码

---

## 有用的命令

```powershell
# 快速构建
dotnet build

# 清理构建
dotnet clean

# 发布版本
dotnet publish -c Release

# 查看详细日志
dotnet build -v d

# 强制重新构建
dotnet build --no-incremental
```
