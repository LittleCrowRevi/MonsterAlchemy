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
    internal static ConfigEntry<int> configExpGain;

    public static void InitConfig(ConfigFile config)
    {
        configExpGain = config.Bind(
            "General",
            "expGain",
            300,
            "Change the experience gained from drinking a pneuma potion."
        );
    }

    public static void InitModOptions(Plugin plugin)
    {
        // Mod Options is loaded, you can do
        // registeration now.
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
            slider.Step = 10f;
            slider.Max = 1000f;
            slider.Value = configExpGain.Value;
            slider.OnValueChanged += v =>
            {
                slider.Title = slider.Value.ToString();
                configExpGain.Value = (int)v;
            };
        };
    }
}
