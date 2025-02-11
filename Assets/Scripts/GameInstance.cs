using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour {

    [SerializeField] private new GameObject light;
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private MapGenerator mapGenerator;

    private static GameInstance Singleton;
    private UPlayerController player;

    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
        }
        mapGenerator.OnGenerateMap += OnGenerateMap;
    }


    private void OnGenerateMap(object sender, EventArgs e) {
        var startPosition = mapGenerator.startRoom.transform.position;
        
        light.transform.position = startPosition;
        camera.transform.position = new Vector3(startPosition.x, startPosition.y, -10);

        player = Instantiate(playerPrefab, startPosition, Quaternion.identity).GetComponent<UPlayerController>();
        player.SetUpRooms(mapGenerator.rooms);
        player.SetUpCamera(camera);

        SkillManager.instance.player = player.gameObject;
        SkillManager.instance.CloseSkillPopup();
    }

    public GameObject GetLight() {
        return light;
    }
    
    public static GameInstance GetSingleton() {
        return Singleton;
    }
    
}
