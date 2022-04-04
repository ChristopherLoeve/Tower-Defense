using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public static Action OnTurretSold;
    public static Action<Node> OnNodeSelected;

    [SerializeField] private GameObject attackRangeSprite;

    public Turret Turret { get; set; }

    private float _rangeSize;
    private Vector3 _rangeOriginalSize;

    private void Start()
    {
        _rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        _rangeOriginalSize = attackRangeSprite.transform.localScale;
    }

    public void SetTurret(Turret turret)
    {
        Turret = turret;
    }

    public void SellTurret()
    {
        if (!IsEmpty())
        {
            CurrencySystem.Instance.AddCoins(Turret.TurretUpgrade.GetSellValue());
            Destroy(Turret.gameObject);
            Turret = null;
            OnTurretSold?.Invoke();
        }
    }

    public bool IsEmpty()
    {
        return Turret == null;
    }

    public void SelectTurret()
    {
        OnNodeSelected?.Invoke(this);
        if (!IsEmpty())
        {
            ShowTurretInfo();
        }
    }

    private void ShowTurretInfo()
    {
        attackRangeSprite.SetActive(true);
        attackRangeSprite.transform.localScale = _rangeOriginalSize * Turret.AttackRange / (_rangeSize / 2);
    }

    public void HideTurretInfo()
    {
        attackRangeSprite.SetActive(false);
    }
}
