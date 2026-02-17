using UnityEngine;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        // METHODS
        public void PlayButton()
        {
            SceneLoader.Instance.LoadGameplayScene("Earth");
        }
    }
}
