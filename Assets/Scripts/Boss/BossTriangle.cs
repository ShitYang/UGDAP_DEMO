using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossTriangle : BaseBoss
{
    [Header("Settings")]
    [SerializeField] private GameObject[] splitPrefabs;
    [SerializeField] private float splitRadius = 2f;
    [SerializeField] private int currentSplitLevel = 0;
    
    public float[] splitThresholds = { 0.7f, 0.6f };
    private bool isSplitTriggered = false;
    public bool isMain = true;
    
    protected void Start() {
        base.Start();
        if (healthComponent != null) {
            healthComponent.OnHealthValueChanged += OnHealthValueChangedEvent;
        } else {
            Debug.Log("HealthComponent Empty");
        }
    }

    public override void ActiveMonster(UPlayerController _player) {
        base.ActiveMonster(_player);
        _bossHealthBar.SetActive(true);
    }

    public override void DeActiveMonster() {
        base.DeActiveMonster();
        _bossHealthBar.SetActive(false);
    }
    
    private void OnHealthValueChangedEvent(object sender, EventArgs e) {
        CheckSplitCondition();
    }

    private void FixedUpdate() {
        if (player == null) {
            return;
        }
        base.HandleMovement();
        base.HandleShooting();
    }
    
    private void CheckSplitCondition() {

        float rate = healthComponent.GetHealthValRate();
        if (currentSplitLevel < splitThresholds.Length &&
            rate <= splitThresholds[currentSplitLevel] &&
            !isSplitTriggered)
        {
            isSplitTriggered = true;
            StartCoroutine(SplitBoss());
        }
    }

    private IEnumerator SplitBoss()
    {
        yield return new WaitForSeconds(0.1f); 

        foreach (var prefab in splitPrefabs)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * splitRadius;
            GameObject childBoss = Instantiate(prefab, spawnPos, Quaternion.identity);
            
            if (childBoss.TryGetComponent(out BossTriangle childController)) {
                childController.isMain = false;
                childController.player = player;
                childController.currentSplitLevel = currentSplitLevel + 1;
            }
        }
    }

    protected override void OnDestroy() {
        ParticleManager.GetInstance().PlayTo(ParticleManager.Type.EXPLOSIION, transform.position);
        if (isMain) {
            SkillManager.instance.OpenSkillPanel();   
        }
    }

}
