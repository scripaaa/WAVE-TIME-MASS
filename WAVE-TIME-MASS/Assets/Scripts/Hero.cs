using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int lives = 5; // количество жизней
    [SerializeField] private float jumpForce = 15f; // сила прыжка
    private bool isGrounded = false; // есть ли замля под ногами

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    //Получаем ссылки на rb и sprite
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }

    //Бег
    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal"); //Направление юнита

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f; //Поворот при смене направления
    }

    //Прыжок
    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }
}
