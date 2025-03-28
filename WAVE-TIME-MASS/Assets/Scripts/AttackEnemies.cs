using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRange = 5f;
    public float attackCooldown = 2f;

    [Header("Debug")]
    public bool showAttackRange = true;

    private float lastAttackTime;
    private SpriteRenderer enemySprite;
    private Transform player;
    private bool isFacingRight = true; // Добавляем явное отслеживание направления

    void Start()
    {
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;

        // Инициализируем начальное направление
        isFacingRight = !enemySprite.flipX;
    }

    void Update()
    {
        if (player == null) return;

        // Обновляем направление взгляда
        UpdateFacingDirection();

        if (PlayerInRange() && Time.time - lastAttackTime >= attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time;
        }
    }

    void UpdateFacingDirection()
    {
        if (player.position.x > transform.position.x)
        {
            isFacingRight = true;
            enemySprite.flipX = false;
        }
        else
        {
            isFacingRight = false;
            enemySprite.flipX = true;
        }
    }

    bool PlayerInRange()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= attackRange;
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        // Передаем направление в скрипт снаряда
        EnemyProjectile projScript = projectile.GetComponent<EnemyProjectile>();
        projScript.SetDirection(isFacingRight);
    }

    void OnDrawGizmosSelected()
    {
        if (!showAttackRange || firePoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Рисуем направление атаки
        Vector3 direction = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(firePoint.position, firePoint.position + direction * 2f);
    }
}