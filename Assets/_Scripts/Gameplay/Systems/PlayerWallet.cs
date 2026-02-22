using System;
using System.Collections.Generic;

using UnityEngine;
using FishingGame.Audio;

public enum CurrencyTypes { Gold }

namespace FishingGame.Gameplay.Systems
{
    public class PlayerWallet
    {
        // VARIABLES
        public Dictionary<CurrencyTypes, float> currencyDictionary = new();

        public event Action<CurrencyChangeData> OnWalletChanged;

        // METHODS
        public void Add(CurrencyTypes currencyType, float amount)
        {
            if (currencyDictionary.ContainsKey(currencyType))
            {
                currencyDictionary[currencyType] += amount;
            }
            else
            {
                currencyDictionary.Add(currencyType, amount);
            }

            OnWalletChanged?.Invoke(new(currencyType, amount));
            // Play money gain SFX if available
            AudioManager.Instance?.PlayMoneyGain();
        }

        public float Get(CurrencyTypes currencyType)
        {
            if (currencyDictionary.TryGetValue(currencyType, out float amount))
            {
                return amount;
            }

            return 0f;
        }
        public void Spend(CurrencyTypes currencyType, float amount) 
        {
            if (currencyDictionary.ContainsKey(currencyType))
            {
                currencyDictionary[currencyType] -= amount;
                currencyDictionary[currencyType] = Mathf.Max(currencyDictionary[currencyType], 0f);
            }
            else
            {
                currencyDictionary.Add(currencyType, 0);
            }

            OnWalletChanged?.Invoke(new(currencyType, -amount));
        }
    }

    public readonly struct CurrencyChangeData
    {
        // VARIABLES
        public readonly CurrencyTypes CurrencyType;
        public readonly float Delta;

        // CONSTRUCTOR
        public CurrencyChangeData(CurrencyTypes currencyType, float delta)
        {
            CurrencyType = currencyType;
            Delta = delta;
        }
    }
}
