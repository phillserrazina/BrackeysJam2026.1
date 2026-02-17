using UnityEngine;
using TMPro;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class GoldUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private TMP_Text goldText;

        private PlayerWallet playerWallet;

        // EXECUTION FUNCTIONS
        private void Start()
        {
            playerWallet = PlayerManager.Instance.Wallet;
            playerWallet.OnWalletChanged += PlayerWallet_OnWalletChanged;

            goldText.text = $"${playerWallet.Get(CurrencyTypes.Gold)}";
        }

        // CALLBACKS
        private void PlayerWallet_OnWalletChanged(CurrencyChangeData changeData)
        {
            goldText.text = $"${playerWallet.Get(CurrencyTypes.Gold)}";
        }
    }
}
