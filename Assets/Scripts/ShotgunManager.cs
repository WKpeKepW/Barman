using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunManager : MonoBehaviour
{
    List<MeshCollider> MyColliders;
    void Start()
    {
        MyColliders = new List<MeshCollider>();
        MyColliders.AddRange(GetComponents<MeshCollider>());
    }
    public void CollidersOn(bool enabled)
    {
        foreach (MeshCollider collider in MyColliders)
        {
            collider.enabled = enabled;
        }
    }
}
