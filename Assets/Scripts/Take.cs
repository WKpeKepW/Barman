using UnityEngine;

public class Take : MonoBehaviour
{
    public float DebugFloat;
    public float distansTake = 0.5f;
    public float modifyHandDistans = 0.6f;
    GameObject Cam, L_hand, R_hand, R_item, L_item;
    bool isL_HandBusy = false, isR_HandBusy = false;
    bool isModifyHandon = false;
    bool isBottle_L_Open = false, isBottle_R_Open = false;
    bool isGunHand, isReloading = false;
    Vector3 oldPosition;//говно код дробовика
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
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isReloading)
            PickUp(ref isL_HandBusy, L_hand, ref L_item);
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isGunHand && !isReloading)//говно код дробовика
            R_item.GetComponent<ShotgunManager>().SendFire();//говно код дробовика
        if (Input.GetKeyDown(KeyCode.R) && R_item != null)//говно код дробовика
        {
            isReloading = !isReloading;//говно код дробовика
            GameObject obj = R_item.transform.Find("TOZ-B.012").gameObject;//говно код дробовика
            if (isReloading)//говно код дробовика
            {
                oldPosition = obj.transform.localPosition;//говно код дробовика
                obj.transform.Rotate(29.92f, 0, 0);//говно код дробовика
                obj.transform.position = obj.transform.TransformPoint(0, -0.1f, 0.05f);//говно код дробовика
            }
            else//говно код дробовика
            {
                obj.transform.Rotate(-29.92f, 0, 0);//говно код дробовика//говно код дробовика
                obj.transform.localPosition = oldPosition;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
            PickUp(ref isR_HandBusy, R_hand, ref R_item);
        if (Input.GetKeyDown(KeyCode.Tab))
            PourModify();
        if (Input.GetKeyDown(KeyCode.CapsLock))
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

            if (hit.transform.gameObject.tag == "Gun")//говно код дробовика
            {
                R_item = hit.transform.gameObject;//говно код дробовика
                R_item.transform.parent = R_hand.transform;//говно код дробовика
                R_item.GetComponent<Rigidbody>().isKinematic = true;//говно код дробовика
                R_item.GetComponent<ShotgunManager>().CollidersOn(false);//говно код дробовика
                R_item.transform.localPosition = Vector3.zero;//говно код дробовика
                R_item.transform.localRotation = Quaternion.identity;//говно код дробовика
                isR_HandBusy = true;//говно код дробовика
                isGunHand = true;//говно код дробовика
            }
            else if (hit.transform.tag == "Ammo" && isReloading && R_item != null)//говно код дробовика
            {//говно код дробовика
                if (R_item.GetComponent<ShotgunManager>().ammo == 2)//говно код дробовика
                    return;//говно код дробовика
                Destroy(hit.transform.gameObject);//говно код дробовика
                R_item.GetComponent<ShotgunManager>().Reload();//говно код дробовика
            }
            else if(!isReloading)//говно код дробовика
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
        if (isGunHand)//говно код дробовика
        {
            item.GetComponent<ShotgunManager>().CollidersOn(true);//говно код дробовика
            isGunHand = false;//говно код дробовика
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
                L_item.transform.Find("Cap")?.gameObject.SetActive(false);
            else
                L_item.transform.Find("Cap")?.gameObject.SetActive(true);
            isBottle_L_Open = !isBottle_L_Open;
        }
        if (R_item != null)
        {
            if (!isBottle_R_Open)
                R_item.transform.Find("Cap")?.gameObject.SetActive(false);
            else
                R_item.transform.Find("Cap")?.gameObject.SetActive(true);
            isBottle_R_Open = !isBottle_R_Open;
        }
    }
}
