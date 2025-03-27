using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private Transform patrolLeftBound;
    [SerializeField] private Transform patrolRightBound;
    [SerializeField] private Transform attackTerritory; // ������ � ����������� ���������� �����
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
        // �������� ������
        //animator.SetBool("IsWalking", true);

        float moveDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);

        // �������� ���������� ������ ��������������
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

        // �������� ������
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
            // �������� ��������� ������ (���������� ����������)
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, playerLayer);
            isChasing = hit.collider != null && hit.collider.CompareTag("Player");
        }
    }

    private void HandleAttacks()
    {
        if (isInTerritory && Vector2.Distance(transform.position, player.position) <= meleeRange)
        {
            // ������� �����
            //animator.SetTrigger("MeleeAttack");
            // ����� ������ ���� ������ ��������� ����� ������
        }
        else if (!isInTerritory && Time.time > lastRangedAttackTime + rangedAttackCooldown)
        {
            // ������� �����
            //animator.SetTrigger("RangedAttack");
            lastRangedAttackTime = Time.time;
            // �������� �������
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

        // �������������� ��������
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
        // �������� ������
        //animator.SetTrigger("Die");

        // ���������� ����������� � ������
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;

        // ���������� �������
        enabled = false;

        // ����� ����� �������� ������ ����������� ������� ����� �������� ������
        // Destroy(gameObject, 2f);
    }

    // ������ ��� ������ �� �������� (Animation Events)
    public void DealMeleeDamage()
    {
        // ���������� ��������� ����� ��� ������� �����
        if (Vector2.Distance(transform.position, player.position) <= meleeRange)
        {
            // player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
        }
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }

    // ������������ ��� �������
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