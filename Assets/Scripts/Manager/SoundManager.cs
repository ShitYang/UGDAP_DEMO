using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    
    [SerializeField] private List<AudioClip> shootClips;
    
    private static SoundManager Instance;

    public enum Type {
        SHOOT
    }
    
    private void Start() {
        if (Instance != null) {
            return;
        }
        Instance = this;
    }

    public static SoundManager GetInstance() {
        return Instance;
    }

    public void PlayTo(Type type, Vector3 position) {
        if (type == Type.SHOOT) {
            int randomIndex =  Random.Range(0, shootClips.Count);
            AudioSource.PlayClipAtPoint(shootClips[randomIndex], position);
        }
    }

}
