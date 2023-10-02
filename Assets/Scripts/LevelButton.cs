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
    [SerializeField] float resetAnimDuration;


    //State
    bool interactable = false;


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
        //mat.color = offColor;
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (interactable && collider.CompareTag("Player"))
        {
            Pressed();  
        }
    }

    public void Activate()
    {
        interactable = true;
        //Activate Visuals
        mat.SetColor("_buttonColor", activeColor);
        buttonParticle.SetVector4("_buttonColor", activeColor);
        //mat.color = activeColor;
    }

    public void Deactivate()
    {
        interactable = false;
        //Deactivate Visuals
        
        mat.SetColor("_buttonColor", offColor);
        buttonParticle.SetVector4("_buttonColor", offColor);
        //mat.color = offColor;
    }

    public void Pressed()
    {
        interactable = false;
        mat.SetColor("_buttonColor", waitColor);
        buttonParticle.SetVector4("_buttonColor", waitColor);
        //mat.color = waitColor;
        levelManager.ButtonClick();
    }


    public void ResetButton()
    {
        interactable = false;
        StartCoroutine(FailAnim());
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
                //mat.color = waitColor;
            }
            else 
            {
                mat.SetColor("_buttonColor", failColor);
                buttonParticle.SetVector4("_buttonColor", failColor);
                //mat.color = failColor;
            }
            
            yield return new WaitForSeconds(resetAnimSpeed);
            colorSwitch = !colorSwitch;
        }
        Activate();
    }
}
