using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int MaxHealth;
    [SerializeField] Slider healthSlider;

    void Start()
    {
        SetHealth(100);
    }

    public void SetHealth(int health)
    {
        currentHealth = MaxHealth = health;
        UpdateHealthBar();
    }

    public IEnumerator TakeDamageDelayed(int amount, int delay)
    {
        yield return new WaitForSeconds(delay);
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        if (currentHealth == 0) { 
            SetHealth(100);
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthSlider.value = (float)currentHealth / (float) MaxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, MaxHealth);
        UpdateHealthBar();
    }
    

}
