using UnityEngine;

public enum Rarities { Common, Uncommon, Rare, VeryRare, Legendary }

namespace FishingGame.Data
{
    [CreateAssetMenu(menuName = "Fishing Game/Fish Config", fileName = "[Fish Config]")]
    public class FishConfigSO : ScriptableObject
    {
        // VARIABLES
        [Header("Settings")]
        public string Name;
        public string Description;
        public Rarities Rarity;

        [Space(5)]
        public Sprite Sprite;

        [Space(5)]
        public float SellValue = 10f;

        [Header("System")]
        public string ObjectID;
    }
}
