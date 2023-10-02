using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource audio;
    public float startTime;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(Wait(audio.clip.length));
    }


    IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
        audio.Stop();
        audio.time = startTime;
        audio.Play();
        StartCoroutine(Wait(audio.clip.length - startTime));
    }
}
