using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QUICK.Math;

public class Ball : MonoBehaviour
{
    public float speed = 7;
    public float rotationSpeed = 5;
    public float velocityReserveDecayRate = 25;
    private Vector3 velocityReserve;

    [HideInInspector] public Player currentTarget;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnDeflected(Vector3 hitVelocity)
    {
        velocityReserve = hitVelocity + Vector3.up * 20f;

        if(currentTarget == Session.playerA)
        {
            currentTarget = Session.playerB;
        }
        else
        {
            currentTarget = Session.playerA;
        }

        speed += 0.25f;
        Session.current.IncrementDeflectCount();
    }

    private void MoveForward()
    {
        if (currentTarget)
        {
            rb.velocity = velocityReserve + transform.forward * speed;
        }
    }

    private void RotateTowardsTarget()
    {
        if (currentTarget)
        {
            Vector3 targetDir = (currentTarget.transform.position - transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(targetDir);


            float adjustedRotationSpeed = Vector3.Distance(transform.position, currentTarget.transform.position).Map(2, 30, 10, 1);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * adjustedRotationSpeed);
        }
    }

    private void ConsumeReserveVelocity()
    {
        velocityReserve = Vector3.ClampMagnitude(velocityReserve, Mathf.Max(0, velocityReserve.magnitude - (Time.fixedDeltaTime * velocityReserveDecayRate)));
    }

    private void OnTriggerEnter(Collider other)
    {
        Player hitPlayer = other.GetComponent<Player>();

        if (hitPlayer)
        {
            Session.current.OnPlayerHit();
        }
    }

    private void FixedUpdate()
    {
        RotateTowardsTarget();
        MoveForward();
        ConsumeReserveVelocity();
    }
}
