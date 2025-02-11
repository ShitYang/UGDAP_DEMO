using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public int Id;
    public EventHandler OnPlayerEnterRoom;
    public EventHandler OnPlayerExitRoom;

    public BaseBoss boss;
    public List<BaseMonster> monsters; 
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out UPlayerController player)) {
            OnPlayerEnterRoom?.Invoke(this, EventArgs.Empty);
            if (boss != null) {
                Debug.Log("Active Boss");
                boss.ActiveMonster(player);
            } 
            foreach (var monster in monsters) {
                monster.ActiveMonster(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent(out UPlayerController player)) {
            OnPlayerExitRoom?.Invoke(this, EventArgs.Empty);
            if (boss != null) {
                boss.DeActiveMonster();
            } 
            foreach (var monster in monsters) {
                monster.DeActiveMonster();
            }
        }      
    }
}