using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGiven : MonoBehaviour
{
    private Enemy _enemy;
    private bool _isHit = false;
    private GameObject _target;
    private PlayerStats _playerStats;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        _target = collider.gameObject;
        _playerStats = _target.GetComponent<PlayerStats>();

        GiveDamage();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (!_isHit)
        {
            GiveDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isHit)
        {
            _isHit = false;
        }
    }

    private void GiveDamage()
    {
        if (_playerStats != null && _enemy.IsAttacking)
        {
            _playerStats.TakeDamage(25);
            _isHit = true;
        }
    }
}
