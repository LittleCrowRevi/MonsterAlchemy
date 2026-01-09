using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterAlchemy.scripts;

public class TraitPneuma : TraitResourceMain
{
    
    public override void WriteNote(UINote n, bool identified)
    {
        base.WriteNote(n, identified);

        // get the source lang text
        var s = EClass.sources.elements.GetRow("117001");
        var textArray = s.textAlt;

        var pneumaTrait = owner.elements.GetElement(117001);
        string altText = "altEnc".lang(textArray[0].IsEmpty(pneumaTrait.Name), textArray[pneumaTrait.vBase - 1].TagColor(FontColor.FoodQuality), "<size=12>Pneuma Quality</size>".TagColor(FontColor.Passive));
        
        // add text and icon to the note
        var uiItem = n.AddText("NoteText_enc", altText);
        uiItem.image1.SetActive(true);
        uiItem.image1.sprite = EClass.core.refs.icons.enc.rune;
    } 
}

public class TraitBodyPotion : TraitPotion
{
    public override void OnDrink(Chara c)
    {
        if (c.elements.GetElement(70) is not AttbMain str) return;

        var prev_exp = str.vExp;
        c.elements.ModExp(70, 300 * MAConfig.configExpGain.Value);
        Plugin.LogInfo($"Used Body Potion to increase exp from {prev_exp} to {str.vExp}");

    }
}
