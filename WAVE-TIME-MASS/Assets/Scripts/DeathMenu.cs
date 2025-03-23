using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenu;
    public Image fadeImage; // ������ �� ����

    private float fadeDuration = 0.5f; // ������������
    private Coroutine fadeCoroutine; // ������ �� ��������
    private Slowdown slowdown; // ������ �� Slowdown

    void Start()
    {
        slowdown = GameObject.FindGameObjectWithTag("Player").GetComponent<Slowdown>(); // ������� ������ Slowdown �� ������� ������
    }

    private void Update()
    {
        if (deathMenu.activeSelf)
        {
            if (fadeCoroutine == null)
            {
                TimeManager.FreezeTime();
                fadeCoroutine = StartCoroutine(FadeToBlack());
            }
        }
    }

    // ����������
    IEnumerator FadeToBlack()
    {
        float counter = 0;
        Color spriteColor = fadeImage.color;

        while (counter < fadeDuration)
        {
            counter += Time.unscaledDeltaTime;
            fadeImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Mathf.Clamp01(counter / fadeDuration));
            yield return null;
        }
    }

    public void Restart()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // ������� ��������� �������� �� ���������
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RemoveArtifact(SceneManager.GetActiveScene().buildIndex - 2);
        }

        deathMenu.SetActive(false); // ������ ���� ������
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ������������� �����

        slowdown.NotActivateSlowdown();
    }

    public void MainMenu()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // ������� ��������� ����� ���������
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // ����������� ����� ��� ������
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(0); // ��������� ������� ����

        slowdown.NotActivateSlowdown();
    }

    public void Exit()
    {
        // ������� ��������� ����� ���������
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // ����������� ����� ��� ������
        Application.Quit();

        slowdown.NotActivateSlowdown();
    }
}