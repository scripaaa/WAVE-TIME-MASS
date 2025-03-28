using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Boss3 : Entity
{
    // �������� ��������� (��� ���������)
    public float speed = 3f;
    public Vector3[] patrolPoints;
    public float attackRange = 2f;
    public float detectionRange = 5f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    // ��������� �������� ��� (��� ���������)
    public float rangedAttackRange = 7f;
    public float rangedAttackCooldown = 3f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    // ����� (��� ���������)
    public GameObject leftDoor;
    public GameObject rightDoor;

    // ����� ������� ��� UI
    public event Action<int> OnHealthChanged;
    public event Action<float> OnCooldownChanged;

    // �������� ������ public ��� ������� �� UI
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Immunity Settings")]
    public float immunityCooldown = 20f; // ������� ������������
    public int healthRegenAmount = 1;    // ������� ���������������
    public Color immunityColor = Color.gray; // ���� ������ ��������

    [Header("UI Reference")]
    public BossUI bossUIController; // ������ �� ���������� UI

    private float lastImmunityTime;
    private bool isImmune = false;
    private Color normalHealthColor;

    private SpriteRenderer sprite;
    private Animator anim;
    private Transform player;
    private float lastAttackTime;
    private float lastRangedAttackTime;
    private int currentPatrolIndex;
    private Vector3 patrolZoneCenter;
    private float patrolZoneRadius;

    private enum EnemyState { Patrolling, Chasing, Attacking, RangedAttacking }
    private EnemyState currentState;

    void Start()
    {
        currentHealth = maxHealth;
        livess = maxHealth; // �������������� � �������� Entity

        // ���������� UI � ��������� ���������
        OnHealthChanged?.Invoke(currentHealth);
        OnCooldownChanged?.Invoke(1f); // ������ �����������

        sprite = GetComponentInChildren<SpriteRenderer>();
        player = Hero.Instance.transform;
        anim = GetComponent<Animator>();
        currentState = EnemyState.Patrolling;

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            patrolPoints = new Vector3[] { transform.position, transform.position + new Vector3(3, 0, 0) };
        }

        CalculatePatrolZone();

        lastImmunityTime = Time.time;
        normalHealthColor = bossUIController.healthSlider.fillRect.GetComponent<Image>().color;
    }

    // ���������������� ����� ��������� �����
    public override void GetDamage()
    {
        if (isImmune) return; // ���������� ���� � ������ ����������

        base.GetDamage(); // ��������� livess �� 1

        currentHealth = livess; // �������������� ��������
        OnHealthChanged?.Invoke(currentHealth); // ���������� UI

        if (livess <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        if (isImmune) return;

        // �������� ����������
        CheckImmunity();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool isPlayerInPatrolZone = Vector3.Distance(player.position, patrolZoneCenter) <= patrolZoneRadius;

        // ������ ��������� (��� ���������)
        if (isPlayerInPatrolZone)
        {
            if (distanceToPlayer <= attackRange)
            {
                currentState = EnemyState.Attacking;
            }
            else if (distanceToPlayer <= rangedAttackRange)
            {
                currentState = EnemyState.RangedAttacking;
                UpdateCooldownUI(); // �������� ����� ���������� �����������
            }
            else
            {
                currentState = EnemyState.Chasing;
            }
        }
        else
        {
            currentState = EnemyState.Patrolling;
        }

        // ��������� ������ ��� ���������
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.RangedAttacking:
                RangedAttack();
                break;
        }

        FlipSprite();
        UpdateCooldown();
    }

    void CheckImmunity()
    {
        if (Time.time - lastImmunityTime >= immunityCooldown && !isImmune)
        {
            StartCoroutine(ActivateImmunity());
        }
    }

    IEnumerator ActivateImmunity()
    {
        // ���������� ���������
        isImmune = true;
        anim.SetTrigger("Immune");

        // ���������� UI � ����� �����
        OnHealthChanged += (health) => {
            if (TryGetComponent<BossUI>(out var bossUI))
            {
                bossUI.SetHealthColor(immunityColor);
                Debug.Log($"Changing color to: {immunityColor}"); // ��� �������
            }
        };
        OnHealthChanged?.Invoke(currentHealth);

        // ������������� �����
        EnemyState previousState = currentState;
        currentState = EnemyState.Patrolling;

        // ��������������� ��������
        currentHealth = Mathf.Min(currentHealth + healthRegenAmount, maxHealth);
        livess = currentHealth;

        // ���� 3 ������� � ����������
        yield return new WaitForSeconds(5f);

        // ���������� ���������� ���������
        isImmune = false;
        anim.SetTrigger("AnImmune");

        // ���������� ������� ����
        OnHealthChanged += (health) => {
            if (TryGetComponent<BossUI>(out var bossUI))
            {
                bossUI.SetHealthColor(normalHealthColor);
            }
        };
        OnHealthChanged?.Invoke(currentHealth);

        currentState = previousState;
        lastImmunityTime = Time.time;
    }

    // ����� ����� ��� ���������� �����������
    void UpdateCooldownUI()
    {
        if (currentState == EnemyState.RangedAttacking)
        {
            float progress = (Time.time - lastRangedAttackTime) / rangedAttackCooldown;
            OnCooldownChanged?.Invoke(Mathf.Clamp01(progress));
        }
        else
        {
            OnCooldownChanged?.Invoke(1f); // �������� ������
        }
    }

    void UpdateCooldown()
    {
        // ������ ��������� �������� �����������
        float progress = (Time.time - lastRangedAttackTime) / rangedAttackCooldown;
        OnCooldownChanged?.Invoke(Mathf.Clamp01(progress));
    }
    void RangedAttack()
    {
        if (isImmune) return; // �� ������� � ������ ����������

        if (Time.time - lastRangedAttackTime >= rangedAttackCooldown)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                anim.SetTrigger("RangeAttack");
            }
            lastRangedAttackTime = Time.time;
            OnCooldownChanged?.Invoke(0f); // �������� �����������
        }
    }

    // ���������������� ������� �����
    void OnRangedAttack()
    {
       
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                Projectile3 projectileScript = projectile.GetComponent<Projectile3>();
                projectileScript.moveRight = !sprite.flipX;

                if (!sprite.flipX)
                {
                    projectile.transform.localScale = new Vector3(-1, 1, 1);
                }

    }

    public override void Die()
    {
        OnHealthChanged?.Invoke(0); // ��������� UI ����� �������

        // ��������� �����
        if (leftDoor != null) leftDoor.SetActive(false);
        if (rightDoor != null) rightDoor.SetActive(false);

        anim.SetTrigger("Die");
    }
    private void OnDie()
    {
        base.Die();
    }

    void CalculatePatrolZone()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            patrolZoneCenter = transform.position;
            patrolZoneRadius = 5f;
            return;
        }

        patrolZoneCenter = Vector3.zero;
        foreach (Vector3 point in patrolPoints)
        {
            patrolZoneCenter += point;
        }
        patrolZoneCenter /= patrolPoints.Length;

        patrolZoneRadius = 0f;
        foreach (Vector3 point in patrolPoints)
        {
            float distance = Vector3.Distance(patrolZoneCenter, point);
            if (distance > patrolZoneRadius)
            {
                patrolZoneRadius = distance;
            }
        }
        patrolZoneRadius *= 1.2f;
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector3 target = patrolPoints[currentPatrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if (isImmune) return; // �� ������� � ������ ����������

       
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("����� ���������");
            anim.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    void FlipSprite()
    {
        if (currentState == EnemyState.Patrolling && patrolPoints.Length > 0)
        {
            Vector3 moveDirection = patrolPoints[currentPatrolIndex] - transform.position;
            sprite.flipX = moveDirection.x < 0;
        }
        else if (currentState != EnemyState.Patrolling)
        {
            sprite.flipX = player.position.x < transform.position.x;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
   // {
    //    if (collision.gameObject == Hero.Instance.gameObject)
    //    {
    //        Hero.Instance.GetDamageHero();
    //        if (livess < 1) Die();
     //   }
    //}
    private void HeroDamage()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= 3.2f)
        {
            Hero.Instance.GetDamageHero();
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolZoneCenter, patrolZoneRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);

        Gizmos.color = Color.yellow;
        if (patrolPoints != null)
        {
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }
    }
}