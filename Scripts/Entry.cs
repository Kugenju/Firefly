using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using Firefly.Powers;

namespace Firefly.Scripts;

/// <summary>
/// Mod 主入口类
/// </summary>
[ModInitializer("Init")]
public static class Entry
{
	/// <summary>
	/// Mod 初始化方法
	/// </summary>
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

			// 注：卡牌、角色、遗物、Power 通过 BaseLib 自动注册
			// 本地化通过 localization/zhs/*.json 文件提供
			Log.Info("Models will be auto-registered by BaseLib.");

			Log.Info("流萤角色 Mod 初始化完成!");
		}
		catch (Exception ex)
		{
			Log.Error($"流萤角色 Mod 初始化失败: {ex.Message}");
			Log.Error(ex.StackTrace ?? "No stack trace available");
		}
	}
}
