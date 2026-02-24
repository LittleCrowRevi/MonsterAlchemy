using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAlchemy.scripts;

[HarmonyPatch]
internal class CommonHooks
{
    [HarmonyPrefix, HarmonyPatch(typeof(Card), "SpawnLoot")]
    public static void SpawnLoot(Card __instance, Card origin)
    {
        if (!Game.Instance.activeZone.IsNefia)
        {
            return;
        }
        var nearestPoint = __instance.pos;
        var qualityLevel = __instance.LV switch
        {
            < 50 => 1, // common
            < 100 => 2, // uncommon
            < 500 => 3, // rare
            < 1000 => 4,
            _ => 0
        };

        // sets encLv(enchantment level) so that different qualities don't get merged on pickup
        // no clue why it doesn't differentiate from them having different data but eh
        var pneuma = ThingGen.Create("pneuma");
        pneuma.SetEncLv(qualityLevel - 1);
        pneuma.SetNum(1);

        var quality = Element.Create(ModIds.pneumaTrait, qualityLevel);
        pneuma.elements.dict.Add(ModIds.pneumaTrait, quality);
        
        // spawn at location of the defeated monster
        EClass._zone.AddCard(pneuma, nearestPoint);

        Plugin.LogInfo("Spawned: " + pneuma.Name + ", at: " + pneuma.pos + ", with: " + string.Join(",", pneuma.elements.dict.Select(kv => $"{kv.Key}={kv.Value.vBase}")));
    }

    /*[HarmonyPostfix, HarmonyPatch(typeof(Thing), "WriteNote")]
    public static void WriteNote(Thing __instance, UINote n, IInspect.NoteMode mode, Recipe recipe)
    {
        if (__instance.id != "pneuma") return;
        __instance.elements.AddNote(n, (Element e) => true, null, ElementContainer.NoteMode.BonusTrait, addRaceFeat: false, delegate (Element e, string s)
        {
            var textArray = e.source.GetTextArray("textAlt");
            string altText = "altEnc".lang(textArray[0].IsEmpty(e.Name), textArray[e.vBase - 1], "<size=12>Pneuma Quality</size>".TagColor(FontColor.Passive));
            return altText;
        });
    }*/
}
