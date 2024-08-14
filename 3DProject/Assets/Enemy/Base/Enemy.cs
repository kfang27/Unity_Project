using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{

    [SerializeField] private float speed = 500f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float rotationThreshold = 5f;
    [SerializeField] private GameObject _player;
    private CharacterController _controller;
    public EnemyHealthBar healthBar;
    public GameObject bossUI;
    public GameObject winText;

    public float MaxHealth { get; set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
    public bool IsDoingCombo { get; set; }
    public bool IsAttacking { get; set; }

    private Animator _animator;
    public Animator Animator => _animator;

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    #endregion

    #region MonoBehavior
    void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);

        Debug.Log("AWAKE");
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        CurrentHealth = MaxHealth;
        healthBar.SetMaxHealth(CurrentHealth);

        Debug.Log("START");
        StateMachine.Initialize(IdleState);
    }

    void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    #endregion

    #region Health Fucntions
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        healthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        bossUI.SetActive(false);

    }
    #endregion

    #region Movement Functions
    public void RotateTowardsPlayer()
    {
        Vector3 targetPosition = _player.transform.position;
        targetPosition.y = transform.position.y; // Keep the enemy on the same Y level
        Vector3 direction = targetPosition - transform.position;

        // Rotate the enemy to face the player
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void MoveTowardsPlayer()
    {
        Vector3 targetPosition = _player.transform.position;
        targetPosition.y = transform.position.y; // Keep the enemy on the same Y level
        Vector3 moveDirection = (targetPosition - transform.position).normalized; // Normalize the movement direction

        // Move the enemy towards the player
        _controller.SimpleMove(moveDirection * speed);
    }

    public bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        directionToPlayer.y = 0; // Keep the comparison on the horizontal plane
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < rotationThreshold;
    }
    #endregion

    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        EnemyDamaged
    }
    #endregion

    #region Trigger Checks
    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetWithinStrikingDistance(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }

    public void SetDoingComboStatus(bool isDoingCombo)
    {
        IsDoingCombo = isDoingCombo;
    }

    public void SetAttackingStatus(int isAttacking)
    {
        IsAttacking = (isAttacking == 1);
    }
    #endregion

    #region Attack Functions
    public virtual void ChooseRandomAttack(string type)
    {
        _animator.SetInteger(type, Random.Range(0, 2));
        SetDoingComboStatus(true);
    }

    public virtual void ResetCombo() 
    {
        _animator.SetInteger("StartingAttack", -1);
        _animator.SetInteger("FollowUp", -1);
    }
    #endregion
}
