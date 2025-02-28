using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class UPlayerController : MonoBehaviour {

    public EventHandler<HealthChangedEventArguement> OnHurt;
    public EventHandler<HealthChangedEventArguement> OnCured;
    
    [SerializeField] private GameObject attackComponentGameObject;
    [SerializeField] private GameObject healthComponentGameObject;
    [SerializeField] private float fireDeltaTime = 1.0f;
    
    [Header("Skill Property")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackVal = 10f;
    
    private float fireTimer = 0f;

    private List<Room> rooms;
    private Room currentRoom;
    private Camera camera;

    private AbstractAttackComponent attackComponent;
    private HealthComponent healthComponent;
    
    private Rigidbody2D body;
    
    // Start is called before the first frame update
    private void Start() {
        body = GetComponent<Rigidbody2D>();
        
        attackComponent = attackComponentGameObject.GetComponent<AbstractAttackComponent>();
        healthComponent = healthComponentGameObject.GetComponent<HealthComponent>();
        
        OnHurt += OnHurtEvent;
        OnCured += OnCuredEvent;

        SetUpAttackValue(attackVal);
    }
    
    private void OnHurtEvent(object sender, HealthChangedEventArguement e) {
        healthComponent.DecreaseHealthVal(e.value);
    }

    private void OnCuredEvent(object sender, HealthChangedEventArguement e) {
        healthComponent.IncreaseHealthVal(e.value);
    }
    
    // Update is called once per frame
    void Update()
    {
        // 获取输入
        float moveX = Input.GetAxis("Horizontal"); // 左右移动
        float moveY = Input.GetAxis("Vertical");   // 上下移动

        // 计算移动向量
        Vector2 movement = new (moveX, moveY);

        // 移动对象
        body.MovePosition((Vector2)transform.position + moveSpeed * Time.deltaTime * movement);
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
        
        if (Input.GetKey(KeyCode.Mouse0)) {
            fireTimer += 10 * Time.deltaTime;
            if (fireTimer >= fireDeltaTime) {
                fireTimer = 0f;
                Vector3 pressPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                pressPosition.z = 0f;
                Vector3 fireDirection = (pressPosition - transform.position).normalized;
                attackComponent.Attack(transform.position, fireDirection);
                
                SoundManager.GetInstance().PlayTo(SoundManager.Type.SHOOT, transform.position);
            }
        }
    }

    public void SetUpRooms(List<Room> rooms) {
        this.rooms = rooms;
        foreach (Room room in rooms) {
            room.OnPlayerExitRoom += OnPlayerExitRoom; 
            room.OnPlayerEnterRoom += OnPlayerEnterRoom;
        }
    }

    public void SetUpCamera(Camera camera) {
        this.camera = camera;
    }

    private void OnPlayerEnterRoom(object sender, EventArgs e) {
        Room newRoom = sender as Room;
        currentRoom = newRoom;
    }

    private void OnPlayerExitRoom(object sender, EventArgs e) {
        Debug.Log("Leave Room");
    }
    
    
    // Tool Callback()
    public HealthComponent GetHealthComponent() {
        return healthComponent;
    }

    public void SetUpMoveSpeed(float _moveSpeed) {
        moveSpeed = _moveSpeed;
    }
    
    public void SetUpAttackValue(float _attackVal) {
        attackVal = _attackVal;
        attackComponent.SetUpDamageVal(_attackVal);
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            ParticleManager.GetInstance().PlayTo(ParticleManager.Type.COLLISION, other.transform.position);
            // StartCoroutine()
        } else if (other.gameObject.TryGetComponent(out UProjectile projectile)) {
            if (projectile.from != gameObject) {
                OnHurt?.Invoke(projectile.from, new HealthChangedEventArguement {
                    value = projectile.damage,
                    from = projectile.from
                });
            }
        } else if (other.gameObject.TryGetComponent(out BaseMonster baseMonster)) {
            OnHurt?.Invoke(baseMonster, new HealthChangedEventArguement {
                value = baseMonster.collisionVal,
                from = baseMonster.gameObject
            });
        } else {
            Debug.Log("Unknown Check");
        }
    }
    
    public float GetAttackVal() {
        return attackVal;
    }

    // private IEnumerator CameraShake() {
    //     
    //     yield break;
    //     
    // }

    public void ChangeWeapon(GameObject weapon) {
        attackComponentGameObject = weapon;
        attackComponent = weapon.GetComponent<AbstractAttackComponent>();
        SetUpAttackValue(attackVal);
    }

    private void OnDestroy() {
        ParticleManager.GetInstance().PlayTo(ParticleManager.Type.BIGEXPLOSION, transform.position);
    }
}
