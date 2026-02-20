using UnityEngine;

public enum UpgradeTypes 
{ 
    Stamina,
    ReduceReelDecay,
    IncreaseReelSpeed,
    LuckScore,
    IncreaseCatchBarSize,
    StaminaRecovery
}

namespace FishingGame.Data
{
    [CreateAssetMenu(menuName = "Fishing Game/Upgrade Config", fileName = "[Upgrade Config]")]
    public class UpgradeConfigSO : ScriptableObject
    {
        // VARIABLES
        [Header("Settings")]
        public string Name;
        public string Description;
        public UpgradeTypes Type;
        public float ValuePerLevel;

        [Space(5)]
        public Sprite Sprite;

        [Space(5)]
        public float PricePerLevel = 10f;

        [Header("System")]
        public string ObjectID;
    }
}
