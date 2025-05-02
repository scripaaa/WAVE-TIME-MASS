using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; // Скорость
    [SerializeField] private int health; // Хп
    [SerializeField] private float jumpForce = 15f; // Высота прыжка

    [SerializeField] private UnityEngine.Transform heartsContainer; // Контейнер для сердец
    [SerializeField] private GameObject heartPrefab; // Префаб сердца
    [SerializeField] private GameObject heartPrefab2; // Префаб сердца
    [SerializeField] private GameObject heartPrefab3; // Префаб сердца

    [SerializeField] private Image[] Hearts; // Сердечки в сцене
    [SerializeField] private Sprite Heart; // Отображение в сцене полных сердечек
    [SerializeField] private Sprite DeadHeart; // Отображение в сцене пустых сердечек

    [SerializeField] private Transform RangePoint;// Место откуда появляются все дальние атаки
    [SerializeField] private GameObject[] projectiles; // массив объектов дальней атаки
    [SerializeField] private float RangeCooldown; // кулдаун на дальнюю атаку, можно изменять в инспекторе
    


    private bool isGrounded = false; // Есть ли земля под ногами
    public float groundCheckRadius = 0.2f; // Радиус проверки нахождения на земле
    public Transform groundCheck; // Точка проверки нахождения на земле
    public LayerMask groundLayer; // Слой земли

    public int score; // Счет монеток
    public Text score_text; // Текст для счета монеток

    public bool IsRangeAttacking = false; //Используем ли дальнюю атаку
    public bool IsAttacking = false; // Атакуем ли мы сейчас
    public bool IsRecharged = true; // Перезарядка атаки
    public bool IsFalling = false; //Падаем ли мы сейчас
    public bool IsJumping = false; //В прыжке ли мы сейчас
    public bool FinishDone = false;

    public UnityEngine.Transform AttackPos; // позиция атаки
    public float attackRange; // Дальность атаки
    public float attackHeight; //Высота атаки
    public float knockbackForce = 6f; // Сила отталкивания при атаке
    public LayerMask enemy; // Слой врагов
    public GameObject deathMenu;

    private Rigidbody2D rb; 
    private SpriteRenderer sprite;
    private Animator anim;
    private int lives;
    private bool active_Damage_Jump = false; // блокируем возможность атаки прыжком
    private bool active_Melee_Attacking = false; // блокируем возможность атаки прыжком
    private bool active_Range_Attacking = false; // блокируем возможность атаки прыжком
    private float cooldownTimer = Mathf.Infinity; //таймер для проверки
    private Color originalColor;

    public static Hero Instance { get; set; }


    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }


    private void Awake()
    {
        lives = 1;
        health = lives;
        Instance = this;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        score_text.text = score.ToString();
        IsRecharged = true;
        originalColor = sprite.color;
    }

    private void Update()
    {
        // Проверяем, заморожено ли время (пауза)
        if (TimeManager.IsTimeFrozen() || FinishDone)
            return;

        CheckIfGrounded();

        if (Input.GetButtonDown("Fire1"))
            Attack();
        if (Input.GetButtonDown("Fire2") && cooldownTimer > RangeCooldown)
            RangeAttack();
        if (isGrounded && !IsAttacking && !IsRangeAttacking)
            State = States.idle;

        if ( Input.GetButton("Horizontal"))
            Run();

        if (( isGrounded && Input.GetButtonDown("Jump")) | ( !isGrounded && Input.GetButtonDown("Jump")))
            Jump();

        
        cooldownTimer += Time.deltaTime; // изменяем значение проверочного таймера

        if (IsFalling && !IsAttacking && !IsRangeAttacking)
            State = States.fall;
        if (IsJumping && !IsAttacking && !IsRangeAttacking) State = States.jump;

        // Смерть при падении с карты
        if (gameObject.transform.position.y < -20)
        {
            Die();
        }
        UpdateAnimationState();
        UpdateHearts();
    }

    // Обновление здоровья
    private void UpdateHearts()
    {
        if (health > lives)
            health = lives;

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < health)
                Hearts[i].sprite = Heart;
            else
                Hearts[i].sprite = DeadHeart;

            if (i < lives)
                Hearts[i].enabled = true;
            else
                Hearts[i].enabled = false;
        }
    }

    // // Проверка на соприкосновение с землей
    private void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            IsFalling = false;
            IsJumping = false;
        }
    }
    //Проверка на падание персонажа 
    private void UpdateAnimationState()
    {
        if (!IsAttacking)
        {
            if (IsJumping)
                State = States.jump;
            if (IsFalling)
                State = States.fall;
            if (rb.velocity.y < 0)
            {
                IsFalling = true;
                IsJumping = false;
            }
            else if (rb.velocity.y > 0)
            {
                IsJumping = true;
                IsFalling = false;
            }
        }
    }

    // Бег
    private void Run()
    {
        if (isGrounded && !IsAttacking && !IsRangeAttacking) State = States.run;
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        if (moveInput != 0)
        {

            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x) * Mathf.Sign(moveInput);
            transform.localScale = newScale;
        }
    }

    // Прыжок
    private void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            IsJumping = true;
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            if (!IsAttacking && !IsRangeAttacking)
                State = States.jump;
        }
    }
    

    private void Attack()
    {
        if (active_Melee_Attacking)
        {
            Debug.Log("Attack executed!");
            if (IsRecharged)
            {
                State = States.attack;
                IsAttacking = true;
                IsRecharged = false;

                StartCoroutine(AttackAnimation());
                StartCoroutine(AttackCoolDown());
            }
        }
    }
    private void RangeAttack()
    {
        if(active_Range_Attacking)
        {
            cooldownTimer = 0;
            State = States.rangeattack;
            IsRangeAttacking = true;
            StartCoroutine(RangeAttackCoolDown());

            projectiles[FindProjectile()].transform.position = RangePoint.position;
            projectiles[FindProjectile()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
        
    }
    //Возвращает нужные элемент дальней атаки
    private int FindProjectile()
    {
        
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void OnAttack()
    {
        Vector2 direction = transform.right;
        if (transform.localScale.x < 0) direction = -transform.right;
    
            Vector2 boxSize = new Vector2(attackRange, attackHeight);
        Vector2 boxCenter = (Vector2)AttackPos.position + (Vector2)(direction * (attackRange / 2));
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, enemy);

        Vector2 knockbackDirection = new Vector2(transform.localScale.x, 0);


        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Entity>().livess > 0)
            {
                colliders[i].GetComponent<Entity>().GetDamage();
                colliders[i].GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            }
            else return;
               
        }

       
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 direction = transform.right;
        if (transform.localScale.x < 0) direction = -transform.right;
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)AttackPos.position + (Vector2)(direction * (attackRange / 2));
        Gizmos.DrawWireCube(boxCenter, new Vector3(attackRange, attackHeight, 1f));
    }


    //Методы для изменения цвета спрайта
    private void ChangeColor(Color color)
    {
        sprite.color = color;
    }
    private IEnumerator ResetColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        sprite.color = originalColor;
    }



    private IEnumerator RangeAttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        IsRangeAttacking = false;
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.43f);
        IsAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.43f);
        IsRecharged = true;
    }

    // Убийство врага прыжком
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && active_Damage_Jump)
        {
            if (other.gameObject.GetComponent<Entity>().livess == 3)
            {
                //Destroy(other.gameObject);
                other.gameObject.GetComponent<Entity>().Die();
            }
            else
                other.gameObject.GetComponent<Entity>().GetDamage();

        }
    }

    // Получение урона
    public override void GetDamageHero()
    {
        health -= 1;
        ChangeColor(Color.red);
        StartCoroutine(ResetColorAfterDelay(1f));
        if (health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        // Проверяем существование CheckpointManager
        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint())
        {
            // Респавн у чекпоинта
            RespawnAtCheckpoint();
        }
        else
        {
            TimeManager.FreezeTime();
            // Перезагрузка сцены
            deathMenu.SetActive(true);
        }
    }

    public void RespawnAtCheckpoint()
    {
        if (!CheckpointManager.Instance.HasCheckpoint()) return;

        Vector2 checkpointPos = CheckpointManager.Instance.GetLastCheckpoint().Value;

        // Телепортация
        transform.position = checkpointPos;

        // Если это чекпоинт босса - перезапускаем бой
        if (CheckpointManager.Instance.IsLastCheckpointBoss())
        {
            var bossFightManager = FindObjectOfType<BossFightManager>();
            if (bossFightManager != null)
            {
                bossFightManager.ResetTrigger();
                StartCoroutine(bossFightManager.StartBossFight());
            }
        }

        // Восстановление параметров
        health = lives;
        UpdateHearts();
        rb.velocity = Vector2.zero;
        sprite.color = originalColor;
    }

    // Добавление монеты
    public void AddCoin()
    {
        score++;
        score_text.text = score.ToString();
    }

    // Активация двойного прыжка
    public void DoubleJump()
    {
        jumpForce += 7;
    }

    // Активация атаки прыжком
    public void Active_Damage_Jump()
    {
        active_Damage_Jump = true;
    }

    // Активация ближней атаки
    public void Active_Melee_Attacking()
    {
        active_Melee_Attacking = true;
    }

    // Активация дальней атаки
    public void Active_Range_Attacking()
    {
        active_Range_Attacking = true;
    }

    public int CntHeart()
    {
        return lives;
    }

    public void AddHeart()
    {
        // Увеличиваем максимальное здоровье
        lives += 1;
        health = lives;

        // Добавляем сердце в HUD
        GameObject newHeart;

        if (lives == 2)
            newHeart = Instantiate(heartPrefab, heartsContainer);

        else if (lives == 3)
            newHeart = Instantiate(heartPrefab2, heartsContainer);

        else
            newHeart = Instantiate(heartPrefab3, heartsContainer);

        newHeart.GetComponent<Image>().sprite = Heart;

        // Расширяем массив `Hearts`
        List<Image> heartsList = new List<Image>(Hearts);
        heartsList.Add(newHeart.GetComponent<Image>());
        Hearts = heartsList.ToArray();

        newHeart.SetActive(true); // Отображаем доп середечко

        Debug.Log("Новое сердце добавлено!");
    }
    public void FinishAnim()
    {
        FinishDone = true;
        State = States.finish;
    }
    public void FinishStatsMessage()
    {
        FinishDone = false;
        Finish finish = FindObjectOfType<Finish>();
        finish.FinishStats();
    }


}

public enum States
{
    idle,
    run,
    jump,
    attack,
    fall,
    rangeattack,
    finish
}