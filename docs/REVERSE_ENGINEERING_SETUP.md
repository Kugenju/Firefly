# 杀戮尖塔2 反编译源码查看指南

## 文件位置

### 游戏主程序
- **DLL位置**: `G:\SteamLibrary\steamapps\common\Slay the Spire 2\data_sts2_windows_x86_64\sts2.dll`
- **PCK资源**: `G:\SteamLibrary\steamapps\common\Slay the Spire 2\SlayTheSpire2.pck`

### 反编译工具
- **dnSpy控制台**: `F:\dnSpy\dnSpy\dnSpy.Console.exe`

### Mod开发引用
- **BaseLib**: 通过NuGet包 `Baselib.Sts2` 引用
- **游戏API**: 通过引用 `sts2.dll` 获取

## 快速查看脚本

使用项目根目录的 `view-source.bat` 脚本来快速查看反编译代码：

```batch
# 查看指定类型的源码
view-source.bat <类名或命名空间>

# 示例
view-source.bat CombatCreature      # 查看战斗生物基类
view-source.bat BlockInfo           # 查看格挡相关
view-source.bat PowerModel          # 查看Power基类
```

## 常用类参考

| 类名 | 命名空间 | 说明 |
|------|----------|------|
| CombatCreature | SlayTheSpire2.Combat.Creatures | 战斗生物基类 |
| BlockInfo | SlayTheSpire2.Combat | 格挡信息管理 |
| PowerModel | SlayTheSpire2.Combat.Powers | Power基类 |
| CardModel | SlayTheSpire2.Cards | 卡牌基类 |
| DamageCmd | SlayTheSpire2.Combat.Commands | 伤害命令 |
| PowerCmd | SlayTheSpire2.Combat.Commands | Power命令 |

## 手动反编译

如果需要导出完整源码：

```batch
# 使用dnSpy.Console导出
F:\dnSpy\dnSpy\dnSpy.Console.exe -o "输出目录" "G:\SteamLibrary\steamapps\common\Slay the Spire 2\data_sts2_windows_x86_64\sts2.dll"
```

## 注意事项

1. 反编译代码仅供学习参考，不要用于商业用途
2. 游戏更新后可能需要重新反编译
3. Mod开发应使用BaseLib提供的公共API
4. 日志位置：C:\Users\27294\AppData\Roaming\SlayTheSpire2\logs\godot.log
