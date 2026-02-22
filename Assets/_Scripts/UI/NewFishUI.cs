using DG.Tweening;
using TMPro;
using UnityEngine;

namespace FishingGame.UI
{
    public class NewFishUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private CanvasGroupFader fader;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            CollectionManager.Instance.OnFishDiscovered += Collection_OnFishDiscovered;
        }

        private void OnDestroy()
        {
            CollectionManager.Instance.OnFishDiscovered -= Collection_OnFishDiscovered;
        }

        private void Collection_OnFishDiscovered(Data.FishConfigSO fish)
        {
            text.text = $"{fish.Name} Added to Collection!";
            fader.FadeIn(0f);

            DOVirtual.DelayedCall(3f, () =>
            {
                fader.FadeOut(0.5f);
            });
        }
    }
}
