using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    GameObject spawn;
    Transform cam;
    List<GameObject> Bullets = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spawn = transform.Find("SpawnBullet").gameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Bullets.Count >= 5)
        {
            Destroy(Bullets[0]);
            Bullets.RemoveAt(0);
        }
    }
    public void ActionShoot()
    {
        Bullets.Add(Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity));
        Bullets[Bullets.Count - 1].GetComponent<Rigidbody>().AddForce(cam.TransformDirection(0, 0, 100), ForceMode.Impulse);
    }    
}
