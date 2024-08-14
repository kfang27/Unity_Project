using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTakenCheck : MonoBehaviour
{
    private GameObject _playerweapon;
    private Enemy _enemy;

    private void Awake()
    {
        _playerweapon = GameObject.FindGameObjectWithTag("Player");
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == _playerweapon)
        {
            _enemy.Damage(10f);
        }
    }
}
