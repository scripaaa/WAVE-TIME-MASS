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
    [SerializeField] private float speed = 3f;
    [SerializeField] private int health; 
    [SerializeField] private float jumpForce = 15f; 
    [SerializeField] private Image[] Hearts;
    [SerializeField] private Sprite Heart;
    [SerializeField] private Sprite DeadHeart;
    private bool isGrounded = false;



    public int score; 
    public Text score_text; 

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
        lives = 5;
        health = lives;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        score_text.text = score.ToString();
        Instance = this;
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

    private void Run()
    {
        if (isGrounded) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f; 
    }

    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        if (!isGrounded) State = States.jump;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }

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

    public void AddCoin()
    {
        score++;
        score_text.text = score.ToString();
    }

    //Применить артефакт к игроку; 
    public void AddArtifact(Artifact artifact) { artifact.Use(); }

    //значение поля jumpForce
    public float GetJumpForce() { return jumpForce; }
    //изменить значение поля jumpForce
    public void SetJumpForce(float newjumpforce) { jumpForce = newjumpforce; }
    //значение поля lives
    public int GetLives() { return lives; }
    //изменить значение поля Live
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