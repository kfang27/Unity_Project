using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;

    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" || collision.tag == "Hittable")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage);
            }
        }

        if (collision.tag == "Enemy" || collision.tag == "Hittable")
        {
            DummyEnemy enemy = collision.GetComponent<DummyEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeaponDamage);
            }
        }
    }
}
