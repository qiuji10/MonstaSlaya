using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HealthSlider : MonoBehaviour
{
    Slider healthBar;
    void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    public void BossHealthChange(int damage)
    {
        healthBar.value -= damage;
    }
}
