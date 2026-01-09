using BepInEx;
using BepInEx.Configuration;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace MonsterAlchemy.scripts;

internal class MAConfig
{
    internal static ConfigEntry<float> configExpGain;

    public static void InitConfig(ConfigFile config)
    {
        configExpGain = config.Bind(
            "General",
            "expGain",
            1f,
            "Change the experience gained from drinking a pneuma potion."
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
            slider.Title = configExpGain.Value.ToString();
            slider.Step = 0.5f;
            slider.Max = 10f;
            slider.Value = configExpGain.Value;
            slider.OnValueChanged += v =>
            {
                slider.Value = MathF.Round(v, 1);
                slider.Title = slider.Value.ToString();
                configExpGain.Value = (int)v;
            };
        };
    }
}
