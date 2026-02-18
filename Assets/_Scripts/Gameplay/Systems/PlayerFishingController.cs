using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using FishingGame.Data;

namespace FishingGame.Gameplay.Systems
{
    public class PlayerFishingController : MonoBehaviour
    {
        // VARIABLES
        [Header("References")]
        [SerializeField] private RectTransform fishingBarContainer;
        [SerializeField] private RectTransform fishIndicator;
        [SerializeField] private RectTransform catchBar;
        [SerializeField] private Image progressBar;

        [Header("Catch Bar Settings")]
        [SerializeField] private List<float> catchBarSizes = new();
        [SerializeField] private List<float> catchBarSpeeds = new();
        [SerializeField] private float gravity = 800f;
        
        [Header("Fish Settings")]
        [SerializeField] private List<float> fishSpeeds = new();

        [Header("Progress Settings")]
        [SerializeField] private float progressFillRate = 0.5f;
        [SerializeField] private float progressDepleteRate = 0.3f;

        private float currentCatchBarSize;
        private float currentCatchBarSpeed;
        private float currentFishSpeed;

        private float currentFishTarget;
        private float currentProgress = 0f;
        private float currentCatchBarVerticalVelocity = 0f;

        private bool catchBarIsMoving = false;

        private PlayerWallet playerWallet;

        private FishConfigSO currentFish;

        // EXECUTION FUNCTIONS
        private void Start()
        {
            playerWallet = PlayerManager.Instance.Wallet;
        }

        private void Update()
        {
            UpdateFishingSettings();
            HandleCatchBarMovement();
            HandleFishMovement();
            UpdateProgress();
            CheckWinLose();
        }

        // METHODS
        public void BeginFishing(FishConfigSO fishConfig)
        {
            currentFish = fishConfig;

            ResetCatchBarPosition();

            currentProgress = 0.5f;
            progressBar.fillAmount = currentProgress;

            gameObject.SetActive(true);

            Debug.Log($"PlayerFishingController::BeginFishing() --- Begin fishing {fishConfig.Name} (Diff: {(int)fishConfig.Rarity})");
        }

        public void SetCatchBarMovementActive(bool isActive)
        {
            catchBarIsMoving = isActive;
        }

        private void UpdateFishingSettings()
        {
            currentCatchBarSize = catchBarSizes[(int)currentFish.Rarity];
            currentCatchBarSpeed = catchBarSpeeds[(int)currentFish.Rarity];
            currentFishSpeed = fishSpeeds[(int)currentFish.Rarity];

            Vector3 catchBarDelta = catchBar.sizeDelta;
            catchBarDelta.y = currentCatchBarSize;
            catchBar.sizeDelta = catchBarDelta;
        }

        private void HandleCatchBarMovement()
        {
            if (catchBarIsMoving)
            {
                currentCatchBarVerticalVelocity = currentCatchBarSpeed;
            }
            else
            {
                currentCatchBarVerticalVelocity -= gravity * Time.deltaTime;
            }

            Vector2 pos = catchBar.anchoredPosition;
            pos.y += currentCatchBarVerticalVelocity * Time.deltaTime;

            float barHeight = catchBar.rect.height;
            float containerHeight = fishingBarContainer.rect.height;

            float top = containerHeight / 2 - barHeight / 2;
            float bottom = -containerHeight / 2 + barHeight / 2;

            if (pos.y > top - 2f)
            {
                currentCatchBarVerticalVelocity = 0f;
            }

            pos.y = Mathf.Clamp(pos.y, bottom, top);
            catchBar.anchoredPosition = pos;
        }

        private void HandleFishMovement()
        {
            if (Mathf.Abs(fishIndicator.anchoredPosition.y - currentFishTarget) < 10f)
            {
                float containerHeight = fishingBarContainer.rect.height;
                currentFishTarget = Random.Range(-containerHeight / 2 + 20, containerHeight / 2 - 20);
            }

            float direction = Mathf.Sign(currentFishTarget - fishIndicator.anchoredPosition.y);
            Vector2 fishPos = fishIndicator.anchoredPosition;
            fishPos.y += direction * currentFishSpeed * Time.deltaTime;
            fishIndicator.anchoredPosition = fishPos;
        }

        private void UpdateProgress()
        {
            float fishY = fishIndicator.anchoredPosition.y;
            float catchBarY = catchBar.anchoredPosition.y;
            float catchBarHeight = catchBar.rect.height;

            bool fishIsInsideCatchBar = fishY >= catchBarY - catchBarHeight / 2 && fishY <= catchBarY + catchBarHeight / 2;
            
            currentProgress += (fishIsInsideCatchBar ? progressFillRate : -progressDepleteRate) * Time.deltaTime;
            currentProgress = Mathf.Clamp01(currentProgress);
            progressBar.fillAmount = currentProgress;
        }

        private void CheckWinLose()
        {
            if (currentProgress >= 1f)
            {
                gameObject.SetActive(false);
                playerWallet.Add(CurrencyTypes.Gold, currentFish.SellValue);

                CollectionManager.Instance.RegisterCatch(currentFish);
            }
            else if (currentProgress <= 0f)
            {
                gameObject.SetActive(false);
            }
        }

        private void ResetCatchBarPosition()
        {
            Vector2 pos = catchBar.anchoredPosition;

            float barHeight = catchBar.rect.height;
            float containerHeight = fishingBarContainer.rect.height;

            float bottom = -containerHeight / 2 + barHeight / 2;
            pos.y = bottom;
            catchBar.anchoredPosition = pos;
        }
    }
}