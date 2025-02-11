using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private HealthComponent _healthComponent;
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetActive(bool active) {
        if (Mathf.Approximately(healthSlider.value, 0f)) {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(active);
    }

    private void Update() {
        healthSlider.value = healthSlider.maxValue * _healthComponent.GetHealthValRate();
        if (Mathf.Approximately(healthSlider.value, 0f)) {
            gameObject.SetActive(false);
        }
    }
}
