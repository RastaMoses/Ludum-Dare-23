using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] Color waitColor;
    [SerializeField] Color activeColor;
    [SerializeField] Color failColor;
    [SerializeField] Color offColor;
    [SerializeField] float resetAnimSpeed;
    [SerializeField] float resetAnimDuration;


    //State
    bool interactable = false;


    //Cached Comps
    CharacterController characterController;
    Collider col;
    LevelManager levelManager;

    private void Awake()
    {
        characterController = FindObjectOfType<CharacterController>();
        levelManager = FindObjectOfType<LevelManager>();
        mat = GetComponent<MeshRenderer>().material;

        //mat.SetColor("buttonColor", offColor);
        mat.color = offColor;
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
        //mat.SetColor("buttonColor", activeColor);
        mat.color = activeColor;
    }

    public void Deactivate()
    {
        interactable = false;
        //Deactivate Visuals
        //mat.SetColor("buttonColor", offColor);
        mat.color = offColor;
    }

    public void Pressed()
    {
        interactable = false;
        //mat.SetColor("buttonColor", waitColor);
        mat.color = waitColor;
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
            if (colorSwitch) 
            {
                //mat.SetColor("buttonColor", waitColor); 
                mat.color = waitColor;
            }
            else 
            {
                //mat.SetColor("buttonColor", failColor);
                mat.color = failColor;
            }
            t += resetAnimSpeed;
            yield return new WaitForSeconds(resetAnimSpeed);
            colorSwitch = !colorSwitch;
        }
        Activate();
    }
}
