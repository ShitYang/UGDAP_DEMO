using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MonsterGemini : BaseMonster {

    public EventHandler OnActiveMonster;

    public override void ActiveMonster(UPlayerController _player) {
        base.ActiveMonster(_player);
        StartCoroutine(SpaceTime()); 
        OnActiveMonster?.Invoke(this, EventArgs.Empty);
    }

    IEnumerator SpaceTime() {
        yield return new WaitForSeconds(1);
    }
    
}
