using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGiven : MonoBehaviour
{
    [SerializeField] private GameObject PlayerTarget;
    [SerializeField] private PlayerStats _playerStats;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("SOMETHING INSIDE");
        if (collider.gameObject == PlayerTarget)
        {
            _playerStats.TakeDamage(25);
        }
    }
}
