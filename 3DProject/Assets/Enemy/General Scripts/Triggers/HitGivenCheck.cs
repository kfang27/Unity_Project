using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGivenCheck : MonoBehaviour
{
    private Enemy _enemy;
    private PlayerStats _playerStats;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        _playerStats = collider.GetComponent<PlayerStats>();

        GiveDamage();
    }

    private void GiveDamage()
    {
        if (_playerStats != null && _enemy.IsAttacking)
        {
            _playerStats.TakeDamage(25);
        }
    }
}
