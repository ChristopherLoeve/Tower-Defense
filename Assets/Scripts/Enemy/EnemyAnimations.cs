using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    private EnemyHealth _enemyHealth;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyHealth = GetComponent<EnemyHealth>();

    }

    private void PlayHurtAnimation()
    {
        _animator.SetTrigger("Hurt");
    }

    private void PlayDieAnimation()
    {
        _animator.SetTrigger("Die");
    }

    private float GetCurrentAnimationLength()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private IEnumerator PlayHurt()
    {
        _enemy.SlowMovement(0.5f);
        PlayHurtAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.3f);
        _enemy.ResumeMovement();
    }

    private IEnumerator PlayDead()
    {
        _enemy.StopMovement();
        PlayDieAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.3f);
        _enemy.ResumeMovement();
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(_enemy.gameObject);
    }

    private void EnemyHit(Enemy enemy)
    {
        if (_enemy == enemy)
        {
            StartCoroutine(PlayHurt());
        }
    }

    private void EnemyKilled(Enemy enemy)
    {
        if (_enemy == enemy)
        {
            StartCoroutine(PlayDead());
        }
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyHit += EnemyHit;
        EnemyHealth.OnEnemyKilled += EnemyKilled;
    }



    private void OnDisable()
    {
        EnemyHealth.OnEnemyHit -= EnemyHit;
        EnemyHealth.OnEnemyKilled -= EnemyKilled;
    }

}
