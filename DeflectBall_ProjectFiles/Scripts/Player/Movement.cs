// Author: Kadir Lofca
// github.com/kadirlofca

using UnityEngine;
using QUICK;

public class Movement : QuickCharacter
{
    [Header("Ground Movement")]
    public Gait gait;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float cayoteTime = 0.15f;

    [Header("Air Movement")]
    public float drag = 0.06f;
    public float airControl = 1.4f;
    public float airAcceleration = 8f;
    public float airAccelBoostThreshold = 4f;
    public float ascendingGravity = 12f;
    public float descendingGravity = 16f;

    [Header("References")]
    public Transform controlTransform;
    public Animator fpsceneAnimator;

    public void OnJump()
    {
        bool jumpSuccessful = Jump(jumpForce, 2, true, cayoteTime);

        // Animate jumps.
        if (jumpSuccessful && fpsceneAnimator)
        {
            if (numberOfJumps == 1)
            {
                fpsceneAnimator.SetTrigger("Jump");
            }
            else
            {
                fpsceneAnimator.ResetTrigger("Jump");
                fpsceneAnimator.SetTrigger("DoubleJump");
            }
        }
    }

    private MoveMedium FindNextMedium()
    {
        if (floor.isValid)
        {
            return MoveMedium.ground;
        }
        else
        {
            return MoveMedium.air;
        }
    }

    protected override MoveMedium PhysicsUpdate()
    {
        switch (medium)
        {
            case MoveMedium.ground:
                ApplyFloorMovement(gait);
                break;

            case MoveMedium.air:
                ApplyGravity(descendingGravity, ascendingGravity);
                ApplyAirControlMovement(airControl);
                ApplyAirMovement(rb.velocity.XZ().magnitude.Map(airAccelBoostThreshold, 0, 0, airAcceleration));
                ApplyDrag(drag);
                break;

            default:
                return MoveMedium.air;
        }

        return FindNextMedium();
    }

    protected override void OnMediumChange(MoveMedium oldMedium)
    {
        if (!fpsceneAnimator)
        {
            return;
        }

        if (DidMediumChangeTo(oldMedium, MoveMedium.ground))
        {
            fpsceneAnimator.ResetTrigger("Jump");
            fpsceneAnimator.ResetTrigger("DoubleJump");
            fpsceneAnimator.SetTrigger("Land");
        }
        else
        {
            fpsceneAnimator.ResetTrigger("Land");
        }
    }

    private void Update()
    {
        if (!fpsceneAnimator)
        {
            return;
        }

        fpsceneAnimator.SetBool("Running", HasMovementInput() && isOnFloor);
        fpsceneAnimator.SetFloat("RunSpeed", velocity.magnitude.Map(0, gait.speed, 0, 1));
    }
}