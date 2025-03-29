using UnityEngine;
using UnityEngine.UI;

public class EnemyShooter : Entity
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public GameObject healthBarPrefab; // Префаб хелсбара (перетащите в инспекторе)

    [Header("Debug")]
    public bool showAttackRange = true;
    public Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);

    private float lastAttackTime;
    private SpriteRenderer enemySprite;
    private Animator anim;
    private Transform player;
    private bool isFacingRight = true;
    private Slider healthSlider;
    private GameObject healthBarInstance;

    void Start()
    {
        livess = 3;

        enemySprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;

        InitializeHealthBar();
    }

    void InitializeHealthBar()
    {
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform);
            healthBarInstance.transform.localPosition = healthBarOffset;
            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();

            // Настройка хелсбара
            if (healthSlider != null)
            {
                healthSlider.minValue = 0;
                healthSlider.maxValue = livess;
                healthSlider.value = livess;
            }

            // Автоповорот к камере
            healthBarInstance.AddComponent<HealthBarFaceCamera>();
        }
    }

    void Update()
    {
        if (player == null) return;

        UpdateFacingDirection();
        UpdateHealthBar();

        if (PlayerInRange() && Time.time - lastAttackTime >= attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time;
        }
    }

    void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = livess;
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

        anim.SetTrigger("Attack");
    }

    // Вызывается из анимации атаки
    void OnShoot()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        projectile.GetComponent<EnemyProjectile>().SetDirection(isFacingRight);
    }

    public override void GetDamage()
    {
        base.GetDamage();
        UpdateHealthBar();

        if (livess <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
        base.Die();
    }

    void OnDrawGizmosSelected()
    {
        if (!showAttackRange || firePoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 direction = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(firePoint.position, firePoint.position + direction * 2f);
    }
}

// Скрипт для поворота хелсбара к камере
public class HealthBarFaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}