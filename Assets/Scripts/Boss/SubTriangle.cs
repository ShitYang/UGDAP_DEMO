using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTriangle : BaseBoss
{
    [Header("???????")]
    [SerializeField] private int breakawayDamageThreshold = 50; // ?????????????
    [SerializeField] private float followSmoothness = 5f;  // ?????????
    [SerializeField] private float positionUpdateInterval = 0.2f; // ��???????

    [Header("???????")]
    [SerializeField] private float followOffset = 2f; // ??????????
    [SerializeField] private float independentSpeed = 1f; // ??????????

    public Transform parent;
    private Vector3 baseLocalPosition; // ??????��??
    private Vector3 targetFollowPos;
    private int currentDamageTaken;
    public bool isIndependent = false;
    private float positionUpdateTimer;

    public Rigidbody2D rb;
    private void Start()
    {
        transform.localPosition = baseLocalPosition;
        currentDamageTaken = 0;
    }
    private void Update()
    {
        if (!isIndependent)
        {
            UpdateFollowPosition();
            base.HandleShooting(); // ??��?????????
        }
    }
    private void FixedUpdate()
    {

        if (player == null) {
            return;
        }
        
        if (isIndependent)
        {

            HandleMovement();

        }
        else
        {
            SmoothFollowMovement();
        }

    }

     

        


    public void TakeDamage(int damage) {
        currentDamageTaken += damage;
       
            // base.TakeDamage(damage);

        if (currentDamageTaken >= breakawayDamageThreshold)
        {
            BreakawayFromParent();
        }
    }


    private void BreakawayFromParent()
    {
        isIndependent = true;
        transform.SetParent(null); // ?????????

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.angularDrag = 0.5f;

        Debug.Log($"{gameObject.name} ??????????");
    }


    protected override void HandleMovement()
    {
        
        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (rb != null)
        {
            rb.velocity = direction * independentSpeed;
        }
    }
    private void UpdateFollowPosition()
    {
        positionUpdateTimer += Time.deltaTime;
        if (positionUpdateTimer >= positionUpdateInterval)
        {
            positionUpdateTimer = 0;
            if (transform.parent != null)
            {
                // ????????????????��??
                Vector3 parentForward = transform.parent.up;
                targetFollowPos = transform.parent.position + parentForward * followOffset;
            }
        }
    }

    // ??????????
    private void SmoothFollowMovement()
    {
        if (transform.parent == null) return;

        transform.position = Vector3.Lerp(
            transform.position,
            targetFollowPos,
            followSmoothness * Time.fixedDeltaTime
        );

        // ????????????
        Vector3 lookDirection = transform.parent.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
