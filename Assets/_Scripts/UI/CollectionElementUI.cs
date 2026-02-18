using UnityEngine;
using UnityEngine.UI;

using FishingGame.Data;

namespace FishingGame.UI
{
    public class CollectionElementUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private Image iconImage;

        private FishConfigSO associatedFish;

        // METHODS
        public void Initialize(FishConfigSO fish)
        {
            associatedFish = fish;
            iconImage.sprite = fish.Sprite;
            UpdateState();
        }

        public void UpdateState()
        {
            iconImage.color = CollectionManager.Instance.IsCollected(associatedFish) ? Color.white : Color.black;
        }
    }
}
