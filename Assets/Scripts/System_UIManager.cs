using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class System_UIManager : MonoBehaviour
{
    [SerializeField]
    private float playerMaxHealth;

    [SerializeField]
    private float playerMaxStamina;

    [SerializeField]
    private float playerMaxSpecial;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private Slider staminaSlider;

    [SerializeField]
    private Slider specialSlider;

    private void Start()
    {
        healthSlider.maxValue = playerMaxHealth;
        healthSlider.value = playerMaxHealth;
        staminaSlider.maxValue = playerMaxStamina;
        staminaSlider.value = playerMaxStamina;
        specialSlider.maxValue = playerMaxSpecial;
        specialSlider.value = 0f;
    }

    public void UpdateHealthBarUI(Component sender, object data)
    {
        healthSlider.value = (float) data;
    }

    public void UpdateStaminaBarUI(Component sender, object data)
    {
        staminaSlider.value = (float) data;
    }

    public void UpdateSpecialBarUI(Component sender, object data)
    {
        specialSlider.value = (float) data;
    }
}
