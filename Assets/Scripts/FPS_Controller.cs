using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller : MonoBehaviour
{
    public Gun gun;
    public Transform shotSpawn;
    public LayerMask groundLayers;
    public float walkSpeed = 10, grindSpeed = 15, jumpStrength = 10f, gravity = 10f;

    [SerializeField] Transform groundCheck;

    private Vector3 movement = Vector3.zero, grindDir = Vector3.zero;
    private CharacterController controller;
    private int jumpCount = 2;
    private bool canShoot = true, grinding = false, pounding = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    #region Input
    public void Shmoove(InputAction.CallbackContext ctx) {
        float y = movement.y;
        movement = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
        movement.y = y;
    }
    public void Yump(InputAction.CallbackContext ctx) {
        if(ctx.performed && jumpCount == 2 && pounding) {
            movement.y = jumpStrength * 2f;
            jumpCount--;
            grinding = false;
        }
        if (ctx.performed && jumpCount > 0 && !pounding) { 
            movement.y = jumpStrength;
            jumpCount--;
            grinding = false;
        }
    }
    public void Shoot(InputAction.CallbackContext ctx) { 
        if(ctx.performed && canShoot) {
            StartCoroutine(ShootCooldown());
            Rail rail = Instantiate(gun.boolet, shotSpawn.position, shotSpawn.rotation).GetComponent<Rail>();
        }
    }
    public void Dash(InputAction.CallbackContext ctx)
    {

    }
    public void Crouch(InputAction.CallbackContext ctx)
    {
        if(jumpCount < 2 && ctx.performed) { pounding = true; movement.y = -6; }
    }
    #endregion

    private void Update()
    {
        Vector3 xyMove = transform.rotation * movement;

        if (!grinding) {
            xyMove.y = movement.y;
            controller.Move(xyMove * Time.deltaTime * walkSpeed);
        }
        else {
            controller.Move(grindDir * Time.deltaTime * grindSpeed);
        }

        if(jumpCount < 2 && jumpCount >= 0) { movement.y = Mathf.Clamp(movement.y - Time.deltaTime * gravity, -10, 999); }
    }

    public void BeginGrind(Vector3 railForward) {
        if(movement.y > 0 || grinding) { return; }
        jumpCount = 2;
        Vector3 playerLook = transform.forward;
        playerLook = new Vector3(playerLook.x, 0, playerLook.z);
        playerLook.Normalize();
        float dotProduct = Vector3.Dot(playerLook, railForward);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        if (angle <= 90) { grindDir = railForward; }
        else { grindDir = -railForward; }
        grinding = true;
    }

    public void Ground() {
        if (pounding) { StartCoroutine(PoundJump()); }
        Debug.Log("GRounded");
        movement.y = -0.25f;
        jumpCount = 2;
    }

    private IEnumerator ShootCooldown() {
        canShoot = false;
        yield return new WaitForSeconds(gun.fireRate);
        canShoot = true;
    }
    private IEnumerator PoundJump() {
        yield return new WaitForSeconds(0.25f);
        pounding = false;
    }
}
