using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEditor.Experimental;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; // Скорость
    [SerializeField] private int health; // Хп
    [SerializeField] private float jumpForce = 15f; // Высота прыжка

    [SerializeField] private Image[] Hearts; // Сердечки в сцене
    [SerializeField] private Sprite Heart; // Отображение в сцене полных сердечек
    [SerializeField] private Sprite DeadHeart; // Отображение в сцене пустых сердечек

    private bool isGrounded = false; // Есть ли земля под ногами

    public int score; // Счет монеток
    public Text score_text; // Текст для счета монеток

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private int lives;

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
    }

    private void Update()
    {
        if (isGrounded) State = States.idle;

        if (Input.GetButton("Horizontal"))
            Run();

        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();

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
        isGrounded = false;
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        if (!isGrounded) State = States.jump;
    }

    // Проверка на соприкосновение с землей
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;

    }

    // Убийство врага
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }

    // Получение урона
    public override void GetDamage()
    {
        health -= 1;
        if (health == 0)
        {
            foreach (var h in Hearts)
                h.sprite = DeadHeart;
            Die();
        }
    }

    // Добавление монеты
    public void AddCoin()
    {
        score++;
        score_text.text = score.ToString();
    }

    // Применить артефакт к игроку; 
    public void AddArtifact(Artifact artifact) { artifact.Use(); }

    // Значение поля jumpForce
    public float GetJumpForce() { return jumpForce; }
    // Изменить значение поля jumpForce
    public void SetJumpForce(float newjumpforce) { jumpForce = newjumpforce; }
    // Значение поля lives
    public int GetLives() { return lives; }
    // Изменить значение поля Live
    public void SetLives(int newlives)
    {
        lives = newlives;
    }
}

public enum States
{
    idle,
    run,
    jump
}