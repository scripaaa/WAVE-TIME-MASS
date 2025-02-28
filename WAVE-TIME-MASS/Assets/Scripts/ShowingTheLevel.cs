using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowingTheLevel : MonoBehaviour
{
    public Transform[] cameraPoints; // Список точек
    public float moveSpeed = 5f; // Скорость
    public bool Showing = true;

    private MonoBehaviour mainCameraScript; // Ссылка на скрипт MainCamera


    void Start()
    {
        if ( Showing == false)
        {
            mainCameraScript.enabled = true;
            enabled = false;
        }
        TimeManager.FreezeTime();
        // Находим скрипт MainCamera на камере
        mainCameraScript = GetComponent<MainCamera>();

        if (mainCameraScript != null)
        {
            mainCameraScript.enabled = false; // Отключаем скрипт MainCamera на время движения камеры
        }

        if (cameraPoints.Length > 0)
        {
            // Начинаем движение камеры по точкам
            StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        foreach (var point in cameraPoints)
        {
            while (Vector3.Distance(transform.position, point.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, moveSpeed * Time.unscaledDeltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, point.rotation, moveSpeed * 50 * Time.unscaledDeltaTime); // 50 - скорость вращения
                yield return null;
            }
        }

        // Включаем скрипт MainCamera
        if (mainCameraScript != null)
        {
            mainCameraScript.enabled = true;
            TimeManager.UnfreezeTime();
        }
    }
}