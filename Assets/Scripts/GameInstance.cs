using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour {
    
    [SerializeField] private GameObject roomlight;
    [SerializeField] private Camera maincamera;
    [SerializeField] private List<Room> rooms;
    [SerializeField] private GameObject playerPrefab;

    private static GameInstance Singleton;
    private UPlayerController player;
    
    private void Start() {

        if (Singleton == null) {
            Singleton = this;//ʵ�ֵ���
        }
        
        player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity).GetComponent<UPlayerController>();
        player.SetUpRooms(rooms);//���ݷ�������
        player.SetUpCamera(maincamera);//�������������

    }

    public GameObject GetLight() {
        return roomlight;
    }
    
    public static GameInstance GetSingleton() {
        return Singleton;
    }
    
}
