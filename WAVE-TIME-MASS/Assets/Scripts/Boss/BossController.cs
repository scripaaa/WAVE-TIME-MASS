using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private Transform patrolLeftBound;
    [SerializeField] private Transform patrolRightBound;
    [SerializeField] private Transform attackTerritory; // Объект с коллайдером территории атаки
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float rangedAttackCooldown = 3f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float immunityDuration = 5f;
    [SerializeField] private float immunityCooldown = 10f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private int currentHealth;
    private bool isFacingRight = true;
    private bool isChasing = false;
    private bool isInTerritory = false;
    private bool isImmune = false;
    private float lastRangedAttackTime;
    private float lastImmunityTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        lastImmunityTime = Time.time;
    }

    private void Update()
    {
        if (isImmune || currentHealth <= 0) return;

        CheckImmunity();
        CheckPlayerInTerritory();
        HandleAttacks();
    }

    private void FixedUpdate()
    {
        if (isImmune || currentHealth <= 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Анимация ходьбы
        //animator.SetBool("IsWalking", true);

        float moveDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);

        // Проверка достижения границ патрулирования
        if (isFacingRight && transform.position.x >= patrolRightBound.position.x)
        {
            Flip();
        }
        else if (!isFacingRight && transform.position.x <= patrolLeftBound.position.x)
        {
            Flip();
        }
    }

    private void ChasePlayer()
    {
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }

        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

        // Анимация ходьбы
        //animator.SetBool("IsWalking", true);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void CheckPlayerInTerritory()
    {
        Collider2D territoryCollider = attackTerritory.GetComponent<Collider2D>();
        isInTerritory = territoryCollider.OverlapPoint(player.position);

        if (isInTerritory)
        {
            isChasing = true;
        }
        else
        {
            // Проверка видимости игрока (простейшая реализация)
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, playerLayer);
            isChasing = hit.collider != null && hit.collider.CompareTag("Player");
        }
    }

    private void HandleAttacks()
    {
        if (isInTerritory && Vector2.Distance(transform.position, player.position) <= meleeRange)
        {
            // Ближняя атака
            //animator.SetTrigger("MeleeAttack");
            // Здесь должна быть логика нанесения урона игроку
        }
        else if (!isInTerritory && Time.time > lastRangedAttackTime + rangedAttackCooldown)
        {
            // Дальняя атака
            //animator.SetTrigger("RangedAttack");
            lastRangedAttackTime = Time.time;
            // Создание снаряда
            Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        }
    }

    private void CheckImmunity()
    {
        if (Time.time > lastImmunityTime + immunityCooldown)
        {
            StartImmunity();
        }
    }

    private void StartImmunity()
    {
        isImmune = true;
        lastImmunityTime = Time.time;
        //animator.SetBool("IsImmune", true);
        rb.velocity = Vector2.zero;

        // Восстановление здоровья
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);

        Invoke("EndImmunity", immunityDuration);
    }

    private void EndImmunity()
    {
        isImmune = false;
        //animator.SetBool("IsImmune", false);
    }

    public void TakeDamage(int damage)
    {
        if (isImmune || currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            //animator.SetTrigger("Hurt");
        }
    }

    private void Die()
    {
        // Анимация смерти
        //animator.SetTrigger("Die");

        // Отключение коллайдеров и физики
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;

        // Отключение скрипта
        enabled = false;

        // Здесь можно добавить логику уничтожения объекта после анимации смерти
        // Destroy(gameObject, 2f);
    }

    // Методы для вызова из анимаций (Animation Events)
    public void DealMeleeDamage()
    {
        // Реализация нанесения урона при ближней атаке
        if (Vector2.Distance(transform.position, player.position) <= meleeRange)
        {
            // player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
        }
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }

    // Визуализация для отладки
    private void OnDrawGizmosSelected()
    {
        if (patrolLeftBound && patrolRightBound)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(patrolLeftBound.position, patrolRightBound.position);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}