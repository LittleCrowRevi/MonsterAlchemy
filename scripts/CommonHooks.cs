using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAlchemy.scripts;

internal class CommonHooks
{
    [HarmonyPrefix, HarmonyPatch(typeof(Card), "SpawnLoot")]
    public static void SpawnLoot(Card __instance, Card origin)
    {
        Point nearestPoint = __instance.pos;
        var testThing = ThingGen.Create("body_pneuma");
        EClass._zone.AddCard(testThing, nearestPoint);

        Plugin.LogInfo("Spawned: " + testThing.Name + ", at: " + testThing.pos + ", with: " + string.Join(",", testThing.elements.dict.Select(kv => $"{kv.Key}={kv.Value}")));
    }
}
