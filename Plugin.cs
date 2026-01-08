using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;
using MonsterAlchemy.scripts;

namespace MonsterAlchemy;

public static class ModInfo
{
    public const string Guid = "littlecrow.monsteralchemy";
    public const string Name = "MonsterAlchemy";
    public const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class Plugin : BaseUnityPlugin
{
    internal static Plugin? Instance;

    private void Awake()
    {
        Instance = this;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), ModInfo.Guid);
    }

    public void OnStartCore()
    {
        // import sources
        var dir = Path.GetDirectoryName(Info.Location);
        var sources = Core.Instance.sources;

        var sourceCard = dir + "/sources/SourceCard.xlsx";
        ModUtil.ImportExcel(sourceCard, "Thing", sources.things);
        var sourceGame = dir + "/sources/SourceGame.xlsx";
        ModUtil.ImportExcel(sourceGame, "Element", sources.elements);

        // config/modoptions init
        MAConfig.InitConfig(Config);
        MAConfig.InitModOptions(Instance);
    }

    internal static void LogDebug(object message, [CallerMemberName] string caller = "")
    {
        Instance?.Logger.LogDebug($"[{caller}] {message}");
    }

    internal static void LogInfo(object message)
    {
        Instance?.Logger.LogInfo(message);
    }

    internal static void LogError(object message)
    {
        Instance?.Logger.LogError(message);
    }
}