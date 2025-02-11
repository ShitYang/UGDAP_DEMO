using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class AbstractAttackComponent : MonoBehaviour {
    
    [SerializeField] protected GameObject projectilePrefab;
    protected float damageVal;

    public void SetUpDamageVal(float _damageVal) {
        damageVal = _damageVal;
    }

    public virtual void Attack(Vector2 startPoint, Vector2 forward) {
        
    }
    
}
