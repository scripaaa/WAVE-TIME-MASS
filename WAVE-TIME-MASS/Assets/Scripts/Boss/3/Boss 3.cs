using UnityEngine;
using UnityEngine.UI;

public class Boss3 : Entity
{
    public float speed = 3f;
    public Vector3[] patrolPoints;
    public float attackRange = 2f;
    public float detectionRange = 5f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    private SpriteRenderer sprite;
    private Transform player;
    private float lastAttackTime;
    private int currentPatrolIndex;
    private bool isPlayerDetected;

    private Vector3 patrolZoneCenter;
    private float patrolZoneRadius;

    [Header("Ranged Attack Settings")]
    public float rangedAttackRange = 7f;   // ��������� ������� �����
    public float rangedAttackCooldown = 3f; // �����������
    public GameObject projectilePrefab;     // ������ �������
    public Transform firePoint;            // ����� ��������

    private float lastRangedAttackTime;
    private bool isRangedAttacking;

    [Header("Cooldown UI")]
    public Image cooldownFill;
    public GameObject cooldownUI;

    [Header("Health UI")]
    public Slider bossHealthSlider;
    public GameObject healthUI;

    [Header("Doors")]
    public GameObject leftDoor;  // ����� �����
    public GameObject rightDoor; // ������ �����

    private enum EnemyState { Patrolling, Chasing, Attacking, RangedAttacking }
    private EnemyState currentState;

    void Start()
    {
        livess = 10; // ������ � ����� 10 HP

        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = 10;                     // ���� ��� ��������
            bossHealthSlider.value = livess;
        }

        sprite = GetComponentInChildren<SpriteRenderer>();
        player = Hero.Instance.transform;
        currentState = EnemyState.Patrolling;

        // ���� ����� �������������� �� ������, ���������� ������� �������
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            patrolPoints = new Vector3[] { transform.position, transform.position + new Vector3(3, 0, 0) };
        }

        CalculatePatrolZone();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool isPlayerInPatrolZone = Vector3.Distance(player.position, patrolZoneCenter) <= patrolZoneRadius;

        // ���������� ���������
        if (isPlayerInPatrolZone)
        {
            if (distanceToPlayer <= attackRange)
            {
                currentState = EnemyState.Attacking;
            }
            else if (distanceToPlayer <= rangedAttackRange)
            {
                currentState = EnemyState.RangedAttacking;
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

        // ��������� �������� � ����������� �� ���������
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
        UpdateCooldownUI();
    }

    // ��������� ���� ��������������
    void CalculatePatrolZone()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            patrolZoneCenter = transform.position;
            patrolZoneRadius = 5f; // ��������� ������, ���� ����� ���
            return;
        }

        // ������� ����� ���� (������� ���� �����)
        patrolZoneCenter = Vector3.zero;
        foreach (Vector3 point in patrolPoints)
        {
            patrolZoneCenter += point;
        }
        patrolZoneCenter /= patrolPoints.Length;

        // ������� ������������ ���������� �� ������ �� ����� �����
        patrolZoneRadius = 0f;
        foreach (Vector3 point in patrolPoints)
        {
            float distance = Vector3.Distance(patrolZoneCenter, point);
            if (distance > patrolZoneRadius)
            {
                patrolZoneRadius = distance;
            }
        }

        // ��������� ��������� ����� (��������, 20%)
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
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Hero.Instance.GetDamageHero();
            lastAttackTime = Time.time;
            // ����� �������� �������� �����
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
            // �������������� � ������ ��� ����� ��� �������������
            sprite.flipX = player.position.x < transform.position.x;
        }
    }

    void RangedAttack()
    {
        if (Time.time - lastRangedAttackTime >= rangedAttackCooldown)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.identity
                );

                // ���������� ����������� �� flipX (��� ������� ������)
                bool isFacingRight = !sprite.flipX;
                Projectile3 projectileScript = projectile.GetComponent<Projectile3>();
                projectileScript.moveRight = isFacingRight;

                // ������������ ������ ������� (�����������)
                if (!isFacingRight)
                {
                    projectile.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            lastRangedAttackTime = Time.time;
        }
    }

    void UpdateCooldownUI()
    {
        if (cooldownUI != null)
        {
            // ���������� UI ������ ��� �����������
            cooldownUI.SetActive(Time.time - lastRangedAttackTime < rangedAttackCooldown);

            // ��������� �����
            float cooldownProgress = (Time.time - lastRangedAttackTime) / rangedAttackCooldown;
            cooldownFill.fillAmount = Mathf.Clamp01(cooldownProgress);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamageHero();
            if (livess < 1) Die();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // ���� �������������� (������)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolZoneCenter, patrolZoneRadius);

        // ������� ����� (�������)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ������� ����� (�����)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);

        // ����� �������������� (����� �����)
        Gizmos.color = Color.yellow;
        if (patrolPoints != null)
        {
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }
    }

    public override void GetDamage()
    {
        base.GetDamage();

        if (bossHealthSlider != null)
        {
            bossHealthSlider.value = livess;
        }

        if (livess <= 0)
        {
            healthUI.SetActive(false);
        }
    }

    public override void Die()
    {
        base.Die();

        // ��������� �����
        if (leftDoor != null) leftDoor.SetActive(false);
        if (rightDoor != null) rightDoor.SetActive(false);
    }
}