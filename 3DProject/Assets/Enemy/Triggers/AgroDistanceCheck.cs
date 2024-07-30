using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroDistanceCheck : MonoBehaviour
{
    public GameObject PlayerTarget {  get; set; }
    private Enemy _enemy;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == PlayerTarget)
        {
            _enemy.SetAggroStatus(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == PlayerTarget)
        {
            _enemy.SetAggroStatus(false);
        }
    }
}
