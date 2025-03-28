using UnityEngine;
using UnityEngine.UI;

public class BossHealth : Entity  // Наследуемся от Entity
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 10;  // Максимальное здоровье
    [SerializeField] private Slider _healthBar;    // Слайдер для отображения здоровья

    public GameObject leftDoor;  // Левая дверь
    public GameObject rightDoor; // Правая дверь

    private void Start()
    {
        livess = _maxHealth;  // Инициализируем livess из родительского класса
        UpdateHealthBar();
    }

    // Переопределяем метод GetDamage
    public override void GetDamage()
    {
        base.GetDamage();  // Вызываем родительский метод (уменьшает livess)
        UpdateHealthBar();

        if (livess <= 0)
        {
            Die();  // Вызовет родительский Die() или переопределенный (см. ниже)
        }
    }

    // Переопределяем метод Die (опционально)
    public override void Die()
    {
        Debug.Log("Босс побежден!");
        // Дополнительные действия: анимация смерти, награда и т.д.
        base.Die();  // Уничтожаем объект (родительский метод)

        // Открываем двери
        if (leftDoor != null) leftDoor.SetActive(false);
        if (rightDoor != null) rightDoor.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        if (_healthBar != null)
        {
            _healthBar.value = livess;
        }
    }
}