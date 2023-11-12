using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] private float mouseSense = 100f;

    private Vector2 mouseDelta;
    private float xRot;
    bool lockCam;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MouseDelta(InputAction.CallbackContext ctx)
    {
        mouseDelta = ctx.ReadValue<Vector2>() * Time.smoothDeltaTime * mouseSense;
    }

    void Update()
    {
        if (lockCam) { return; }
        xRot -= mouseDelta.y;
        xRot = Mathf.Clamp(xRot, -90f, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        body.Rotate(Vector3.up * mouseDelta.x);
    }
    public void LockCamera()
    {
        lockCam = true;
    }
    public void UnlockCamera()
    {
        lockCam = false;
    }

    public void LockMouse()
    {
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
