using System.Collections.Generic;
using System.IO;
using UnityEngine;
using FishingGame.Data;

namespace FishingGame.Gameplay.Systems
{
    [System.Serializable]
    public class UpgradeSaveEntry
    {
        public string id;
        public int level;
    }

    [System.Serializable]
    public class PlayerSaveData
    {
        public float gold = 0f;
        public List<UpgradeSaveEntry> upgrades = new();
    }

    public static class PlayerSaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "player_save.json");

        public static void Save(PlayerSaveData data)
        {
            try
            {
                var json = JsonUtility.ToJson(data);
                File.WriteAllText(SavePath, json);
                Debug.Log($"PlayerSaveSystem::Save() saved to {SavePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"PlayerSaveSystem::Save() failed: {e}");
            }
        }

        public static PlayerSaveData Load()
        {
            try
            {
                if (!File.Exists(SavePath))
                    return new PlayerSaveData();

                var json = File.ReadAllText(SavePath);
                var data = JsonUtility.FromJson<PlayerSaveData>(json);
                return data ?? new PlayerSaveData();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"PlayerSaveSystem::Load() failed: {e}");
                return new PlayerSaveData();
            }
        }

        public static void Delete()
        {
            try
            {
                if (File.Exists(SavePath))
                    File.Delete(SavePath);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"PlayerSaveSystem::Delete() failed: {e}");
            }
        }
    }
}
