using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject[] platforms; // Массив платформ, которые будут раздвигаться
    public float moveDistance = 2f; // Расстояние, на которое раздвигаются платформы
    public float moveSpeed = 2f; // Скорость раздвижения
    private Animator anim;

    private bool isPressed = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            MovePlatforms();
            anim.SetTrigger("Press");
        }
    }

    private void MovePlatforms()
    {
        foreach (var platform in platforms)
        {
            StartCoroutine(MovePlatform(platform));
        }
    }

    private System.Collections.IEnumerator MovePlatform(GameObject platform)
    {
        Vector3 startPosition = platform.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, moveDistance, 0);

        float elapsedTime = 0;

        while (elapsedTime < moveSpeed)
        {
            platform.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platform.transform.position = endPosition;
    }
}