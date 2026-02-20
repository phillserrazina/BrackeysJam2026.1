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

            priceText.text = upgrade.Price.ToString("F0");

            UpdateState();
        }

        public void Buy()
        {
            if (playerWallet.Get(CurrencyTypes.Gold) >= associatedUpgrade.Price)
            {
                playerUpgrades.Add(associatedUpgrade);
                UpdateState();
            }
        }

        public void UpdateState()
        {
            if (playerUpgrades.Has(associatedUpgrade))
            {
                gameObject.SetActive(false);
                return;
            }

            iconImage.color = playerWallet.Get(CurrencyTypes.Gold) >= associatedUpgrade.Price ? Color.white : Color.black;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
