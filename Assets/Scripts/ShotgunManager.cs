using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShotgunManager : MonoBehaviour
{
    public List<MeshCollider> MyColliders;
    Shoot sh;
    public bool isActive = false;
    public int ammo = 2;
    void Start()
    {
        sh = GetComponent<Shoot>();
        MyColliders = new List<MeshCollider>();
        MyColliders.AddRange(GetComponents<MeshCollider>());
    }

    public void CollidersOn(bool enabled)
    {
        foreach (MeshCollider collider in MyColliders)
        {
            collider.enabled = enabled;
        }
        sh.SetActive();
    }
    public void SendFire()
    {
        if (ammo != 0)
        {
            sh.ActionShoot();
            ammo--;
        }
    }
    public void Reload()
    {
        ammo++;
    }
    
}
