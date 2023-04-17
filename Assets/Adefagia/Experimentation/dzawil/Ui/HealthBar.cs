using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float damage;
    [SerializeField] private Gradient colorGradient;

    public Image healthBarImage;

    public void UpdateHealthBar(float health) 
    {
        healthBarImage.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
        healthBarImage.color = colorGradient.Evaluate(1 - healthBarImage.fillAmount);
    }

}
