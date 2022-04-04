using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgrade : MonoBehaviour
{
    [SerializeField] private int upgradeInitialCost;
    [SerializeField] private int upgradeCostIncremental;
    [SerializeField] private float damageIncremental;
    [SerializeField] private float delayReduction;

    [Header("Sell")]
    [Range(0, 1)]
    [SerializeField] private float sellPercentage;

    public int UpgradeCost { get; set; }
    public int Level { get; set; }


    private float SellPercentage;
    private TurretProjectile _turretProjectile;

    private void Start()
    {
        _turretProjectile = GetComponent<TurretProjectile>();
        UpgradeCost = upgradeInitialCost;
        SellPercentage = sellPercentage;
        Level = 1;
    }

    public void UpgradeTurret()
    {
        if (CurrencySystem.Instance.TotalCoins >= UpgradeCost)
        { 
            _turretProjectile.Damage += damageIncremental;
            _turretProjectile.DelayBetweenAttacks -= delayReduction;
            UpdateUpgrade();
        }
    }

    public int GetSellValue()
    {
        return Mathf.RoundToInt(UpgradeCost * sellPercentage);
    }

    private void UpdateUpgrade()
    {
        CurrencySystem.Instance.RemoveCoins(UpgradeCost);
        UpgradeCost += upgradeCostIncremental;
        Level++;
    }

}
