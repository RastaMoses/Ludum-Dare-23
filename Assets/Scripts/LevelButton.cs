using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelButton : MonoBehaviour
{
    
    [SerializeField]
    [ColorUsage(true, true)] Color waitColor;
    [SerializeField]
    [ColorUsage(true, true)]
    Color activeColor;
    [SerializeField]
    [ColorUsage(true, true)]
    Color failColor;
    [SerializeField]
    [ColorUsage(true, true)]
    Color offColor;
    [SerializeField] float resetAnimSpeed;
    [SerializeField] public float resetAnimDuration;


    //State
    bool interactable = false;
    public bool pressed = false;


    //Cached Comps
    CharacterController characterController;
    Collider col;
    LevelManager levelManager;
    Material mat;
    VisualEffect buttonParticle;

    private void Awake()
    {
        characterController = FindObjectOfType<CharacterController>();
        levelManager = FindObjectOfType<LevelManager>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        buttonParticle = GetComponentInChildren<VisualEffect>();

        mat.SetColor("_buttonColor", offColor);
        buttonParticle.SetVector4("_buttonColor", offColor);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (interactable && collider.CompareTag("Player") && !pressed)
        {
            Pressed();  
        }
    }

    public void Activate()
    {
        pressed = false;
        interactable = true;
        //Activate Visuals
        mat.SetColor("_buttonColor", activeColor);
        buttonParticle.SetVector4("_buttonColor", activeColor);
    }

    public void Deactivate()
    {
        interactable = false;
        //Deactivate Visuals
        mat.SetColor("_buttonColor", offColor);
        buttonParticle.SetVector4("_buttonColor", offColor);
    }

    public void Pressed()
    {
        pressed = true;
        StopAllCoroutines();
        interactable = false;
        mat.SetColor("_buttonColor", waitColor);
        buttonParticle.SetVector4("_buttonColor", waitColor);
        levelManager.ButtonClick();
    }


    public void ResetButton()
    {
        StartCoroutine(FailAnim());
    }
    public void StopAnimation()
    {
        StopAllCoroutines();
    }

    IEnumerator FailAnim()
    {
        float t = 0;
        bool colorSwitch = false;
        while (t <= resetAnimDuration)
        {
            t += resetAnimSpeed;
            if (colorSwitch) 
            {
                mat.SetColor("_buttonColor", waitColor);
                buttonParticle.SetVector4("_buttonColor", waitColor);
            }
            else 
            {
                mat.SetColor("_buttonColor", failColor);
                buttonParticle.SetVector4("_buttonColor", failColor);
            }
            
            yield return new WaitForSeconds(resetAnimSpeed);
            colorSwitch = !colorSwitch;
        }
        interactable = false;
        Activate();
    }

    public void SetWaitColor()
    {
        mat.SetColor("_buttonColor", waitColor);
        buttonParticle.SetVector4("_buttonColor", waitColor);
    }
    public void SetActiveColor()
    {
        mat.SetColor("_buttonColor", activeColor);
        buttonParticle.SetVector4("_buttonColor", activeColor);
    }
}
