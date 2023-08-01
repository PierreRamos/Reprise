using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class System_EnemyUI : MonoBehaviour
{
    [SerializeField]
    private float enemyMaxHealth;

    [SerializeField]
    private float enemyMaxStamina;

    [SerializeField]
    private Slider bossHealthSlider;

    [SerializeField]
    private Slider bossStaminaSlider;

    private void Start()
    {
        bossHealthSlider.maxValue = enemyMaxHealth;
        bossHealthSlider.value = enemyMaxHealth;
        bossStaminaSlider.maxValue = enemyMaxStamina;
        bossStaminaSlider.value = enemyMaxStamina;
    }

    public void UpdateHealthBarUI(Component sender, object data)
    {
        bossHealthSlider.value = (float) data;
    }

    public void UpdateStaminaBarUI(Component sender, object data)
    {
        bossStaminaSlider.value = (float) data;
    }
}
