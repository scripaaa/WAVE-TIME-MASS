using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private bool isBossCheckpoint = false; // Новое поле
    private Animator anim;

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    public bool IsBossCheckpoint => isBossCheckpoint; // Свойство для доступа

    private void Awake()
    {
        anim = GetComponent<Animator>();
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