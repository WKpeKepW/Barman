using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    GameObject cap;
    ParticleSystem particle;
    public float TimeCapacity = 3f;
    float currentCapacity = 0f;
    void Start()
    {
        cap = transform.Find("Cap").gameObject;
        particle = transform.Find("Water").gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        outPour();
    }

    void outPour()
    {
        if (!cap.gameObject.activeSelf && currentCapacity/300 <= TimeCapacity)
        {
           // if (Mathf.Abs(transform.localEulerAngles.z) >= 90 || Mathf.Abs(transform.localEulerAngles.x) >= 90)
           if(!(transform.localEulerAngles.z < 90 || transform.localEulerAngles.z > 275)|| !(transform.localEulerAngles.x < 110 || transform.localEulerAngles.x > 250)) //|| transform.localEulerAngles.x < -90 && transform.localEulerAngles.x > 90)
            {
                
                if (!particle.isEmitting)
                    particle.Play();
                currentCapacity += Time.fixedTime;
            }
            else if (particle.isEmitting)
                particle.Stop();
        }
        else if (particle.isEmitting)
            particle.Stop();
    }
}
