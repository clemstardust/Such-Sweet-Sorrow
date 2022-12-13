using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class GameSaveHandler : MonoBehaviour
{
    public void SaveGame(UnlockedItemManager unlockedItemManager)
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("Save00");
        writer.Write("TotalSouls", unlockedItemManager.totalSouls);
        writer.Write("UnlockedSword", unlockedItemManager.unlockedSword);
        writer.Write("UnlockedMace", unlockedItemManager.unlockedMace);
        writer.Write("UnlockedBigSword", unlockedItemManager.unlockedBigSword);
        writer.Write("UnlockedBigMace", unlockedItemManager.unlockedBigMace);
        writer.Write("unlockedExtraLivesRing", unlockedItemManager.unlockedExtraLivesRing);
        writer.Write("unlockedExtraSoulsRing", unlockedItemManager.unlockedExtraSoulsRing);
        writer.Write("unlockedSoulshield", unlockedItemManager.unlockedSoulshield);
        writer.Write("unlockedSoulfire", unlockedItemManager.unlockedSoulfire);
        writer.Write("unlockedSoulGenerator", unlockedItemManager.unlockedSoulGenerator);
        writer.Write("unlockedDoubleUpgrades", unlockedItemManager.unlockedDoubleUpgrades);
        writer.Commit();
    }
    public void LoadGame()
    {
        QuickSaveReader reader = QuickSaveReader.Create("Save00");
        UnlockedItemManager unlocks = FindObjectOfType<UnlockedItemManager>();
        unlocks.totalSouls = reader.Read<int>("TotalSouls");
        unlocks.unlockedSword = reader.Read<bool>("UnlockedSword");
        unlocks.unlockedMace = reader.Read<bool>("UnlockedMace");
        unlocks.unlockedBigSword = reader.Read<bool>("UnlockedBigSword");
        unlocks.unlockedBigMace = reader.Read<bool>("UnlockedBigMace");
        unlocks.unlockedExtraLivesRing = reader.Read<bool>("unlockedExtraLivesRing");
        unlocks.unlockedExtraSoulsRing = reader.Read<bool>("unlockedExtraSoulsRing");
        unlocks.unlockedSoulshield = reader.Read<bool>("unlockedSoulshield");
        unlocks.unlockedSoulfire = reader.Read<bool>("unlockedSoulfire");
        unlocks.unlockedSoulGenerator = reader.Read<bool>("unlockedSoulGenerator");
        unlocks.unlockedDoubleUpgrades = reader.Read<bool>("unlockedDoubleUpgrades");
    }    
}
