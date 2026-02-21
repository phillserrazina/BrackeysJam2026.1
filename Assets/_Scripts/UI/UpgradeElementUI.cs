using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

using FishingGame.Data;
using FishingGame.Gameplay.Systems;
using DG.Tweening;

namespace FishingGame.UI
{
    public class UpgradeElementUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        // VARIABLES
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject[] fillObjects;

        [Header("Tooltip")]
        [SerializeField] private TMP_Text tooltipText;
        [SerializeField] private GameObject tooltipObject;

        private PlayerWallet playerWallet;
        private PlayerUpgradesHandler playerUpgrades;
        private UpgradeConfigSO associatedUpgrade;

        private Vector3 originalIconScale;


        // EXECUTION FUNCTIONS
        private void Awake()
        {
            playerWallet = PlayerManager.Instance.Wallet;
            playerUpgrades = PlayerManager.Instance.Upgrades;

            originalIconScale = iconImage.transform.localScale;
        }

        private void OnDisable()
        {
            iconImage.transform.localScale = originalIconScale;
        }

        // METHODS
        public void Initialize(UpgradeConfigSO upgrade)
        {
            associatedUpgrade = upgrade;
            iconImage.sprite = upgrade.Sprite;

            tooltipText.text = $"<size=15><b>{upgrade.Name}</b></size>\n<size=25>{upgrade.Description}</size>";

            UpdateState();
        }

        public void Buy()
        {
            if (playerUpgrades.GetUpgradeLevel(associatedUpgrade) >= associatedUpgrade.MaxLevel)
            {
                return;
            }

            float upgradePrice = GetUpgradePrice();

            if (playerWallet.Get(CurrencyTypes.Gold) >= upgradePrice)
            {
                playerWallet.Spend(CurrencyTypes.Gold, upgradePrice);
                playerUpgrades.Increment(associatedUpgrade);
                UpdateState();
            }
        }

        public void UpdateState()
        {
            priceText.text = $"${GetUpgradePrice():F0}";
            int currentLevel = playerUpgrades.GetUpgradeLevel(associatedUpgrade);

            for (int i = 0; i < currentLevel; i++) 
            {
                fillObjects[i].SetActive(i < currentLevel);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            iconImage.transform.DOScale(originalIconScale * 1.1f, 0.1f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Buy();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            iconImage.transform.DOScale(originalIconScale, 0.1f);
        }

        private float GetUpgradePrice()
        {
            if (associatedUpgrade == null)
            {
                return 0f;
            }

            return associatedUpgrade.PricePerLevel * (playerUpgrades.GetUpgradeLevel(associatedUpgrade) + 1);
        }

        
    }
}
