using UnityEngine;
using UnityEngine.UI;

public class PlayerHealt : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth = 10;
    public Image healthBar;

    private void Start()
    {
        Updatebar();
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        Updatebar();

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Updatebar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
