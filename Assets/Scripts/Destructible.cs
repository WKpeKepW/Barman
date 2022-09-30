using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject parts;
    Rigidbody body;
    bool isbroken = false;
    public float magnitudeDestroy = 3;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (body.velocity.magnitude > magnitudeDestroy && !isbroken)
        {
            isbroken = true;
            destroy();
        }
    }
    void destroy()
    {
        Destroy(gameObject);
        GameObject obj = Instantiate(parts, transform.position,transform.rotation);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddForce(body.velocity, ForceMode.Impulse);
        }
    }
}
