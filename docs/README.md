# 杀戮尖塔2 (Slay the Spire 2) Mod 开发指南

> ⚠️ **重要提示**: 杀戮尖塔2使用 **Godot 4.5.1 + C#** 开发，与1代的 **Java + LibGDX** 完全不同！1代Mod无法直接迁移。

---

## 1. 技术栈概览

| 项目 | 杀戮尖塔1 | 杀戮尖塔2 |
|------|-----------|-----------|
| 游戏引擎 | LibGDX | Godot 4.5.1 (MegaDot) |
| 开发语言 | Java 8 | C# / .NET 9.0 |
| Mod加载器 | ModTheSpire | 内置Mod系统 |
| 资源格式 | JAR | .pck (Godot) + .dll (C#) |
| 动画格式 | Spine 3.4 JSON | Spine 4.2 二进制 (.skel) |

---

## 2. 开发环境准备

### 必需工具

1. **Godot 4.5.1 .NET版本** (Mono构建)
   - 下载: https://godotengine.org/
   - **必须使用4.5.1版本**，新版打包的.pck会被游戏拒绝

2. **.NET 9.0 SDK**
   - 下载: https://dotnet.microsoft.com/

3. **杀戮尖塔2游戏本体** (Steam)

4. **反编译工具** (推荐)
   - **ILSpy** 或 **dnSpyEx** - 查看游戏源码
   - 游戏未做混淆，可直接反编译 `sts2.dll`

5. **资源解包工具**
   - **GDRE Tools** - 解包游戏原始.pck资源
   - **PCK Explorer** - 创建仅包含manifest的空pck

---

## 3. 项目结构

```
MySts2Mod/
├── MySts2Mod.csproj          # C#项目文件
├── MySts2Mod.json            # Mod元数据manifest
├── FirstMod.cs               # 入口类
├── src/
│   ├── cards/                # 卡牌代码
│   ├── relics/               # 遗物代码
│   ├── powers/               # 能力代码
│   ├── characters/           # 角色代码
│   └── utils/                # 工具类
├── assets/                   # 美术资源
│   ├── cards/
│   ├── relics/
│   ├── characters/
│   └── animations/           # Spine动画 (.skel)
└── export/
    └── MySts2Mod.pck         # Godot资源包
```

---

## 4. Mod Manifest 格式

创建 `YourMod.json`:

```json
{
    "id": "YourMod",
    "name": "你的Mod名称",
    "author": "作者名",
    "description": "Mod描述",
    "version": "1.0.0",
    "has_pck": true,          // 是否有资源文件
    "has_dll": true,          // 是否有代码文件
    "dependencies": ["BaseLib"],
    "affects_gameplay": true
}
```

---

## 5. 基础代码示例

### 5.1 Mod入口点

使用 `[ModInitializer]` 属性标记入口方法：

```csharp
using Godot;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Logging;

namespace MyMod;

[ModInitializer("Initialize")]
public static class MyModEntry {
    public static void Initialize() {
        Log.Info("我的Mod已加载!");
        
        // 在这里注册你的内容
        RegisterCards();
        RegisterRelics();
    }
    
    private static void RegisterCards() {
        // 注册卡牌
    }
    
    private static void RegisterRelics() {
        // 注册遗物
    }
}
```

### 5.2 自定义卡牌

```csharp
using Alchyr.Sts2.BaseLib.Cards;
using MegaCrit.Sts2.Cards.Models;

namespace MyMod.Cards;

public class MyCustomCard : CustomCardModel {
    
    public MyCustomCard() {
        // 设置卡牌属性
        Name = "强力打击";
        Cost = 1;
        Type = CardType.Attack;
        Rarity = CardRarity.Common;
        Target = CardTarget.Enemy;
        
        // 设置数值
        BaseDamage = 8;
        UpgradeDamage = 3;
        
        // 设置图片
        PortraitPath = "MyMod/cards/my_card.png";
    }
    
    public override void OnUse(CombatContext ctx) {
        // 使用卡牌时的逻辑
        ctx.Target?.TakeDamage(ctx, Damage, this);
    }
}
```

### 5.3 自定义遗物

```csharp
using Alchyr.Sts2.BaseLib.Relics;
using MegaCrit.Sts2.Relics.Models;

namespace MyMod.Relics;

public class MyCustomRelic : CustomRelicModel {
    
    public MyCustomRelic() {
        Name = "示例遗物";
        Tier = RelicTier.Common;
        
        // 设置图片
        ImagePath = "MyMod/relics/my_relic.png";
    }
    
    public override void OnBattleStart(CombatContext ctx) {
        // 战斗开始时触发
        Flash();  // 遗物闪烁效果
    }
    
    public override void OnVictory(RunContext ctx) {
        // 战斗胜利时触发
    }
}
```

---

## 6. 项目配置文件

### 6.1 .csproj 文件

```xml
<Project Sdk="Godot.NET.Sdk/4.5.1">
  
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <RootNamespace>MyMod</RootNamespace>
  </PropertyGroup>
  
  <ItemGroup>
    <!-- 引用游戏本体 -->
    <Reference Include="sts2">
      <HintPath>$(STS2_PATH)\SlayTheSpire2_Data\Managed\sts2.dll</HintPath>
    </Reference>
    
    <!-- 引用Godot -->
    <Reference Include="Godot">
      <HintPath>$(STS2_PATH)\SlayTheSpire2_Data\Managed\Godot.dll</HintPath>
    </Reference>
    
    <!-- BaseLib (推荐) -->
    <PackageReference Include="Alchyr.Sts2.BaseLib" Version="*" />
  </ItemGroup>
  
</Project>
```

---

## 7. 安装和测试Mod

### 7.1 安装位置

```
Steam/steamapps/common/SlayTheSpire2/mods/
├── YourMod/
│   ├── YourMod.json      # manifest文件
│   ├── YourMod.dll       # 编译后的C#代码
│   └── YourMod.pck       # Godot资源包
```

### 7.2 Steam启动参数

- `--nomods` - 禁用所有Mod（排查崩溃用）
- `--fastmp` - 快速多人模式测试
- `--seed <seed>` - 指定随机种子
- `--log-file <path>` - 自定义日志路径

---

## 8. BaseLib 框架

**BaseLib** 是杀戮尖塔2的Mod开发基础库，由Alchyr开发。

### 主要功能

| 功能 | 说明 |
|------|------|
| `CustomCardModel` | 卡牌基础类 |
| `CustomRelicModel` | 遗物基础类 |
| `CustomPowerModel` | 能力基础类 |
| `CustomCharacterModel` | 角色基础类 |
| `SimpleModConfig` | 配置系统（自动生成游戏内设置UI） |
| `CommonActions` | 常用动作辅助方法 |

---

## 9. 推荐学习资源

### 官方/社区资源
- **[STS2FirstMod](https://github.com/jiegec/STS2FirstMod)** - 最小工作示例
- **[ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2)** - Alchyr的项目模板
- **[BaseLib-StS2](https://github.com/Alchyr/BaseLib-StS2)** - 基础功能库
- **[STS2 Modding MCP](https://www.nexusmods.com/slaythespire2/mods/345)** - AI辅助开发工具

### 中文社区
- **QQ群**: 812670568 (蝴蝶是幼虫的开发群)
- **QQ群**: 542370192 (GlitchedReme教程群)
- **GitHub教程**: [SlayTheSpireModTutorials](https://github.com/GlitchedReme/SlayTheSpireModTutorials)

### 反编译参考
- 使用 **ILSpy** 或 **dnSpyEx** 打开 `sts2.dll`
- 位置: `SlayTheSpire2_Data/Managed/sts2.dll`
- 游戏源码未混淆，可直接查看

---

## 10. 从1代迁移注意事项

1. **完全重写代码**: Java → C#
2. **动画格式升级**: Spine 3.4 JSON → Spine 4.2 二进制
3. **缩放比例**: 迁移后约需10倍缩放
4. **动画轴**: 2代有6种内置动画轴（1代只有待机、受击）
5. **资源打包**: 需要使用Godot打包为.pck格式

---

## 下一步

你想制作什么类型的Mod？我可以提供更具体的代码示例：
- 新卡牌
- 新遗物
- 新角色
- 新事件
- 游戏机制修改
