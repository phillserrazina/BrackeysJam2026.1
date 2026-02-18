using UnityEngine;

public static class CollectionSaveSystem
{
    private const string KEY = "FISH_COLLECTION";

    public static void Save(CollectionSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public static CollectionSaveData Load()
    {
        if (!PlayerPrefs.HasKey(KEY))
            return new CollectionSaveData();

        string json = PlayerPrefs.GetString(KEY);
        return JsonUtility.FromJson<CollectionSaveData>(json);
    }
}
