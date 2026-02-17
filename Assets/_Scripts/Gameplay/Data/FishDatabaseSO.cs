using System.Collections.Generic;
using UnityEngine;
using FishingGame.Data;

[CreateAssetMenu(fileName = "FishDatabaseSO", menuName = "Fishing Game/Fish Database")]
public class FishDatabaseSO : ScriptableObject
{
    [Header("All Fish In Game")]
    public List<FishConfigSO> allFish;

    private Dictionary<string, FishConfigSO> lookup;

    public void Init()
    {
        lookup = new Dictionary<string, FishConfigSO>();

        foreach (var fish in allFish)
        {
            if (fish == null) continue;

            if (string.IsNullOrEmpty(fish.ObjectID))
            {
                Debug.LogWarning($"Fish {fish.name} has no ObjectID!");
                continue;
            }

            if (!lookup.ContainsKey(fish.ObjectID))
                lookup.Add(fish.ObjectID, fish);
        }
    }

    // Get fish by ID
    public FishConfigSO Get(string id)
    {
        if (lookup == null)
            Init();

        lookup.TryGetValue(id, out var fish);
        return fish;
    }
}
