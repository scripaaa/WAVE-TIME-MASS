using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // ñêîðîñòü äâèæåíèÿ
    [SerializeField] private int lives = 5; // êîëè÷åñòâî æèçíåé
    [SerializeField] private float jumpForce = 15f; // ñèëà ïðûæêà
    private bool isGrounded = false; // åñòü ëè çàìëÿ ïîä íîãàìè

    public int score; // êîëëè÷åñòâî ìîíåò
    public Text score_text; // òåêñò, âûâîäÿùèé êîëëè÷åñòâî ìîíåò

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }


    //Ïîëó÷àåì ññûëêè íà rb è sprite
    private void Awake()
    {
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
    }

    //Áåã
    private void Run()
    {
        if (isGrounded) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal"); //Íàïðàâëåíèå þíèòà

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f; //Ïîâîðîò ïðè ñìåíå íàïðàâëåíèÿ
    }

    //Ïðûæîê
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

    //Äîáàâëÿåò ìîíåòêó
    public void AddCoin()
    {
        score++;
        score_text.text = score.ToString();
    }
}

public enum States
{
    idle,
    run,
    jump
}