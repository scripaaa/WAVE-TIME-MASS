using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossUI : MonoBehaviour
{
    [Header("References")]
    public Boss3 boss; // ������ �� ������ �����
    public Slider healthSlider;
    public Slider cooldownSlider;
    public Canvas canvas;
    public Vector3 offset = new Vector3(0, 2f, 0);

    [Header("Settings")]
    public Color healthColor = Color.red;
    public Color cooldownColor = Color.blue;
    public float flashDuration = 0.1f;

    private Image healthFill;
    private Image cooldownFill;
    private Color originalHealthColor;

    void Start()
    {
        // ��������� Canvas
        canvas.renderMode = RenderMode.WorldSpace;

        if (canvas.worldCamera == null)
            canvas.worldCamera = Camera.main;

        canvas.transform.SetParent(boss.transform);
        canvas.transform.localPosition = offset;
        canvas.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        // ������������� ���������
        healthSlider.minValue = 0;
        healthSlider.maxValue = boss.maxHealth;
        healthSlider.value = healthSlider.maxValue; // ������ ��������

        cooldownSlider.minValue = 0;
        cooldownSlider.maxValue = 1;
        cooldownSlider.value = cooldownSlider.maxValue; // ������ �����������
        cooldownSlider.gameObject.SetActive(false);

        // �������� ����������
        healthFill = healthSlider.fillRect.GetComponent<Image>();
        cooldownFill = cooldownSlider.fillRect.GetComponent<Image>();
        originalHealthColor = healthFill.color;

        // ������������� �� �������
        boss.OnHealthChanged += UpdateHealth;
        boss.OnCooldownChanged += UpdateCooldown;
    }

    void Update()
    {
        // �������������� �������� ���������
        if (!healthSlider.gameObject.activeSelf)
            healthSlider.gameObject.SetActive(true);

        if (!cooldownSlider.gameObject.activeSelf)
            cooldownSlider.gameObject.SetActive(true);

        // ������������ � ����� (�� ������ ���� Canvas "������")
        transform.position = boss.transform.position + offset;
    }

    void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
        StartCoroutine(FlashHealth());
    }

    // ������������ ����� ��� �����������
    public void UpdateCooldown(float progress)
    {
        // ������ ���������� ������, �� � ������ �������������
        cooldownSlider.gameObject.SetActive(true);

        if (progress >= 1f)
        {
            // ������� ���� ��� ����������
            cooldownSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
        else
        {
            // ����� ���� ��� �����������
            cooldownSlider.fillRect.GetComponent<Image>().color = Color.blue;
        }

        cooldownSlider.value = progress;
    }

    IEnumerator FlashHealth()
    {
        healthFill.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        healthFill.color = originalHealthColor;
    }

    void OnDestroy()
    {
        // ������������ �� �������
        boss.OnHealthChanged -= UpdateHealth;
        boss.OnCooldownChanged -= UpdateCooldown;
    }

    public void SetImmuneVisuals(bool isImmune)
    {
        Image healthFill = healthSlider.fillRect.GetComponent<Image>();
        Image cooldownFill = cooldownSlider.fillRect.GetComponent<Image>();

        if (isImmune)
        {
            healthFill.color = Color.gray;
            cooldownSlider.gameObject.SetActive(false);
        }
        else
        {
            healthFill.color = Color.red;
        }
    }

    public void SetHealthColor(Color newColor)
    {
        if (healthSlider == null)
        {
            Debug.LogError("Health Slider reference is missing!");
            return;
        }

        if (healthSlider.fillRect == null)
        {
            Debug.LogError("Fill Rect is missing on Health Slider!");
            return;
        }

        Image fillImage = healthSlider.fillRect.GetComponent<Image>();
        if (fillImage == null)
        {
            Debug.LogError("No Image component on Fill Rect!");
            return;
        }

        fillImage.color = newColor;
        Debug.Log($"Color changed to: {newColor}"); // ������������� ���������
    }
}

