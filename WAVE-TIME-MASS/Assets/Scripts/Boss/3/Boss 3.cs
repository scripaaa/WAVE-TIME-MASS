using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Boss3 : Entity
{
    // Основные параметры (без изменений)
    public float speed = 3f;
    public Vector3[] patrolPoints;
    public float attackRange = 2f;
    public float detectionRange = 5f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    // Параметры дальнего боя (без изменений)
    public float rangedAttackRange = 7f;
    public float rangedAttackCooldown = 3f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    // Двери (без изменений)
    public GameObject leftDoor;
    public GameObject rightDoor;

    // Новые события для UI
    public event Action<int> OnHealthChanged;
    public event Action<float> OnCooldownChanged;

    // Здоровье теперь public для доступа из UI
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Immunity Settings")]
    public float immunityCooldown = 20f; // Частота срабатывания
    public int healthRegenAmount = 1;    // Сколько восстанавливать
    public Color immunityColor = Color.gray; // Цвет полосы здоровья

    [Header("UI Reference")]
    public BossUI bossUIController; // Ссылка на контроллер UI

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
        livess = maxHealth; // Синхронизируем с системой Entity

        // Уведомляем UI о начальном состоянии
        OnHealthChanged?.Invoke(currentHealth);
        OnCooldownChanged?.Invoke(1f); // Полная перезарядка

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

    // Модифицированный метод получения урона
    public override void GetDamage()
    {
        if (isImmune) return; // Игнорируем урон в режиме иммунитета

        base.GetDamage(); // Уменьшает livess на 1

        currentHealth = livess; // Синхронизируем значения
        OnHealthChanged?.Invoke(currentHealth); // Уведомляем UI

        if (livess <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        if (isImmune) return;

        // Проверка иммунитета
        CheckImmunity();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool isPlayerInPatrolZone = Vector3.Distance(player.position, patrolZoneCenter) <= patrolZoneRadius;

        // Логика состояний (без изменений)
        if (isPlayerInPatrolZone)
        {
            if (distanceToPlayer <= attackRange)
            {
                currentState = EnemyState.Attacking;
            }
            else if (distanceToPlayer <= rangedAttackRange)
            {
                currentState = EnemyState.RangedAttacking;
                UpdateCooldownUI(); // Добавлен вызов обновления перезарядки
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

        // Остальная логика без изменений
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
        // Активируем иммунитет
        isImmune = true;
        anim.SetTrigger("Immune");

        // Уведомляем UI о смене цвета
        OnHealthChanged += (health) => {
            if (TryGetComponent<BossUI>(out var bossUI))
            {
                bossUI.SetHealthColor(immunityColor);
                Debug.Log($"Changing color to: {immunityColor}"); // Для отладки
            }
        };
        OnHealthChanged?.Invoke(currentHealth);

        // Останавливаем босса
        EnemyState previousState = currentState;
        currentState = EnemyState.Patrolling;

        // Восстанавливаем здоровье
        currentHealth = Mathf.Min(currentHealth + healthRegenAmount, maxHealth);
        livess = currentHealth;

        // Ждем 3 секунды в иммунитете
        yield return new WaitForSeconds(5f);

        // Возвращаем нормальное состояние
        isImmune = false;
        anim.SetTrigger("AnImmune");

        // Возвращаем обычный цвет
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

    // Новый метод для обновления перезарядки
    void UpdateCooldownUI()
    {
        if (currentState == EnemyState.RangedAttacking)
        {
            float progress = (Time.time - lastRangedAttackTime) / rangedAttackCooldown;
            OnCooldownChanged?.Invoke(Mathf.Clamp01(progress));
        }
        else
        {
            OnCooldownChanged?.Invoke(1f); // Скрываем полосу
        }
    }

    void UpdateCooldown()
    {
        // Всегда обновляем прогресс перезарядки
        float progress = (Time.time - lastRangedAttackTime) / rangedAttackCooldown;
        OnCooldownChanged?.Invoke(Mathf.Clamp01(progress));
    }
    void RangedAttack()
    {
        if (isImmune) return; // Не атакуем в режиме иммунитета

        if (Time.time - lastRangedAttackTime >= rangedAttackCooldown)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                anim.SetTrigger("RangeAttack");
            }
            lastRangedAttackTime = Time.time;
            OnCooldownChanged?.Invoke(0f); // Начинаем перезарядку
        }
    }

    // Модифицированная дальняя атака
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
        OnHealthChanged?.Invoke(0); // Обновляем UI перед смертью

        // Открываем двери
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
        if (isImmune) return; // Не атакуем в режиме иммунитета

       
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Атака совершена");
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