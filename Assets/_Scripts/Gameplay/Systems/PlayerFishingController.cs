using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using FishingGame.Data;
using DG.Tweening;
using TMPro;

namespace FishingGame.Gameplay.Systems
{
    public enum FishingStates { Idle, Casting, Fishing, CatchInputWindow, Catching, PostCatch }

    public class PlayerFishingController : MonoBehaviour
    {
        // VARIABLES
        [Header("References")]
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private GameObject castingUI;
        [SerializeField] private GameObject fishingUI;

        [Header("Casting")]
        [SerializeField] private Image castingFillImage;
        [SerializeField] private float castingFillSpeed = 1f;

        [Header("Fishing")]
        [SerializeField] private GameObject waterRippleObject;
        [SerializeField] private GameObject biteWarningObject;

        [SerializeField] private float minFishingWaitTime = 2f;
        [SerializeField] private float maxFishingWaitTime = 10f;

        [Header("Catch Window")]
        [SerializeField] private float catchWindowTime = 1f;

        [Header("Catching")]
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

        private PlayerManager player;
        private FishConfigSO currentFish;

        private float castingTime = 0f;
        private float currentCastingFillSpeed = 0f;
        private float currentCastingValue = 0f;

        private float targetFishingWaitTime = 0f;
        private float currentFishingWaitTime = 0f;

        private float currentCatchWindowTime = 0f;

        private FishingStates currentState = FishingStates.Idle;

        // EXECUTION FUNCTIONS
        private void Start()
        {
            player = PlayerManager.Instance;
        }

        private void Update()
        {
            switch (currentState)
            {
                case FishingStates.Idle:
                    break;
                case FishingStates.Casting:
                    HandleCasting();
                    break;
                case FishingStates.Fishing:
                    HandleFishingWait();
                    break;
                case FishingStates.CatchInputWindow:
                    HandleCatchWindow();
                    break;
                case FishingStates.Catching:
                    if (currentFish == null)
                    {
                        return;
                    }

                    UpdateFishingSettings();
                    HandleCatchBarMovement();
                    HandleFishMovement();
                    UpdateProgress();
                    CheckWinLose();
                    break;
                default:
                    break;
            }
        }

        // METHODS
        private void ChangeToState(FishingStates state)
        {
            currentState = state;
        }

        public void OnFishingInput(bool isPressed)
        {
            switch (currentState)
            {
                case FishingStates.Idle:
                    if (isPressed)
                    {
                        BeginCastHold();
                    }
                    break;
                case FishingStates.Casting:
                    if (!isPressed)
                    {
                        ReleaseCastHold();
                    }
                    break;
                case FishingStates.Fishing:
                    EndFishing();
                    break;
                case FishingStates.CatchInputWindow:
                    currentFishingWaitTime = 0f;
                    currentCatchWindowTime = 0f;

                    PlanetConfigSO currentPlanet = LocationManager.Instance.CurrentLocation;
                    FishConfigSO randomFish = DataManager.Instance.GetRandomFishData(currentPlanet);

                    BeginCatching(randomFish);
                    break;
                case FishingStates.Catching:
                    SetCatchBarMovementActive(isPressed);
                    break;
                default:
                    break;
            }
        }

        public void BeginCastHold()
        {
            currentCastingFillSpeed = castingFillSpeed;
            playerAnimator.Play("Cast Start");

            castingTime = 0f;
            currentCastingValue = 0f;

            fishingUI.SetActive(false);
            castingUI.SetActive(true);

            targetFishingWaitTime = Random.Range(minFishingWaitTime, maxFishingWaitTime);

            ChangeToState(FishingStates.Casting);
        }

        public void ReleaseCastHold()
        {
            playerAnimator.Play("Cast End");

            currentCastingFillSpeed = 0f;

            DOVirtual.DelayedCall(0.6f, () =>
            {
                castingUI.SetActive(false);
                ChangeToState(FishingStates.Fishing);
                waterRippleObject.SetActive(true);
            });
        }

        private void HandleCasting()
        {
            castingTime += Time.deltaTime * currentCastingFillSpeed;
            currentCastingValue = Mathf.PingPong(castingTime, 1f);
            castingFillImage.fillAmount = currentCastingValue;
        }

        private void HandleFishingWait()
        {
            currentFishingWaitTime += Time.deltaTime;

            if (currentFishingWaitTime >= targetFishingWaitTime)
            {
                biteWarningObject.SetActive(true);
                currentCatchWindowTime = catchWindowTime;
                ChangeToState(FishingStates.CatchInputWindow);
            }
        }

        private void HandleCatchWindow()
        {
            currentCatchWindowTime -= Time.deltaTime;

            if (currentCatchWindowTime <= 0f)
            {
                currentFishingWaitTime = 0f;
                currentCatchWindowTime = 0f;
                targetFishingWaitTime = Random.Range(minFishingWaitTime, maxFishingWaitTime);

                ChangeToState(FishingStates.Fishing);
                biteWarningObject.SetActive(false);
            }
        }

        public void BeginCatching(FishConfigSO fishConfig)
        {
            playerAnimator.Play("Struggling Start");

            biteWarningObject.SetActive(false);
            ChangeToState(FishingStates.Catching);

            currentFish = fishConfig;
            fishIndicator.GetComponentInChildren<Image>().sprite = fishConfig.Sprite;

            ResetCatchBarPosition();
            
            currentProgress = 0.5f;
            progressBar.fillAmount = currentProgress;

            fishingUI.SetActive(true);
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

            currentCatchBarSize = player.GetUpgradeModifiedValue(UpgradeTypes.IncreaseCatchBarSize, currentCatchBarSize);
            currentFishSpeed = player.GetUpgradeModifiedValue(UpgradeTypes.ReduceFishSpeed, currentFishSpeed);

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
                OnCatchEnd(true);
            }
            else if (currentProgress <= 0f)
            {
                OnCatchEnd(false);
            }
        }

        private void OnCatchEnd(bool success)
        {
            if (success)
            {
                player.Wallet.Add(CurrencyTypes.Gold, currentFish.SellValue);
                CollectionManager.Instance.RegisterCatch(currentFish);
            }

            castingUI.SetActive(false);
            fishingUI.SetActive(false);

            currentFish = null;
            waterRippleObject.SetActive(false);

            playerAnimator.SetBool("Success", success);
            playerAnimator.Play("Struggling End");

            ChangeToState(FishingStates.PostCatch);
            StartCoroutine(CatchEndCoroutine());
        }

        private IEnumerator CatchEndCoroutine()
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

            EndFishing();
        }

        private void EndFishing()
        {
            castingUI.SetActive(false);
            fishingUI.SetActive(false);
            playerAnimator.Play("Idle");

            currentFish = null;
            waterRippleObject.SetActive(false);

            DOVirtual.DelayedCall(0.1f, () =>
            {
                ChangeToState(FishingStates.Idle);
            });
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