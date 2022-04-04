using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Action<Enemy> OnEndReached;

    [SerializeField] private float movementSpeed = 3f;
    public float MovementSpeed { get; set; }

    public Waypoint Waypoint { get; set; }


    public Vector3 CurrentPointPosition => Waypoint.GetWaypointPosition(_currentWaypointIndex);

    private int _currentWaypointIndex;
    private Vector3 _lastPointPosition;
    public EnemyHealth EnemyHealth;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        EnemyHealth = GetComponent<EnemyHealth>();
        _currentWaypointIndex = 0;
        _lastPointPosition = transform.position;
        MovementSpeed = movementSpeed;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Rotate();
    }

    private void Update()
    {
        Move();
        Rotate();
        if (CurrentPointPositionReached()) UpdateCurrentPointIndex();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, MovementSpeed * Time.deltaTime);
    }

    public void StopMovement()
    {
        MovementSpeed = 0f;
    }

    public void ResumeMovement()
    {
        MovementSpeed = movementSpeed;
    }

    public void SlowMovement(float percent)
    {
        MovementSpeed = movementSpeed * percent;
    }

    private void Rotate()
    {
        if (CurrentPointPosition.x > _lastPointPosition.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;

        if ( distanceToNextPointPosition < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }

        return false;
    }

    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = Waypoint.Points.Length - 1;
        if (_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;

        }
        else
        {
            _currentWaypointIndex = 0;
            EndPointReached();
        }
    }

    private void EndPointReached()
    {
        OnEndReached?.Invoke(this);
        EnemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void ResetEnemy()
    {
        _currentWaypointIndex = 0;

    }
}
