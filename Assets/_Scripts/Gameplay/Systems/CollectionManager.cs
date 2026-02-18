using System;
using System.Collections.Generic;

using UnityEngine;

using FishingGame.Data;
using FishingGame.Gameplay.Systems;

public class CollectionManager : MonoBehaviour
{
    [Header("References")]
    private HashSet<string> collectedFish = new();

    public event Action<FishConfigSO> OnFishDiscovered;
    public static CollectionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
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
        List<FishConfigSO> list = new();

        foreach (var id in collectedFish)
        {
            var fish = DataManager.Instance.GetFishByID(id);
            if (fish != null)
                list.Add(fish);
        }

        return list;
    }

    // =========================
    // SAVE / LOAD
    // =========================
    private void Save()
    {
        CollectionSaveData data = new();
        data.collectedFish.AddRange(collectedFish);

        CollectionSaveSystem.Save(data);

        Debug.Log($"CollectionManager::Save() --- Data saved successfuly. Fish collected: {collectedFish.Count}");
    }

    private void Load()
    {
        var data = CollectionSaveSystem.Load();
        collectedFish = new(data.collectedFish);

        Debug.Log($"CollectionManager::Load() --- Data loaded successfuly. Fish collected: {collectedFish.Count}");
    }

    public void Delete()
    {
        collectedFish = new();
        CollectionSaveSystem.Delete();

        Debug.Log($"CollectionManager::Delete() --- Data deleted successfuly. Fish collected: {collectedFish.Count}");
    }
}
