// Author: Kadir Lofca
// github.com/kadirlofca

using UnityEngine;
using QUICK;

public class AutoPlayer : Player
{
    public float ballHitPower = 50;
    public float ballHitRange = 4;
    public QuickCharacter quickCharacter;
    public Transform controlTransform;

    private void Awake()
    {
        if (!quickCharacter)
        {
            quickCharacter = GetComponent<QuickCharacter>();
        }
    }

    private void MoveTowardsBall()
    {

        //quickCharacter.AddMovementInput(wishDir, input.magnitude);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(controlTransform.position, Session.ball.transform.position) < ballHitRange)
        {
            Session.ball.OnDeflected((GameObject.FindGameObjectWithTag("Player").transform.position - controlTransform.position).normalized * ballHitPower);
        }
    }
}