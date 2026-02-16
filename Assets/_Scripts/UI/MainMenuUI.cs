using UnityEngine;

using FishingGame.Systems;

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
