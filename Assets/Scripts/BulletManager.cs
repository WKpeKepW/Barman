using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    Rigidbody rb;
    RaycastHit hit;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(0, 0, 100), out hit))
        {
            if (hit.transform.GetComponent<Rigidbody>() != null)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(rb.velocity, ForceMode.Impulse);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
