using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public bool start = false;
    public float duration = 1f;
    public AnimationCurve curve; //creates animation curve. can be adjusted from the inspector.
    void Update()
    {
         if (start)
        {
            start = false;
            StartCoroutine(StartShake());
        }
        
    }
    public IEnumerator StartShake ()
    {
        Vector3 originalPos = transform.position;

        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);                 // sets parameters for animation curve.
            transform.position = originalPos + Random.insideUnitSphere * strength;
            //wait one frame
            yield return null;
        }

        transform.position = originalPos;
    }
}
