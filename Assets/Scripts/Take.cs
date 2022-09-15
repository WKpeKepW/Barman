using UnityEngine;

public class Take : MonoBehaviour
{
    public float distans = 0.5f;
    GameObject Cam, L_hand, R_hand, R_item,L_item;
    bool isL_HandBusy = false;
    bool isR_HandBusy = false;
    // Start is called before the first frame update
    void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
        L_hand = Cam.transform.Find("L_hand").gameObject;
        R_hand = Cam.transform.Find("R_hand").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            PickUp(ref isL_HandBusy, L_hand,ref L_item);
        if (Input.GetKeyDown(KeyCode.Mouse1))
            PickUp(ref isR_HandBusy, R_hand,ref R_item);
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E))
            RotateItem(ref R_item);
        if (Input.GetKey(KeyCode.Q))
            RotateItem(ref L_item);
    }
    void PickUp(ref bool handbusy, GameObject hand, ref GameObject item)
    {
        RaycastHit hit;
        if (handbusy)
            Drop(ref handbusy,ref item);//,hit);
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, distans, LayerMask.GetMask("Items")))
        {
            item = hit.transform.gameObject;
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.transform.parent = hand.transform;
            item.GetComponent<Collider>().enabled = false;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            handbusy = true;
        }
    }
    void Drop(ref bool handbusy,ref GameObject item)//, RaycastHit hit)
    {
        item.transform.parent = null;
        //RaycastHit hit;
        //if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, distans, ~LayerMask.GetMask("Ignore Raycast")))// пока хуйня
        //{
        //    print(hit.transform.tag);
        //    item.transform.position = hit.transform.position;// + new Vector3(,item.transform.localScale.y / 2,0);
        //}
        item.GetComponent<Collider>().enabled = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        handbusy = false;
        item = null;
    }
    void RotateItem(ref GameObject item)
    {
        if(item != null)
            item.transform.Rotate(new Vector3(1, 0, 0), 18f);
    }

}
