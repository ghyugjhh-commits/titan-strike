using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxArmor = 50f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float currentArmor = 50f;

    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent<float> OnDamageTaken = new UnityEvent<float>();

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentArmor => currentArmor;
    public float MaxArmor => maxArmor;

    public void Damage(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        float absorb = Mathf.Min(currentArmor, amount);
        currentArmor -= absorb;
        currentHealth -= (amount - absorb);

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            OnDeath.Invoke();
        }

        OnDamageTaken.Invoke(amount);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void RestoreArmor(float amount)
    {
        currentArmor = Mathf.Min(maxArmor, currentArmor + amount);
    }
}
