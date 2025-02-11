using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MonsterNi : BaseMonster
{
    public List<Transform> points;

    [SerializeField] private float moveSpeed;
    [SerializeField] private MonsterGemini monsterGemini;

    
    private void Start() {
        monsterGemini.OnActiveMonster += OnActiveMonster;
    }

    private void OnActiveMonster(object sender, EventArgs e) {
        StartCoroutine(MoveRound());
    }
    
    private IEnumerator MoveRound() {
        int count = 2;
        while (count > 0) {
            foreach (var point in points) {
                while (Vector3.Distance(transform.position, point.position) > 0.001f) {
                    Vector3 moveDir = point.position - transform.position;
                    transform.position += moveDir * moveSpeed * Time.deltaTime;
                    yield return null;
                }
            }
            count--;
        }
    }
}
