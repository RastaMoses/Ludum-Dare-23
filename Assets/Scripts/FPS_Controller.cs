using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FPS_Controller : MonoBehaviour
{
    public Gun gun;
    public GameObject meleePlatform, slash;
    public Image meleeChargeUI;
    public Transform shotSpawn;
    public LayerMask groundLayers;
    public float walkSpeed = 10, grindSpeed = 15, jumpStrength = 10f, gravity = 10f;

    [SerializeField] Transform groundCheck;
    public Vector3 posChange = Vector3.zero;

    private Vector3 movement = Vector3.zero, grindDir = Vector3.zero;
    private Vector2 endPos = new Vector2(999, 999);
    private CharacterController controller;
    private float currentSpeed, barCharge = 1;
    private int jumpCount = 2;
    private bool canShoot = true, grinding = false, pounding = false, canDash = false, grounded = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    #region Input
    public void Shmoove(InputAction.CallbackContext ctx) {
        float y = movement.y;
        movement = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
        movement.y = y;
    }
    public void Yump(InputAction.CallbackContext ctx) {
        if(ctx.performed && grounded && pounding) {
            movement.y = jumpStrength * 2f;
            jumpCount--;
            grinding = false;
            canDash = true;
            grounded = false;
        }
        if (ctx.performed && jumpCount > 0 && !pounding) { 
            movement.y = jumpStrength;
            jumpCount--;
            grinding = false;
            canDash = true;
            grounded = false;
        }
    }
    public void Shoot(InputAction.CallbackContext ctx) { 
        if(ctx.performed && canShoot) {
            StartCoroutine(ShootCooldown());
            Instantiate(gun.boolet, shotSpawn.position, shotSpawn.rotation);
        }
    }
    public void Melee(InputAction.CallbackContext ctx) {
        if (ctx.performed && barCharge == 1)
        {
            barCharge = 0;
            Melee_Platform plat = FindObjectOfType<Melee_Platform>();
            if (plat != null) { Destroy(plat.transform.root.gameObject); }
            meleeChargeUI.fillAmount = 0;
            Instantiate(meleePlatform, shotSpawn.position, shotSpawn.rotation);
            Instantiate(slash, shotSpawn.position, shotSpawn.rotation);
        }
    }
    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canDash) {
            grinding = false;
            StartCoroutine(Dash(shotSpawn.forward));
        }
    }
    public void Crouch(InputAction.CallbackContext ctx)
    {
        if((jumpCount < 2 && ctx.performed) || (grinding && ctx.performed)) {
            grinding = false;
            pounding = true; 
            movement.y = -6; 
        }
    }
    #endregion

    private void Update()
    {
        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), endPos) < 1) {
            movement.y = jumpStrength * 0.5f;
            jumpCount--;
            grinding = false;
            canDash = true;
            grounded = false;
            endPos = new Vector2(999, 999);
        }

        Vector3 xyMove = transform.rotation * movement;

        if (!grinding) {
            xyMove.y = movement.y;
            xyMove += posChange * currentSpeed;
            controller.Move(xyMove * Time.deltaTime * currentSpeed);
        }
        else {
            controller.Move(grindDir * Time.deltaTime * currentSpeed);
            barCharge = Mathf.Clamp(barCharge + Time.deltaTime, 0, 1);
            meleeChargeUI.fillAmount = barCharge;
        }

        if(!grounded) { movement.y = Mathf.Clamp(movement.y - Time.deltaTime * gravity, -10, 999); }
    }

    public void BeginGrind(Vector3 railForward, Vector3 end1, Vector3 end2) {
        if (movement.y > 0 || grinding || pounding || currentSpeed == 0) { return; }
        #region Movement Calc
        currentSpeed = grindSpeed;
        jumpCount = 2;
        Vector3 playerLook = transform.forward;
        playerLook = new Vector3(playerLook.x, 0, playerLook.z);
        playerLook.Normalize();
        float dotProduct = Vector3.Dot(playerLook, railForward);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        if (angle <= 90) { grindDir = railForward; }
        else { grindDir = -railForward; }
        #endregion
        #region EndCalc
        dotProduct = Vector3.Dot(playerLook, end1.normalized);
        angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        dotProduct = Vector3.Dot(playerLook, end2.normalized);
        float angle2 = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        if(angle < angle2) { endPos = new Vector2(end1.x, end1.z); }
        else { endPos = new Vector2(end2.x, end2.z); }
        #endregion
        grinding = true;
    }
    public void Ground() {
        if(movement.y > 0) { return; }
        if (pounding) { StartCoroutine(PoundJump()); }
        grounded = true;
        currentSpeed = walkSpeed;
        movement.y = -0.25f;
        jumpCount = 2;
    }
    public void UnGround() {
        grounded = false;
    }

    private IEnumerator Dash(Vector3 dashDir) {
        float timer = 0;
        float _speed = currentSpeed;
        currentSpeed = 0;
        while (timer < 0.15f) {
            timer += Time.deltaTime;
            controller.Move(dashDir * Time.deltaTime * 75);
            yield return null;
        }
        currentSpeed = _speed;
        movement.y = 0;
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
