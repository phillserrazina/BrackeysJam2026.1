using UnityEngine;

namespace FishingGame.Data
{
    [CreateAssetMenu(menuName = "Fishing Game/Upgrade Config", fileName = "[Upgrade Config]")]
    public class UpgradeConfigSO : ScriptableObject
    {
        // VARIABLES
        [Header("Settings")]
        public string Name;
        public string Description;

        [Space(5)]
        public Sprite Sprite;

        [Space(5)]
        public float Price = 10f;

        [Header("System")]
        public string ObjectID;
    }
}
