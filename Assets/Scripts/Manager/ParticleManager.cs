using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    [SerializeField] private List<Type> types;
    [SerializeField] private List<GameObject> particles;
    private Dictionary<Type, GameObject> Type2Particle;
    private static ParticleManager Instance;
    
    public enum Type {
        EXPLOSIION,
        COLLISION,
        FIRE,
        BIGEXPLOSION
    }

    private void Awake() {
        if (Instance != null) {
            return;
        }
        Instance = this;
    }

    private void Start() {
        Type2Particle = new Dictionary<Type, GameObject>();
        Debug.Log(types.Count + " " + particles.Count);
        for (int i = 0; i < types.Count; ++i) {
            Type2Particle.Add(types[i], particles[i]);
        }
    }

    public void PlayTo(Type type, Vector3 position) {
        ParticleSystem particleSystem = Instantiate(Type2Particle[type], position, Quaternion.identity).GetComponent<ParticleSystem>();
        particleSystem.Play();
        Destroy(particleSystem.gameObject, 3);
    }

    public static ParticleManager GetInstance() {
        return Instance;
    }
    
}
