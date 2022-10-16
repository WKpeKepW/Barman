using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    GameObject spawn;
    LineRenderer Lr;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        Lr = GetComponent<LineRenderer>();
        spawn = transform.Find("SpawnBullet").gameObject;
    }
    private void Update()
    {
        Physics.Raycast(spawn.transform.position, spawn.transform.TransformDirection(0, 0, 100), out hit);
        Lr.SetPosition(0, spawn.transform.position);
        Lr.SetPosition(1, hit.point);
    }
    public void ActionShoot()
    {
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(spawn.transform.TransformDirection(0,0,100), ForceMode.Impulse);
       
    }    
}
