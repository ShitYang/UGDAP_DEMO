using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackComponent : AbstractAttackComponent {

    public override void Attack(Vector2 startPoint, Vector2 forward) {
        GameObject projectileGameObject = Instantiate(projectilePrefab, startPoint + forward * (new Vector2(0.8f, 0.8f)), Quaternion.identity);
        projectileGameObject.transform.up = forward;
        
        var projectile = projectileGameObject.GetComponent<UProjectile>();
        projectile.from = transform.parent.gameObject;
        projectile.damage = base.damageVal;
    }
}
