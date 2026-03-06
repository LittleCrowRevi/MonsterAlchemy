using Cwl.Helper.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MonsterAlchemy.scripts;

public class TraitPneuma : TraitResourceMain
{

    public override void WriteNote(UINote n,
        bool identified)
    {
        base.WriteNote(n,
            identified);

        // write the note for the pneuma trait
        // get the source lang text
        var s = EClass.sources.elements.GetRow(ModIds.pneumaTrait.ToString());
        var textArray = s.textAlt;

        var pneumaTrait = owner.elements.GetOrCreateElement(ModIds.pneumaTrait);
        var altText = "altEnc".lang(
            textArray[0].IsEmpty(pneumaTrait.Name),
            textArray[pneumaTrait.vBase > 0 ? pneumaTrait.vBase - 1 : 1].TagColor(FontColor.FoodQuality),
            "<size=12>Pneuma Quality</size>".TagColor(FontColor.Passive)
            );

        // add text and icon to the note
        var uiItem = n.AddText("NoteText_enc", altText);
        uiItem.image1.SetActive(true);
        uiItem.image1.sprite = EClass.core.refs.icons.enc.rune;
    }
}

public class TraitPneumaPotion : TraitPotion
{
    // Id of the attribute to increase
    public override void OnCrafted(Recipe recipe,
        List<Thing> ings)
    {
        base.OnCrafted(recipe,
            ings);

        // Each recipe contains the id of the attribute to increase in the "unknown" column
        var attb = EClass.sources.things.GetRow(recipe.GetIdThing()).unknown.ToInt();
        owner.SetFlagValue("attbId", attb);

        // copy quality over
        var pneuma = ings.Find(x => x.trait is TraitPneuma);
        owner.SetEncLv(pneuma?.encLV ?? 1);

        if (pneuma == null) return;
        var t = pneuma.elements.dict.First();
        owner.elements.dict.Add(t.Key, t.Value);
    }

    public override void OnDrink(Chara c)
    {
        var attb = c.elements.GetElement(owner.GetFlagValue("attbId"));
        if (attb is not AttbMain)
        {
            Plugin.LogError("[PneumaPotionDrink] Id of element does not match an attribute: " + attb);
            return;
        }

        var prevExp = attb.vExp;
        c.elements.ModExp(
            attb.id, 
            MaConfig.ConfigExpGain.Value * (owner.elements.GetElement(ModIds.pneumaTrait).vBase * MaConfig.ConfigQualityMod.Value)
            );
        Plugin.LogInfo($"Used Pneuma Potion to increase exp for {attb.Name} from {prevExp} to {attb.vExp}");

    }

    public override void WriteNote(UINote n, bool identified)
    {
        base.WriteNote(n, identified);

        // write the note for the pneuma trait
        // get the source lang text
        var s = EClass.sources.elements.GetRow(ModIds.pneumaTrait.ToString());
        var textArray = s.textAlt;

        var pneumaTrait = owner.elements.GetOrCreateElement(ModIds.pneumaTrait);
        var altText = "altEnc".lang(
            textArray[0].IsEmpty(pneumaTrait.Name),
            textArray[pneumaTrait.vBase > 0 ? pneumaTrait.vBase - 1 : 1].TagColor(FontColor.FoodQuality),
            "<size=12>Pneuma Quality</size>".TagColor(FontColor.Passive)
            );

        // add text and icon to the note    
        var uiItem = n.AddText("NoteText_enc", altText);
        uiItem.image1.SetActive(true);
        uiItem.image1.sprite = EClass.core.refs.icons.enc.rune;
    }
}

public static class ModIds
{
    public const string strPotion = "strPneumaPotion";
    public const string endPotion = "endPneumaPotion";

    public const int pneumaTrait = 117001;

    public const int str = 70,
        end = 71,
        dex = 72,
        per = 73,
        ler = 74,
        wil = 75,
        mag = 76,
        cha = 77;
}