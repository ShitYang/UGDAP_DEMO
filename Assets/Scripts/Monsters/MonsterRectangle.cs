using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRectangle : BaseMonster {
    
    [SerializeField] private Vector2 initPosition = new Vector2(0, 0); 
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float activeRadius = 10f;
    [SerializeField] private float rotateSpeed = 1000f;
    [SerializeField] private float rotateTimerRate = 1.8f;
    [SerializeField] private float sprintTimerRate = 2.0f;
    
    private State state = State.ROTATE;
    private float rotateTimer = 0f;
    private float sprintTimer = 1f;
    
    private enum State {
        ROTATE,
        SPRINT
    }
    
    private void Start() {
    }

    private void Update() {
        
        if (player == null) {
            return;
        }

        DoAttack();

    }
    
    private void DoAttack() {

        if (state == State.SPRINT) {
            sprintTimer = Mathf.Lerp(sprintTimer, 0, sprintTimerRate * Time.deltaTime);
            transform.position += (Vector3)(transform.up * (sprintTimer * moveSpeed * Time.deltaTime));
            if (Mathf.Abs(sprintTimer) < 0.01f) {
                rotateTimer = 0f;
                sprintTimer = 0f;
                state = State.ROTATE;
            }
        } else {
            rotateTimer = Mathf.Lerp(rotateTimer, 1f, rotateTimerRate * Time.deltaTime);
            transform.Rotate(0, 0, rotateTimer * rotateSpeed * Time.deltaTime);
            if (Mathf.Abs(rotateTimer - 1) < 0.01f && Vector3.Dot(transform.up, (player.transform.position - transform.position).normalized) >= 0.99f) {
                rotateTimer = 1f;
                sprintTimer = 1f;
                state = State.SPRINT;
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent(out UProjectile projectile)) {
            if (projectile.from.TryGetComponent(out UPlayerController _player)) {
                OnHurt?.Invoke(this, new HealthChangedEventArguement {
                    value = projectile.damage,
                    from = _player.gameObject
                });
            }
        }
    }

    private void OnDestroy() {
        ParticleManager.GetInstance().PlayTo(ParticleManager.Type.EXPLOSIION, transform.position);
    }
}