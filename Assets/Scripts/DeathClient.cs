using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathClient : MonoBehaviour
{
    delegate void d_DeathClinet();
    d_DeathClinet death;
    private void Start()
    {
        if (transform.GetComponent<ClientMove>() != null)
            death = transform.GetComponent<ClientMove>().Death;
        if (transform.GetComponent<DialogsRuner>() != null)
            death = transform.GetComponent<DialogsRuner>().Death;
    }
    public void Death()
    {
        death();
    }
}
