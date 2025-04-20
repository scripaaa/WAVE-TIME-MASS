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
    private KeyCode skipKey = KeyCode.C; // Пропуск показа на C
    private Coroutine CameraMovement;


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
            CameraMovement = StartCoroutine(FollowPath());
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
            TimeManager.ResetFreezeCount();
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(skipKey)) && (Showing))
        {
            SkipShow();
        }
    }

    public void SkipShow()
    {
        if (CameraMovement != null)
        {
            StopCoroutine(CameraMovement);
            if (cameraPoints.Length > 0)
            {
                Transform lastPoint = cameraPoints[cameraPoints.Length - 1];
                transform.position = lastPoint.position;
                transform.rotation = lastPoint.rotation;
            }
            // Включаем скрипт MainCamera
            Showing = false;
            if (mainCameraScript != null)
            {
                mainCameraScript.enabled = true;
                TimeManager.ResetFreezeCount();
            }
        }
    }
}