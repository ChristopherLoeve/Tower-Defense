using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Action<Enemy, float> OnEnemyHit;

    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] private float minDistanceToDealDamage = 1;

    public TurretProjectile TurretOwner { get; set; }
    public float Damage { get; set; }

    protected Enemy _enemyTarget;


    protected virtual void Update()
    {
        if (_enemyTarget != null)
        {
            MoveProjectile();
            RotateProjectile();
        }
    }

    protected virtual void MoveProjectile()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            _enemyTarget.transform.position, moveSpeed * Time.deltaTime);

        float distanceToTarget = (_enemyTarget.transform.position - transform.position).magnitude;
        if (distanceToTarget < minDistanceToDealDamage)
        {
            OnEnemyHit?.Invoke(_enemyTarget, Damage);
            _enemyTarget.EnemyHealth.DealDamage(Damage);
            TurretOwner.ResetTurretProjectile();
            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private void RotateProjectile()
    {
        Vector2 direction = (Vector2)(_enemyTarget.transform.position - transform.position);
        transform.up = direction;
    }

    public void SetEnemy(Enemy enemy)
    {
        _enemyTarget = enemy;
    }

    public void ResetProjectile()
    {
        _enemyTarget = null;
        transform.localRotation = Quaternion.identity;
    }
}
