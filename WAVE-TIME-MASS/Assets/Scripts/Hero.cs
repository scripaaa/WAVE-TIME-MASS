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

    private bool isGrounded = false; // Есть ли земля под ногами
    public float groundCheckRadius = 0.2f; // Радиус проверки нахождения на земле
    public Transform groundCheck; // Точка проверки нахождения на земле
    public LayerMask groundLayer; // Слой земли

    public int score; // Счет монеток
    public Text score_text; // Текст для счета монеток

    public bool IsAttacking = false; // Атакуем ли мы сейчас
    public bool IsRecharged = true; // Перезарядка атаки
    public bool IsFalling = false; //Падаем ли мы сейчас
    public bool IsJumping = false; //В прыжке ли мы сейчас

    public UnityEngine.Transform AttackPos; // позиция атаки
    public float attackRange; // Дальность атаки
    public LayerMask enemy; // Слой врагов

    private Rigidbody2D rb; 
    private SpriteRenderer sprite;
    private Animator anim;
    private int lives;
    private bool active_Damage_Jump = false; // блокируем возможность атаки прыжком

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

        if (isGrounded && !IsAttacking)
            State = States.idle;

        if ( Input.GetButton("Horizontal"))
            Run();

        if (( isGrounded && Input.GetButtonDown("Jump")) | ( !isGrounded && Input.GetButtonDown("Jump")))
            Jump();

        if (Input.GetButtonDown("Fire1"))
            Attack();
        if (IsFalling && !IsAttacking)
            State = States.fall;
        if (IsJumping && !IsAttacking) State = States.jump;

        // Смерть при падении с карты
        if (gameObject.transform.position.y < -20)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (isGrounded && !IsAttacking) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f; 
    }

    // Прыжок
    private void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            IsJumping = true;
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            if (!IsAttacking)
                State = States.jump;
        }
    }
    

    private void Attack()
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


    // Убийство врага
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }

    // Добавление монеты
    public void AddCoin()
    {
        score++;
        score_text.text = score.ToString();
    }


    // Выбор артефакта
    public void ApplyArtifactEffects()
    {
        if (PlayerPrefs.GetInt("DoubleJump", 0) == 1)
        {
            Debug.Log("DoubleJump активирован!");
            jumpForce += 7;
        }
        else if (PlayerPrefs.GetInt("DoubleHP", 0) == 1)
        {
            Debug.Log("DoubleHP активирован!");
            AddHeart();

        }
        else if (PlayerPrefs.GetInt("JumpAttack", 0) == 1)
        {
            Debug.Log("JumpAttack активирован!");
            active_Damage_Jump = true; // активируем возможность атаковать прыжком
        }
    }

    private void AddHeart()
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
    fall
}