using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using FishingGame.Data;

namespace FishingGame.Systems
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
        [SerializeField] private float catchBarSize = 150f;
        [SerializeField] private float catchBarSpeed = 500f;
        [SerializeField] private float gravity = 800f;
        
        [Header("Fish Settings")]
        [SerializeField] private float fishSpeed = 200f;

        [Header("Progress Settings")]
        [SerializeField] private float progressFillRate = 0.5f;
        [SerializeField] private float progressDepleteRate = 0.3f;

        private float currentFishTarget;
        private float currentProgress = 0f;
        private float currentCatchBarVerticalVelocity = 0f;

        private bool catchBarIsMoving = false;

        private PlayerWallet playerWallet;

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
            ResetCatchBarPosition();

            currentProgress = 0.5f;
            progressBar.fillAmount = currentProgress;

            gameObject.SetActive(true);

            Debug.Log($"PlayerFishingController::BeginFishing() --- Begin fishing {fishConfig.Name}");
        }

        public void SetCatchBarMovementActive(bool isActive)
        {
            catchBarIsMoving = isActive;
        }

        private void UpdateFishingSettings()
        {
            Vector3 catchBarDelta = catchBar.sizeDelta;
            catchBarDelta.y = catchBarSize;
            catchBar.sizeDelta = catchBarDelta;
        }

        private void HandleCatchBarMovement()
        {
            if (catchBarIsMoving)
            {
                currentCatchBarVerticalVelocity = catchBarSpeed;
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
            fishPos.y += direction * fishSpeed * Time.deltaTime;
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
                playerWallet.Add(CurrencyTypes.Gold, 10f);
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