using System;
using UnityEngine;

public class BaseMonster : MonoBehaviour {

    public float attackVal = 10f;
    public float collisionVal = 10f;
    
    public EventHandler<HealthChangedEventArguement> OnHurt;
    public EventHandler<HealthChangedEventArguement> OnCured;
    [SerializeField] protected HealthComponent healthComponent;
    protected UPlayerController player;

    protected void Awake()
    {
        OnHurt += OnHurtEvent;
        OnCured += OnCuredEvent;
    }

    private void OnHurtEvent(object sender, HealthChangedEventArguement e) {
        if (healthComponent != null) {
            healthComponent.DecreaseHealthVal(e.value);
        }
    }

    private void OnCuredEvent(object sender, HealthChangedEventArguement e) {
        if (healthComponent != null) {
            healthComponent.IncreaseHealthVal(e.value);
        }
    }

    public virtual void ActiveMonster(UPlayerController _player) {
        this.player = _player;
    }

    public virtual void DeActiveMonster() {
        this.player = null;
    }
    
}
