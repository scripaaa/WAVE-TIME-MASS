using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowingTheLevel : MonoBehaviour
{
    public Transform[] cameraPoints; // ������ �����
    public float moveSpeed = 5f; // ��������
    public bool Showing = true;

    private MonoBehaviour mainCameraScript; // ������ �� ������ MainCamera
    private KeyCode skipKey = KeyCode.C; // ������� ������ �� C
    private Coroutine CameraMovement;


    void Start()
    {
        if ( Showing == false)
        {
            mainCameraScript.enabled = true;
            enabled = false;
        }
        TimeManager.FreezeTime();
        // ������� ������ MainCamera �� ������
        mainCameraScript = GetComponent<MainCamera>();

        if (mainCameraScript != null)
        {
            mainCameraScript.enabled = false; // ��������� ������ MainCamera �� ����� �������� ������
        }

        if (cameraPoints.Length > 0)
        {
            // �������� �������� ������ �� ������
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, point.rotation, moveSpeed * 50 * Time.unscaledDeltaTime); // 50 - �������� ��������
                yield return null;
            }
        }

        // �������� ������ MainCamera
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
            // �������� ������ MainCamera
            Showing = false;
            if (mainCameraScript != null)
            {
                mainCameraScript.enabled = true;
                TimeManager.ResetFreezeCount();
            }
        }
    }
}