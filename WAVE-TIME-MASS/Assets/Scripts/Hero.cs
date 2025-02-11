using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEditor.Experimental;
using Unity.VisualScripting;

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

    public int score; // Счет монеток
    public Text score_text; // Текст для счета монеток

    public bool IsAttacking = false; // Атакуем ли мы сейчас
    public bool IsRecharged = true; // Перезарядка атаки

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
        if (isGrounded && !IsAttacking)
            State = States.idle;

        if (!IsAttacking && Input.GetButton("Horizontal"))
            Run();

        if ((!IsAttacking && isGrounded && Input.GetButtonDown("Jump")) | (!IsAttacking && !isGrounded))
            Jump();
        if (Input.GetButtonDown("Fire1"))
            Attack();
        




        // Смерть при падении с карты
        if (gameObject.transform.position.y < -20)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

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

    // Бег
    private void Run()
    {
        if (isGrounded) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f; 
    }

    // Прыжок
    private void Jump()
    {
        if (!isGrounded)
        {
            State = States.jump; 
        }
        else
        {
            isGrounded = false;
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            if (!isGrounded) State = States.jump;
        }
    }

    // Проверка на соприкосновение с землей
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;

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
        yield return new WaitForSeconds(0.2f);
        IsAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.1f);
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
    attack
}