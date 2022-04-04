using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : Singleton<CurrencySystem>
{
    [SerializeField] private int coinTest;
    private string CURRENCY_SAVE_KEY = "MYGAME_CURRENCY";

    public int TotalCoins { get; set; }

    private void Start()
    {
        LoadCoins();
    }

    private void LoadCoins()
    {
        PlayerPrefs.DeleteKey(CURRENCY_SAVE_KEY);
        TotalCoins = PlayerPrefs.GetInt(CURRENCY_SAVE_KEY, coinTest);
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        SaveCoinsToPlayerPrefs();
    }

    public void RemoveCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            SaveCoinsToPlayerPrefs();
        }
    }

    private void SaveCoinsToPlayerPrefs()
    {
        PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
        PlayerPrefs.Save();
    }

    private void AddCoins(Enemy enemy)
    {
        AddCoins(5);
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += AddCoins;
    }

    private void OnDisable()
    {
        
    }
}
