using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller : MonoBehaviour
{
    public LayerMask groundLayers;
    public float walkSpeed = 10;
    public float grindSpeed = 15;
    public float jumpStrength = 10f;
    public float gravity = 10f;

    [SerializeField] Transform groundCheck;

    private Vector3 movement = Vector2.zero;
    private CharacterController controller;
    private bool grounded = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    #region Input
    public void Shmoove(InputAction.CallbackContext ctx) {
        movement = transform.rotation * new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
    }
    public void Yump(InputAction.CallbackContext ctx) {
        if (ctx.performed && grounded) { movement.y = jumpStrength; grounded = false; }
    }
    #endregion

    private void FixedUpdate()
    {
        controller.Move(movement * Time.deltaTime * walkSpeed);
        movement.y = Mathf.Clamp(movement.y - Time.deltaTime * gravity, -10, 999);

        if(Physics.SphereCast(groundCheck.position, 0.5f, Vector3.zero, out RaycastHit hit, groundLayers) && movement.y < 0) {
            grounded = true;
            movement.y = -2;
        }
    }
}
