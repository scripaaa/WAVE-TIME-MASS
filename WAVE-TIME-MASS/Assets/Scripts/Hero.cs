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

    public UnityEngine.Transform AttackPos; // позиция атаки
    public float attackRange; // Дальность атаки
    public LayerMask enemy; // Слой врагов
    public GameObject deathMenu;

    private Rigidbody2D rb; 
    private SpriteRenderer sprite;
    private Animator anim;
    private int lives;
    private bool active_Damage_Jump = false; // блокируем возможность атаки прыжком
    private bool active_Melee_Attacking = false; // блокируем возможность атаки прыжком
    private float cooldownTimer = Mathf.Infinity; //таймер для проверки

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
    }

    private void Update()
    {
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
            gameObject.GetComponent<DeathMenu>().enabled = true;
            deathMenu.SetActive(true);
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
        cooldownTimer = 0;
        State = States.rangeattack;
        IsRangeAttacking = true;
        StartCoroutine(RangeAttackCoolDown());

        projectiles[FindProjectile()].transform.position = RangePoint.position;
            projectiles[FindProjectile()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackPos.position, attackRange, enemy);


        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }

       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, attackRange);
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
            Destroy(other.gameObject);
        }
    }

    // Получение урона
    public override void GetDamageHero()
    {
        health -= 1;
        if (health == 0)
        {
            foreach (var h in Hearts)
                h.sprite = DeadHeart;
            gameObject.GetComponent<DeathMenu>().enabled = true;
            deathMenu.SetActive(true);
        }
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

    // Активация атаки прыжком
    public void Active_Melee_Attacking()
    {
        active_Melee_Attacking = true;
    }

    public void AddHeart()
    {
        // Увеличиваем максимальное здоровье
        lives += 1;
        health = lives;

        // Добавляем сердце в HUD
        GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
        newHeart.GetComponent<Image>().sprite = Heart;

        // Расширяем массив `Hearts`
        List<Image> heartsList = new List<Image>(Hearts);
        heartsList.Add(newHeart.GetComponent<Image>());
        Hearts = heartsList.ToArray();

        newHeart.SetActive(true); // Отображаем доп середечко

        Debug.Log("Новое сердце добавлено!");
    }


}

public enum States
{
    idle,
    run,
    jump,
    attack,
    fall,
    rangeattack
}