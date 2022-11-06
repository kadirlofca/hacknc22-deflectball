// Author: Kadir Lofca
// github.com/kadirlofca

using UnityEngine;
using QUICK;

public class HumanPlayer : Player
{
    public float ballHitPower = 25f;
    public float ballHitRange = 2f;
    public QuickCharacter quickCharacter;

    [Header("Camera")]
    public Transform controlTransform;
    public float cameraSensitivity = 0.5f;

    private float cameraPitch = 0;
    private const float MAX_CAMERA_PITCH = 89.99f;

    private QuickInputActions.SimpleActions actions;

    private void Awake()
    {
        if (!quickCharacter)
        {
            quickCharacter = GetComponent<QuickCharacter>();
        }

        actions = new QuickInputActions().Simple;
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void UpdateCameraPosition()
    {
        if (!controlTransform)
        {
            return;
        }

        // This will handle changes in location when character moves and when the capsule height changes.
        controlTransform.position = quickCharacter.topCenter;
    }

    private void UpdateLookInput()
    {
        Vector2 input = actions.Look.ReadValue<Vector2>();
        input *= cameraSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch - input.y, -MAX_CAMERA_PITCH, MAX_CAMERA_PITCH);
        float cameraYaw = controlTransform.rotation.eulerAngles.y + input.x;

        controlTransform.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
    }

    private void UpdateMovementInput()
    {
        Vector2 input = actions.Move.ReadValue<Vector2>();

        Vector3 wishDir = Vector3.zero;
        wishDir += Vector3.ProjectOnPlane(controlTransform.forward, Vector3.up).normalized * input.y;
        wishDir += controlTransform.right.normalized * input.x;

        quickCharacter.AddMovementInput(wishDir, input.magnitude);
    }

    private void Update()
    {
        UpdateCameraPosition();
        UpdateLookInput();
        UpdateMovementInput();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public override void HitBall()
    {
        if (Vector3.Distance(controlTransform.position, Session.ball.transform.position) < ballHitRange)
        {
            Session.ball.OnDeflected(controlTransform.forward * ballHitPower);
        }
    }

    public void OnPrimaryAction()
    {
        HitBall();
    }
}