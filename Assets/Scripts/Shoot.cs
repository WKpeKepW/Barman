using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab, crosshairpefab;
    GameObject spawn, crosshair;
    RaycastHit hit;
    public bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        spawn = transform.Find("SpawnBullet").gameObject;
        crosshair = Instantiate(crosshairpefab,transform.position,Quaternion.identity);
        crosshair.SetActive(isActive);
    }
    private void Update()
    {
        Physics.Raycast(spawn.transform.position, spawn.transform.TransformDirection(0, 0, 100), out hit);
        if (isActive)
            VisibleCrossHair();
    }
    public void ActionShoot()
    {
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(spawn.transform.TransformDirection(0, 0, 100), ForceMode.Impulse);
        if (hit.rigidbody != null)
        {
            Vector3 velocity = spawn.transform.TransformDirection(Random.Range(0, 3), Random.Range(0, 3), 30);
            hit.transform.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            if (hit.transform.GetComponent<Destructible>() != null)
                hit.transform.GetComponent<Destructible>().destroyItem(velocity);
        }
        if (hit.transform.tag == "Client")
            hit.transform.GetComponent<DeathClient>().Death();
    }
    void VisibleCrossHair()
    {
        if (hit.point != null)
            crosshair.transform.position = hit.point;
    }
    public void SetActive()
    {
        isActive = !isActive;
        crosshair.SetActive(isActive);
    } 
}
