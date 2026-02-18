using System;
using System.Collections.Generic;
using UnityEngine;
using FishingGame.Data;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    [Header("References")]
    public FishDatabaseSO database;

    private HashSet<string> collectedFish = new HashSet<string>();

    public event Action<FishConfigSO> OnFishDiscovered;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();

        if (database != null)
            database.Init();
    }

    // =========================
    // PUBLIC API
    // =========================

    public void RegisterCatch(FishConfigSO fish)
    {
        if (fish == null) return;

        if (string.IsNullOrEmpty(fish.ObjectID))
        {
            Debug.LogWarning($"Fish {fish.name} has no ObjectID!");
            return;
        }

        // Already collected?
        if (collectedFish.Contains(fish.ObjectID))
            return;

        collectedFish.Add(fish.ObjectID);

        Debug.Log($"New Fish Collected: {fish.Name}");

        OnFishDiscovered?.Invoke(fish);

        Save();
    }

    public bool IsCollected(FishConfigSO fish)
    {
        if (fish == null) return false;
        return collectedFish.Contains(fish.ObjectID);
    }

    public int GetCollectedCount()
    {
        return collectedFish.Count;
    }

    public List<FishConfigSO> GetAllCollected()
    {
        List<FishConfigSO> list = new List<FishConfigSO>();

        foreach (var id in collectedFish)
        {
            var fish = database.Get(id);
            if (fish != null)
                list.Add(fish);
        }

        return list;
    }

    public List<FishConfigSO> GetAllFish()
    {
        return database.allFish;
    }

    // =========================
    // SAVE / LOAD
    // =========================

    private void Save()
    {
        CollectionSaveData data = new CollectionSaveData();
        data.collectedFish.AddRange(collectedFish);

        CollectionSaveSystem.Save(data);
    }

    private void Load()
    {
        var data = CollectionSaveSystem.Load();
        collectedFish = new HashSet<string>(data.collectedFish);
    }
}
