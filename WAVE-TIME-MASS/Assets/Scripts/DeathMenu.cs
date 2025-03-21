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

        deathMenu.SetActive(false); // ������ ���� ������
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ������������� �����
    }

    public void MainMenu()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // ����������� ����� ��� ������
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(0); // ��������� ������� ����
    }

    public void Exit()
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // ����������� ����� ��� ������
        Application.Quit();
    }
}