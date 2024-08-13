using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerManager playerManager;

    public HealthBar healthBar;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;
    public bool isDead;

    public StaminaBar staminaBar;
    public int maxStamina = 100;
    public int currentStamina;
    public float staminaRegenRate = 5;
    public float staminaRegenDelay = 2;

    private bool isRegeneratingStamina;
    private float regenTimer;

    AnimatorHandler animatorHandler;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        healthBar = FindObjectOfType<HealthBar>();

        animatorHandler = GetComponentInChildren<AnimatorHandler>();

    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (playerManager.isInvulnerable)
            return;

        if (isDead)
            return;

        currentHealth -= damage;

        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Dead_01", true);
            isDead = true;

            //HANDLE PLAYER DEATH (like option to revive/respawn or restart)
        }
    }

}
