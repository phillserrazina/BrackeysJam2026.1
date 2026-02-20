using UnityEngine;

public enum UpgradeTypes { LuckScore, IncreaseCatchBarSize, ReduceFishSpeed }

namespace FishingGame.Data
{
    [CreateAssetMenu(menuName = "Fishing Game/Upgrade Config", fileName = "[Upgrade Config]")]
    public class UpgradeConfigSO : ScriptableObject
    {
        // VARIABLES
        [Header("Settings")]
        public string Name;
        public string Description;
        public UpgradeAttribute[] Attributes;

        [Space(5)]
        public Sprite Sprite;

        [Space(5)]
        public float Price = 10f;

        [Header("System")]
        public string ObjectID;

        // HELPER CLASSES
        [System.Serializable]
        public class UpgradeAttribute
        {
            public UpgradeTypes Type;
            public float Value;
        }
    }
}
