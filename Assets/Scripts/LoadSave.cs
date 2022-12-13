using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class LoadSave : MonoBehaviour
{
    public void SaveGame(PlayerStats playerStats, PlayerActionHandler playerActionHandler)
    {
        RoomSpawns roomSpawns = FindObjectOfType<RoomSpawns>();
        QuickSaveWriter writer = QuickSaveWriter.Create("Save00");
        writer.Write("Seed", roomSpawns.seed);
        writer.Write("PlayerPos", playerActionHandler.gameObject.transform.position);
        writer.Write("PlayerHealth", playerStats.currentHealth);
        writer.Write("PlayerWeaponData", GameManager.selectedStartingWeaponData);
        writer.Write("PlayerWeapon", GameManager.selectedStartingWeapon);
        writer.Write("PlayerRelic", GameManager.selectedStartingRelicData);
        writer.Write("PlayerDeathState", PlayerStats.isDead);
        writer.Commit();
    }
    public void LoadGame()
    {
        RoomSpawns roomSpawns = FindObjectOfType<RoomSpawns>();
        QuickSaveReader reader = QuickSaveReader.Create("Save00");
        roomSpawns.seed = reader.Read<int>("Seed");
        //player.transform.position = reader.Read<Vector3>("PlayerPos");
    }
    public Vector3 LoadPlayerPos()
    {
        QuickSaveReader reader = QuickSaveReader.Create("Save00");
        return reader.Read<Vector3>("PlayerPos");
    }
}
