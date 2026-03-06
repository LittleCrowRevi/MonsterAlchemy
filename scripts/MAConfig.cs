using BepInEx;
using BepInEx.Configuration;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace MonsterAlchemy.scripts;

internal class MaConfig
{
    internal static ConfigEntry<float> ConfigExpGain;
    internal static ConfigEntry<float> ConfigQualityMod;

    public static void InitConfig(ConfigFile config)
    {
        ConfigExpGain = config.Bind(
            "General",
            "expGain",
            300f,
            "Change the experience gained from drinking a pneuma potion."
        );
        ConfigQualityMod = config.Bind(
            "General",
            "qualityMod",
            10f,
            "Changes the modifier pneuma quality applies to the gained exp from potions."
        );
    }

    public static void InitModOptions(Plugin plugin)
    {
        // check if mod options is loaded
        var modLoaded = false;
        foreach (var obj in ModManager.ListPluginObject)
        {
            var mod = obj as BaseUnityPlugin;
            if (mod.Info.Metadata.GUID == "evilmask.elinplugins.modoptions")
            {
                modLoaded = true;
                break;
            }
        }

        if (!modLoaded) return;

        var controller = ModOptionController.Register(ModInfo.Guid);
        using (StreamReader sr = new(Path.GetDirectoryName(plugin.Info.Location) + "/config/" + "ConfigExample.en.xml"))
            controller.SetPreBuildWithXml(sr.ReadToEnd());

        controller.SetTranslation(ModInfo.Guid,
            "Monster Alchemy(EN)", "何かのMod(JP)", "我的模组(CN)");
        controller.SetTranslation("mod.tooltip",
            "Monster Alchemy!", "俺が作ったのだ！", "这是我的模组！");
        controller.SetTranslation("exampleText",
            "This text only has an English version!");

        controller.OnBuildUI += builder =>
        {
            var slider = builder.GetPreBuild<OptSlider>("expSlider");
            slider.Title = ConfigExpGain.Value.ToString(CultureInfo.CurrentCulture);
            slider.Step = 1f;
            slider.Max = 1000f;
            slider.Value = ConfigExpGain.Value;
            slider.OnValueChanged += v =>
            {
                slider.Value = MathF.Floor(v);
                slider.Title = slider.Value.ToString(CultureInfo.InvariantCulture);
                ConfigExpGain.Value = v;
            };
        };
        controller.OnBuildUI += builder =>
        {
            var slider = builder.GetPreBuild<OptSlider>("qualityMod");
            slider.Title = ConfigQualityMod.Value.ToString(CultureInfo.InvariantCulture);
            slider.Step = 1f;
            slider.Max = 100f;
            slider.Value = ConfigQualityMod.Value;
            slider.OnValueChanged += v =>
            {
                slider.Value = MathF.Floor(v);
                slider.Title = slider.Value.ToString(CultureInfo.InvariantCulture);
                ConfigQualityMod.Value = (int)v;
            };
        };
    }
}
