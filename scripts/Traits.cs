using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterAlchemy.scripts;

public class TraitBodyPneuma : TraitResourceMain
{
    public override void WriteNote(UINote n, bool identified)
    {
        if (n is null)
        {
            Plugin.LogInfo("N null?");
            return;
        }
        base.WriteNote(n, identified);
        n.AddText("altEnc".lang("Body Pneuma", "Body Pneuma", "Enhances the body."));
    }
}

public class TraitBodyPotion : TraitPotion
{
    public override void OnDrink(Chara c)
    {
        if (c.elements.GetElement(70) is not AttbMain str) return;

        var prev_exp = str.vExp;
        c.elements.ModExp(70, MAConfig.configExpGain.Value);
        Plugin.LogInfo($"Used Body Potion to increase exp from {prev_exp} to {str.vExp}");

    }
}
