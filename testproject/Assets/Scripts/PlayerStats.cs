using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorHandler animatorHandler;
    InputHandler inputHandler;

    public HealthBar healthBar;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;
    public bool isDead;

    StaminaBar staminaBar;
    public float staminaLevel = 10;
    public float maxStamina;
    public float currentStamina;
    public float staminaRegenAmount = 7;
    public float staminaRegenTimer = 0;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputHandler = GetComponent<InputHandler>();

        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();

        animatorHandler = GetComponentInChildren<AnimatorHandler>();

    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);

    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
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

    public void DrainStamina(float drainCost)
    {
        currentStamina -= drainCost;
        staminaBar.SetCurrentStamina(currentStamina);
    }

    public void RegenerateStamina()
    {
        if (inputHandler.isInteracting)
        {
            staminaRegenTimer = 0;
        }

        else
        {
            staminaRegenTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenTimer > 1f)
            {
                currentStamina += staminaRegenAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
    }
}
