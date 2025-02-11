using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoss : BaseMonster {


    public EventHandler OnBossDead;
    [SerializeField] protected BossHealthBar _bossHealthBar;
   
    public float moveSpeed = 3f;
    public float minDistance = 0f;//������
    
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public Transform[] firePoints;

    public float nextFireTime;

    protected void Start() {
        
    }

    protected virtual void HandleMovement()
    {
        // ���㳯����ҵķ���
        Vector2 direction = (player.transform.position - transform.position).normalized;

        //Debug.DrawLine(Vector2.zero,direction,Color.white,2.5f);

        // �ƶ�
        transform.Translate(
            moveSpeed * Time.deltaTime * direction
        );
    }
    protected virtual void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            foreach (Transform point in firePoints)
            {
                Vector3 fireDirection = (player.transform.position - transform.position).normalized;
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    point.position,
                    Quaternion.identity
                );
                
                UProjectile uProjectile = projectile.GetComponent<UProjectile>();
                uProjectile.from = gameObject;
                uProjectile.damage = attackVal;
                
                projectile.transform.up = fireDirection;
            }
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent(out UProjectile projectile)) {
            if (projectile.from != gameObject) {
                OnHurt?.Invoke(this, new HealthChangedEventArguement {
                    value = projectile.damage,
                    from = projectile.from
                });
            }
        }
    }
    
    protected virtual void OnDestroy() {
        ParticleManager.GetInstance().PlayTo(ParticleManager.Type.EXPLOSIION, transform.position);
        SkillManager.instance.OpenSkillPanel();
        OnBossDead?.Invoke(this, EventArgs.Empty);
    }
    
}
