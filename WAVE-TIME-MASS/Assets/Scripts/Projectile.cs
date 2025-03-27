using UnityEngine;

public class Projectile : MonoBehaviour
{
   [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private BoxCollider2D boxCollider;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit) return;
        float movementspeed = speed * Time.deltaTime * direction;
        transform.Translate(movementspeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 3)
            gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Entity>().GetDamage();
        }
    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
    

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y,transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);

    }

}
