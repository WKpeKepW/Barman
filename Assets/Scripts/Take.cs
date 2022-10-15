using UnityEngine;

public class Take : MonoBehaviour
{
    public float distansTake = 0.5f;
    public float modifyHandDistans = 0.6f;
    GameObject Cam, L_hand, R_hand, R_item, L_item;
    bool isL_HandBusy = false, isR_HandBusy = false;
    bool isModifyHandon = false;
    bool isBottle_L_Open = false, isBottle_R_Open = false;
    bool isGunHand = false;
    void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
        L_hand = Cam.transform.Find("L_hand").gameObject;
        R_hand = Cam.transform.Find("R_hand").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isGunHand)
            PickUp(ref isL_HandBusy, L_hand, ref L_item);
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isGunHand)
            R_item.GetComponent<Shoot>().ActionShoot();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            PickUp(ref isR_HandBusy, R_hand, ref R_item);
        if (Input.GetKeyDown(KeyCode.R))
            PourModify();
        if (Input.GetKeyDown(KeyCode.T))
            OpenBottle();
    }
    void FixedUpdate()
    {
        //RotateItem(ref L_item);
        if (!isModifyHandon)
        {
            if (Input.GetKey(KeyCode.E))
                RotateItem(ref R_item);
            if (Input.GetKey(KeyCode.Q))
                RotateItem(ref L_item);
        }
        else
        {
            if (Input.GetKey(KeyCode.E))
                RotateItem(false);
            if (Input.GetKey(KeyCode.Q))
                RotateItem(true);
        }
    }
    void PickUp(ref bool handbusy, GameObject hand, ref GameObject item)
    {
        RaycastHit hit;
        if (handbusy)
            Drop(ref handbusy, ref item);

        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, distansTake, LayerMask.GetMask("Items")))
        {
            
            if (hit.transform.gameObject.tag == "Gun")
            {
                R_item = hit.transform.gameObject;
                R_item.transform.parent = R_hand.transform;
                R_item.GetComponent<Rigidbody>().isKinematic = true;
                R_item.GetComponent<ShotgunManager>().CollidersOn(false);
                R_item.transform.localPosition = Vector3.zero;
                R_item.transform.localRotation = Quaternion.identity;
                isR_HandBusy = true;
                isGunHand = true;
            }
            else
            {
                item = hit.transform.gameObject;
                item.transform.parent = hand.transform;
                item.GetComponent<Rigidbody>().isKinematic = true;
                //if(item.GetComponent<Collider>() != null)
                item.GetComponent<Collider>().enabled = false;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                handbusy = true;
            }
            
        }
    }
    void Drop(ref bool handbusy, ref GameObject item)
    {
        item.transform.parent = null;
        if (isGunHand)
        {
            item.GetComponent<ShotgunManager>().CollidersOn(true);
            isGunHand = false;
        }
        else
            item.GetComponent<Collider>().enabled = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        handbusy = false;
        if (!isModifyHandon)
            item.GetComponent<Rigidbody>().AddForce(Cam.transform.TransformDirection(0, 0, 6), ForceMode.Impulse);// для кидания бутылок
        item = null;
    }
    void RotateItem(ref GameObject item)
    {
        if (item != null)
            item.transform.Rotate(new Vector3(1, 0, 0), 18f);
    }
    void RotateItem(bool left = false)
    {
        if (!left)
        {
            if (R_item != null)
                R_item.transform.Rotate(new Vector3(0, 0, 1), 3f);
            if (L_item != null)
                L_item.transform.Rotate(new Vector3(0, 0, -1), 3f);
        }
        else
        {
            if (R_item != null)
                R_item.transform.Rotate(new Vector3(0, 0, -1), 3f);
            if (L_item != null)
                L_item.transform.Rotate(new Vector3(0, 0, 1), 3f);
        }
    }
    void PourModify()
    {
        if (isModifyHandon)
        {
            L_hand.transform.localPosition += new Vector3(0, 0, -modifyHandDistans);
            R_hand.transform.localPosition += new Vector3(0, 0, -modifyHandDistans);
        }
        else
        {
            L_hand.transform.localPosition += new Vector3(0, 0, modifyHandDistans);
            R_hand.transform.localPosition += new Vector3(0, 0, modifyHandDistans);
        }
        if (R_item != null)
            R_item.transform.localRotation = Quaternion.identity;
        if (L_item != null)
            L_item.transform.localRotation = Quaternion.identity;
        isModifyHandon = !isModifyHandon;
    }
    void OpenBottle()
    {
        if (L_item != null)
        {
            if (!isBottle_L_Open)
                L_item.transform.Find("Cap").gameObject.SetActive(false);
            else
                L_item.transform.Find("Cap").gameObject.SetActive(true);
            isBottle_L_Open = !isBottle_L_Open;
        }
        if (R_item != null)
        {
            if (!isBottle_R_Open)
                R_item.transform.Find("Cap").gameObject.SetActive(false);
            else
                R_item.transform.Find("Cap").gameObject.SetActive(true);
            isBottle_R_Open = !isBottle_R_Open;
        }
    }
}
