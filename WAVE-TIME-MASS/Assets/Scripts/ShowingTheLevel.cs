using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowingTheLevel : MonoBehaviour
{
    public Transform[] cameraPoints; // ������ �����
    public float moveSpeed = 5f; // ��������
    public bool Showing = true;
    public GameObject Skip;

    private MonoBehaviour mainCameraScript; // ������ �� ������ MainCamera
    private KeyCode skipKey = KeyCode.C; // ������� ������ �� C
    private Coroutine CameraMovement;

    private bool levelAlreadyShown = false;

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
        Skip.SetActive(true);
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
            Skip.SetActive(false);
        }

        levelAlreadyShown = true;

        // ������� ArtifactSelector � ���������� ����� ����������
        ArtifactSelector artifactSelector = FindObjectOfType<ArtifactSelector>();
        if (artifactSelector != null)
        {
            artifactSelector.ShowArtifactSelection();
        }
        else
        {
            TimeManager.UnfreezeTime(); // ������������� ����, ���� ��� ���������� ��� ������
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
        if (CameraMovement != null && !levelAlreadyShown)
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
                Skip.SetActive(false);
            }

            // ������� ArtifactSelector � ���������� ����� ����������
            ArtifactSelector artifactSelector = FindObjectOfType<ArtifactSelector>();
            if (artifactSelector != null)
            {
                artifactSelector.ShowArtifactSelection();
            }
            else
            {
                TimeManager.UnfreezeTime(); // ������������� ����, ���� ��� ���������� ��� ������
            }
        }
    }
}