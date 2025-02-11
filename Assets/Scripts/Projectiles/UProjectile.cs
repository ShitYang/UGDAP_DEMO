using System;
using Unity.VisualScripting;
using UnityEngine;

public class UProjectile : MonoBehaviour {
    
    [SerializeField] protected float flySpeed = 20f;
    [SerializeField] protected Rigidbody2D rigidbody2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    
    public float damage;
    public GameObject from;
    
    // Start is called before the first frame update
    protected void Start() {
        Destroy(gameObject, 3f);
        rigidbody2D.velocity = flySpeed * transform.up;
    }

    // Update is called once per frame
    protected void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            Destroy(gameObject);
            return;
        }
        
        if (from != null && other.gameObject != from) {
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if (from.TryGetComponent(out BossTriangle bossTriangle)) {
            if (other.TryGetComponent(out UPlayerController playerController)) {
                playerController.OnHurt?.Invoke(this, new HealthChangedEventArguement {
                    value = damage,
                    from = gameObject
                });
                Destroy(gameObject);
            }
        }
    }
}
