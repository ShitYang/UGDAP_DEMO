using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCircle : BaseBoss
{
    [SerializeField] private float rotationSpeed = 30f;

    protected void Start()
    {
        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        base.HandleMovement();
        base.HandleShooting();
    }
}


