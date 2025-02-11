using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour {

    public EventHandler OnHealthValueChanged;
    
    [SerializeField] private GameObject parentGameObject;
    [SerializeField] private float maxHealthVal = 30;
    public float currentHealthVal = 30;

    public void IncreaseHealthVal(float value) {
        currentHealthVal += value;
        if (currentHealthVal > maxHealthVal) {
            currentHealthVal = maxHealthVal;
        }
    }

    public void DecreaseHealthVal(float value) {
        currentHealthVal -= value;
        OnHealthValueChanged?.Invoke(this, EventArgs.Empty);
        if (currentHealthVal <= 0f) {
            Destroy(parentGameObject);
        }
    }
    
    public float GetHealthValRate() {
        return currentHealthVal / maxHealthVal;
    }

    public float GetMaxHealthVal() {
        return maxHealthVal;
    }

    public void SetMaxHealthVal(float newMax) {
        maxHealthVal = newMax;
    }
    
}
