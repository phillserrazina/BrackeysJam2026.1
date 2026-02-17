using UnityEngine;

namespace FishingGame.Data
{
    [CreateAssetMenu(menuName = "Fishing Game/Planet Config", fileName = "[Planet Config]")]
    public class PlanetConfigSO : ScriptableObject
    {
        // VARIABLES
        [Header("Settings")]
        public string Name;
        public string Description;

        [Space(5)]
        public Sprite Sprite;

        [Space(5)]
        public FishConfigSO[] Fishes;

        [Header("System")]
        public string ObjectID;
    }
}
