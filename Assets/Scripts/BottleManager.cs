using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BottleManager : MonoBehaviour
{
    GameObject cap;
    ParticleSystem particle;
    public float TimeCapacity = 3f;
    float currentCapacity = 0f;
    public bool isEmpty = true;
    void Start()
    {
        FindAndOpen();
    }
    public void FindAndOpen(ParticleSystem ps = null)
    {
        cap = transform.Find("Cap").gameObject;
        if (ps != null)
        {
            particle = ps;
            isEmpty = false;
        }
        else
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag == "fluid")
                {
                    particle = transform.GetChild(i).GetComponent<ParticleSystem>();
                    isEmpty = false;
                    break;
                }
            }
        currentCapacity = 0;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if(!isEmpty)
            outPour();
        if (currentCapacity >= TimeCapacity && !isEmpty)
            DestroyFluid();
    }

    public void DestroyFluid()
    {
        isEmpty = true;
        Destroy(particle.gameObject);
    }
    void outPour()
    {
        if (!cap.gameObject.activeSelf)
        {
           // if (Mathf.Abs(transform.localEulerAngles.z) >= 90 || Mathf.Abs(transform.localEulerAngles.x) >= 90)
           if(!(transform.localEulerAngles.z < 90 || transform.localEulerAngles.z > 275)|| !(transform.localEulerAngles.x < 110 || transform.localEulerAngles.x > 250)) //|| transform.localEulerAngles.x < -90 && transform.localEulerAngles.x > 90)
            {
                
                if (!particle.isEmitting)
                    particle.Play();
                currentCapacity += Time.deltaTime;
            }
            else if (particle.isEmitting)
                particle.Stop();
        }
        else if (particle.isEmitting)
            particle.Stop();
    }
}
