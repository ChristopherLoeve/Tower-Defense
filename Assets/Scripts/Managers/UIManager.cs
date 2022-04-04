using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject turretShopPanel;
    [SerializeField] private GameObject nodeUIPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI turretLevelText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI gameOverTotalCoins;

    private Node _currentNodeSelected;

    private void Update()
    {
        coinsText.text = CurrencySystem.Instance.TotalCoins.ToString();
        livesText.text = LevelManager.Instance.TotalLives.ToString();
        currentWaveText.text = $"Wave {LevelManager.Instance.CurrentWave}";
    }

    public void SlowTime()
    {
        Time.timeScale = 0.5f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    public void FastTime()
    {
        Time.timeScale = 2f;
    }

    public void ExtraFastTime()
    {
        Time.timeScale = 5f;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverTotalCoins.text = CurrencySystem.Instance.TotalCoins.ToString();
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseTurretShopPanel()
    {
        turretShopPanel.SetActive(false);
    }

    public void UpgradeTurret()
    {
        _currentNodeSelected.Turret.TurretUpgrade.UpgradeTurret();
        UpdateUpgradeText();
        UpdateSellText();
        UpdateTurretLevel();
    }

    public void SellTurret()
    {
        CloseNodeUI();
        _currentNodeSelected.SellTurret();
        _currentNodeSelected = null;
    }

    private void ShowNodeUI()
    {
        nodeUIPanel.SetActive(true);
        UpdateUpgradeText();
        UpdateSellText();
        UpdateTurretLevel();
    }

    public void CloseNodeUI()
    {
        nodeUIPanel.SetActive(false);
        _currentNodeSelected.HideTurretInfo();
    }

    private void UpdateSellText()
    {
        sellText.text = _currentNodeSelected.Turret.TurretUpgrade.GetSellValue().ToString();
    }

    private void UpdateUpgradeText()
    {
        upgradeText.text = _currentNodeSelected.Turret.TurretUpgrade.UpgradeCost.ToString();
    }

    private void UpdateTurretLevel()
    {
        turretLevelText.text = $"Level {_currentNodeSelected.Turret.TurretUpgrade.Level}";
    }

    private void NodeSelected(Node nodeSelected)
    {
        if (_currentNodeSelected != null)
        {
            _currentNodeSelected.HideTurretInfo();
        }
        _currentNodeSelected = nodeSelected;
        if (_currentNodeSelected.IsEmpty())
        {
            CloseNodeUI();
            turretShopPanel.SetActive(true);
        }
        else
        {
            ShowNodeUI();
        }
    }

    private void OnEnable()
    {
        Node.OnNodeSelected += NodeSelected;
    }

    private void OnDisable()
    {
        Node.OnNodeSelected -= NodeSelected;
    }
}
