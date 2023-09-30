using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    [SerializeField] private Transform[] lerpPositions;
    [SerializeField] private float speed;

    private float lerp; 
    private int index = 1, prevIndex = 0, indexChange = 1, prevIndexChange = 1;
    private Vector3 prevPos, positionDelta;

    private void Start()
    {
        prevPos = transform.position;
    }

    private void Update()
    {
        lerp += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(lerpPositions[prevIndex].position, lerpPositions[index].position, lerp);

        if(transform.position == lerpPositions[index].position) {
            lerp = 0;
            index += indexChange;
            prevIndex += indexChange;
            if (index >= lerpPositions.Length) { index = lerpPositions.Length - 2; indexChange = -1; }
            if(index < 0) { index = 1; indexChange = 1; }
            if (prevIndex >= lerpPositions.Length) { prevIndex = lerpPositions.Length - 2; prevIndexChange = -1; }
            if(prevIndex < 0) { prevIndex = 1; prevIndexChange = 1; }
        }

        positionDelta = transform.position - prevPos;
        prevPos = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") { other.transform.root.GetComponent<FPS_Controller>().posChange = positionDelta; }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") { other.transform.root.GetComponent<FPS_Controller>().posChange = Vector3.zero; }
    }
}
