using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

using FishingGame.Data;
using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class UpgradeElementUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // VARIABLES
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text priceText;

        [Header("Tooltip")]
        [SerializeField] private TMP_Text tooltipText;
        [SerializeField] private GameObject tooltipObject;

        private PlayerWallet playerWallet;
        private PlayerUpgradesHandler playerUpgrades;
        private UpgradeConfigSO associatedUpgrade;

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            playerWallet = PlayerManager.Instance.Wallet;
            playerUpgrades = PlayerManager.Instance.Upgrades;
        }

        // METHODS
        public void Initialize(UpgradeConfigSO upgrade)
        {
            associatedUpgrade = upgrade;
            iconImage.sprite = upgrade.Sprite;

            tooltipText.text = $"<size=15><b>{upgrade.Name}</b></size>\n<size=25>{upgrade.Description}</size>";
            priceText.text = upgrade.PricePerLevel.ToString("F0");

            UpdateState();
        }

        public void Buy()
        {
            if (playerWallet.Get(CurrencyTypes.Gold) >= associatedUpgrade.PricePerLevel)
            {
                playerWallet.Spend(CurrencyTypes.Gold, associatedUpgrade.PricePerLevel);
                playerUpgrades.Add(associatedUpgrade);
                UpdateState();
            }
        }

        public void UpdateState()
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipObject.SetActive(false);
        }
    }
}
