using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 movementVector;
    public float speed;
    private Animator animator;
    private Rigidbody rb;
    private Health health;
    


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        speed = 1.5f;
    }

    private void Update()
    {
        CalculateMovement();
        if(movementVector != Vector3.zero) {
            SetRotation();
        }
        animator.SetBool("Walking", movementVector != Vector3.zero);

    }

    private void CalculateMovement()
    {
        movementVector = new Vector3 (Input.GetAxis("Horizontal"),rb.velocity.y, Input.GetAxis("Vertical"));
        rb.velocity = new Vector3(movementVector.x * speed, movementVector.y, movementVector.z * speed);

    }

    private void SetRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), 0.1f);
    }

    public void Hurt(int amount, int delay = 0)
    {
        StartCoroutine(health.TakeDamageDelayed(amount, delay));
    }
}
