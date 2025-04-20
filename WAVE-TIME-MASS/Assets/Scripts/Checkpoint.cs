using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private bool isBossCheckpoint = false; // Новое поле

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    public bool IsBossCheckpoint => isBossCheckpoint; // Свойство для доступа

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = inactiveSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActive)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        isActive = true;
        spriteRenderer.sprite = activeSprite;
        CheckpointManager.Instance.SetCheckpoint(transform.position);

        // Сохраняем, является ли это чекпоинтом босса
        if (isBossCheckpoint)
        {
            CheckpointManager.Instance.SetBossCheckpoint(this);
        }

        if (TryGetComponent(out Animator anim))
            anim.SetTrigger("Activate");
    }
}