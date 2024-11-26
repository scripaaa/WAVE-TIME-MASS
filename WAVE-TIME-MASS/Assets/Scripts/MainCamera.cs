using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    private void Awake()
    {
        if (!player)
        {
            player = FindObjectOfType<Hero>().transform;
        }
    }
    private void Update()
    {
        pos = player.position;
        pos.z = -10f;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}*/

public class MainCamera : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public Vector2 mapMinBounds; // Минимальные координаты карты
    public Vector2 mapMaxBounds; // Максимальные координаты карты
    public float smoothSpeed = 0.125f; // Скорость плавности камеры

    private Camera cam; // Ссылка на компонент камеры
    private float camHeight; // Высота видимой области камеры
    private float camWidth;  // Ширина видимой области камеры

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateCameraSize();
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Получаем позицию игрока
        Vector3 targetPosition = player.position;

        // Ограничиваем движение камеры с учетом её размеров
        targetPosition.x = Mathf.Clamp(targetPosition.x, mapMinBounds.x + camWidth, mapMaxBounds.x - camWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, mapMinBounds.y + camHeight, mapMaxBounds.y - camHeight);

        // Сохраняем Z-координату камеры
        targetPosition.z = transform.position.z;

        // Плавное движение камеры
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }

    void UpdateCameraSize()
    {
        // Расчет размеров камеры в мире на основе ортографического размера
        camHeight = cam.orthographicSize;
        camWidth = cam.aspect * camHeight;
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация границ карты
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(mapMinBounds.x, mapMinBounds.y, 0), new Vector3(mapMinBounds.x, mapMaxBounds.y, 0));
        Gizmos.DrawLine(new Vector3(mapMaxBounds.x, mapMinBounds.y, 0), new Vector3(mapMaxBounds.x, mapMaxBounds.y, 0));
        Gizmos.DrawLine(new Vector3(mapMinBounds.x, mapMinBounds.y, 0), new Vector3(mapMaxBounds.x, mapMinBounds.y, 0));
        Gizmos.DrawLine(new Vector3(mapMinBounds.x, mapMaxBounds.y, 0), new Vector3(mapMaxBounds.x, mapMaxBounds.y, 0));
    }
}
