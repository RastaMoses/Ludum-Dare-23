using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class FPS_Controller : MonoBehaviour
{
    //Serialize Params
    public Gun gun;
    public UIManager ui;
    public GameObject meleePlatform, slash;
    public Transform shotSpawn;
    public LayerMask groundLayers;
    public float walkSpeed = 10, grindSpeed = 15, jumpStrength = 10f, gravity = 10f;

    [SerializeField] Transform groundCheck;
    public Vector3 posChange = Vector3.zero;
    public Animator rightHand, leftHand;
    public VisualEffect vfx;

    
    
    //Cached Componens
    private SFX sfx;
    private Score score;

    //State
    private Vector3 movement = Vector3.zero, grindDir = Vector3.zero;
    private Vector2 endPos = new Vector2(999, 999);
    private CharacterController controller;
    [HideInInspector] public float currentSpeed, barCharge = 1, dashCharge = 2, railCharge = 4;
    private int jumpCount = 2;
    private bool canShoot = true, grinding = false, pounding = false, canDash = false, grounded = true, canSlash = true, dashing = false;
    
    
    private void Awake()
    {
        //Get Comps
        controller = GetComponent<CharacterController>();
        sfx = GetComponent<SFX>();
        score = GetComponent<Score>();
    }
    private void Start()
    {
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
            sfx.RailGrind(false);
            grinding = false;
            canDash = true;
            grounded = false;
        }
        if (ctx.performed && jumpCount > 0 && !pounding) { 
            movement.y = jumpStrength;
            jumpCount--;
            sfx.RailGrind(false);
            grinding = false;
            canDash = true;
            grounded = false;
        }
    }
    public void Shoot(InputAction.CallbackContext ctx) { 
        if(ctx.performed && canShoot && railCharge >= 1) {
            vfx.SendEvent("OnPlay");
            rightHand.SetTrigger("shoot");
            sfx.Shoot();
            railCharge -= 1;
            StartCoroutine(ShootCooldown());
            Instantiate(gun.boolet, shotSpawn.position, shotSpawn.rotation);
        }
        else if(railCharge < 1) { sfx.GunEmpty(); }
    }
    public void Melee(InputAction.CallbackContext ctx) {
        if (ctx.performed && canSlash)
        {
            rightHand.SetTrigger("slash");
            leftHand.SetTrigger("slash");
            sfx.Melee();
            canSlash = false;
            StartCoroutine(SlashCooldown());
            Instantiate(slash, shotSpawn.position, shotSpawn.rotation);
        }
    }
    public void Platform(InputAction.CallbackContext ctx) {
        if (ctx.performed && barCharge == 1)
        {
            barCharge = 0;
            Melee_Platform plat = FindObjectOfType<Melee_Platform>();
            if (plat != null) { Destroy(plat.transform.root.gameObject); }
            Instantiate(meleePlatform, shotSpawn.position, shotSpawn.rotation);
        }
    }
    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canDash && dashCharge >= 1) {
            rightHand.SetTrigger("dash");
            leftHand.SetTrigger("dash");
            sfx.Dash();
            sfx.RailGrind(false);
            grinding = false;
            dashCharge -= 1;
            StartCoroutine(Dash(shotSpawn.forward));
            sfx.Dash();
        }
    }
    public void Crouch(InputAction.CallbackContext ctx)
    {
        if((jumpCount < 2 && ctx.performed) || (grinding && ctx.performed)) {
            sfx.RailGrind(false);
            rightHand.SetBool("pound", true);
            grinding = false;
            pounding = true; 
            movement.y = -6; 
        }
    }
    #endregion

    private void Update()
    {
        barCharge = Mathf.Clamp(barCharge + Time.deltaTime / 10, 0, 1);
        railCharge = Mathf.Clamp(railCharge + Time.deltaTime / 20, 0, 4);

        //UI
        ui.UpdateGun(railCharge);
        ui.UpdatePlatform(barCharge);
        ui.UpdateDash(dashCharge);


        dashCharge = Mathf.Clamp(dashCharge + Time.deltaTime, 0, 2);

        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), endPos) < 1) {
            movement.y = jumpStrength * 0.5f;
            jumpCount--;
            sfx.RailGrind(false);
            grinding = false;
            canDash = true;
            grounded = false;
            endPos = new Vector2(999, 999);
        }

        Vector3 xyMove = transform.rotation * movement;

        if (!grinding && !dashing) {
            xyMove.y = movement.y;
            xyMove += posChange * currentSpeed;
            controller.Move(xyMove * Time.deltaTime * currentSpeed);
        }
        else if (!dashing) {
            railCharge = Mathf.Clamp(railCharge + Time.deltaTime * 2, 0, 4);
            controller.Move(grindDir * Time.deltaTime * currentSpeed);
            barCharge = Mathf.Clamp(barCharge + Time.deltaTime, 0, 1);
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
        sfx.RailGrind(true);
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

    private IEnumerator SlashCooldown() {
        yield return new WaitForSeconds(1);
        canSlash = true;
    }
    private IEnumerator Dash(Vector3 dashDir) {
        float timer = 0;
        float _speed = currentSpeed;
        dashing = true;
        while (timer < 0.15f) {
            timer += Time.deltaTime;
            controller.Move(dashDir * Time.deltaTime * 75);
            yield return null;
        }
        dashing = false;
        currentSpeed = _speed;
    }
    private IEnumerator ShootCooldown() {
        canShoot = false;
        yield return new WaitForSeconds(gun.fireRate);
        canShoot = true;
    }
    private IEnumerator PoundJump() {
        rightHand.SetBool("pound", false);
        yield return new WaitForSeconds(0.25f);
        pounding = false;
    }
    public void Launch() {
        movement.y = jumpStrength * 1.25f;
        sfx.RailGrind(false);
        grinding = false;
    }
    public void Kill(int scoreValue, string enemyName) {
        dashCharge = Mathf.Clamp(dashCharge + 1, 0, 2);
        railCharge = Mathf.Clamp(railCharge + 1, 0, 4);
        score.IncreaseScore(scoreValue, enemyName);
        score.IncreaseMultiplier(1);
    }
}
