using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class MonsterMi : BaseMonster {
    
    public List<Transform> points;

    [SerializeField] private float moveSpeed;
    [SerializeField] private MonsterGemini monsterGemini;
    [SerializeField] private MonsterNi monsterNi;

    [SerializeField] private LineRenderer line;
    
    private bool StartMove = false;
    
    private void Start() {
        monsterGemini.OnActiveMonster += OnActiveMonster;
    }

    private void OnActiveMonster(object sender, EventArgs e) {
        StartMove = true;
        StartCoroutine(MoveRound());
    }
    
    private IEnumerator MoveRound() {
        line.positionCount = 2;
        
        int count = 2;
        while (count > 0) {
            foreach (var point in points) {
                while (Vector3.Distance(transform.position, point.position) > 0.001f) {
                    line.SetPosition(0, transform.localPosition);
                    line.SetPosition(1, monsterNi.transform.localPosition);
                    Vector3 moveDir = point.position - transform.position;
                    transform.position += moveDir * moveSpeed * Time.deltaTime;
                    yield return null;
                }
            }
            count--;
        }
        StartMove = false;
        
        line.positionCount = 0;
    }

    private void Update() {

        if (player == null) {
            return;
        }

        if (StartMove) {
            Vector3 direction = (monsterNi.transform.position - transform.position).normalized;
            RaycastHit2D hits = Physics2D.Raycast(transform.position, direction,
                Vector2.Distance(monsterNi.transform.position, transform.position));
            if (hits.collider.transform.TryGetComponent(out UPlayerController playerController)) {
                playerController.OnHurt?.Invoke(this, new HealthChangedEventArguement {
                    value = monsterGemini.attackVal,
                    from = monsterGemini.gameObject
                });
            }
        }

    }
    
}
