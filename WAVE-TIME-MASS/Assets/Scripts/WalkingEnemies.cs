using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WalkingEnemies : Entity
{
    public float speed;
    public Vector3[] positions;
    public GameObject healthBarPrefab; // Префаб хелсбара (перетащите в инспекторе)
    public Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);

    private Slider healthSlider;
    private GameObject healthBarInstance;

    private Vector3 target;
    private int currentTarget;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        livess = 3;

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

    void Flip() // Поворот врага при смене направления
    {
        var target = transform.position;
        if (target.x == positions[1].x)
        {
            sprite.flipX = true;
        }
        if (target.x == positions[0].x)
        {
            sprite.flipX = false;
        }
    }

    public void FixedUpdate()
    {

        transform.position = Vector3.MoveTowards(transform.position, positions[currentTarget], speed);


        if (transform.position == positions[currentTarget])
        {

            if (currentTarget < positions.Length - 1)
            {
                currentTarget++;

            }
            else
            {
                currentTarget = 0;
            }
        }
        Flip();

        // Смерть при падении с карты
        if (gameObject.transform.position.y < -20)
        {
            Destroy(gameObject);
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = livess;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamageHero();
            if (livess < 1)
                Die();
        }
        
    }
}
