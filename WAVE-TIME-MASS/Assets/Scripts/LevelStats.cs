using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelStats : MonoBehaviour
{
    public int totalCoins; // ����� ���������� ����� �� ������
    public int collectedCoins = 0; // ��������� ������
    public float levelTime = 0f; // ����� ����������� ������
    public float targetTime; // ������� ����� ��� ��������� 2 ������

    public GameObject statsPanel; // ������ ����������
    public Text coinsText; // ����� ��� ����������� �����
    public Text timeText; // ����� ��� ����������� �������
    public Image[] stars; // ������ ����������� �����
    public Text timerText; // ��������� ������� ��� ����������� �������
    public GameObject panel; // ��������� �����

    private bool levelCompleted = false;

    void Update()
    {
        if (!levelCompleted)
        {
            levelTime += Time.deltaTime; // ��������� �����
            UpdateTimerUI(); // ��������� ��������� ������� �������
        }
    }

    public void CollectCoin()
    {
        collectedCoins++;
    }

    public void CompleteLevel()
    {
        levelCompleted = true;
        ShowStats();
    }

    void ShowStats()
    {
        for (int i = 0; i < 3; i++)
        {
            if (stars[i] != null)
            {
                stars[i].color = Color.gray; 
            }
        }

        if (!statsPanel.activeSelf) // ���������, ��� ������ ��� �� �������
        {
            statsPanel.SetActive(true); // ���������� ������ ����������

            // ���������� ���������� ��������� �����
            if (coinsText != null)
            {
                coinsText.text = collectedCoins + " / " + totalCoins;
            }
            else
            {
                Debug.LogError("coinsText �� �������� � ����������!");
            }

            // ���������� ����� ����������� ������
            if (timeText != null)
            {
                timeText.text = levelTime.ToString("F1") + "s";
            }
            else
            {
                Debug.LogError("timeText �� �������� � ����������!");
            }

            // ������������ ���������� �����
            int starsEarned = 0;

            // 1 ������ �� ����������� ������
            starsEarned++;

            // 2 ������ �� ��������� �����
            if (levelTime <= targetTime)
            {
                starsEarned++;
            }

            // 3 ������ �� ��� ��������� ������
            if (collectedCoins >= totalCoins)
            {
                starsEarned++;
            }

            // ���������� ������
            for (int i = 0; i < starsEarned; i++)
            {
                if (stars[i] != null)
                {
                    stars[i].color = Color.yellow; // ������������ ������
                }
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // ���������� ����� � �������� � ����� ���������� ������
            timerText.text = levelTime.ToString("F1") + "�";
        }
        else
        {
            Debug.LogError("timerText �� �������� � ����������!");
        }
    }

    public void RestartLevel()
    {
        // ������� ��������� �������� �� ���������
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RemoveArtifact(SceneManager.GetActiveScene().buildIndex - 2);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        TimeManager.UnfreezeTime();
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            statsPanel.SetActive(false);
            panel.SetActive(true);
            TimeManager.FreezeTime(); 
        }
        else
        {
            TimeManager.UnfreezeTime();
            TimeManager.ResetFreezeCount(); // �������� ��������� �������
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}