using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] float speed = 4000;
    [SerializeField] float sensetive = 1;
    float _mouseX, _mouseY;
    [SerializeField] Transform CamTransform;
    [SerializeField] Vector3 CamOffset;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        TrueMove();
    }
    void LateUpdate()
    {
        CameraFirstPerson();
    }
    void TrueMove()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity *= speed * Time.deltaTime;
        Vector3 velocityChange = targetVelocity - transform.InverseTransformDirection(_rb.velocity);
        _rb.AddRelativeForce(velocityChange);
    }
    void CameraFirstPerson()
    {
        _mouseX += Input.GetAxis("Mouse X") * sensetive;
        _mouseY += Input.GetAxis("Mouse Y") * sensetive;
        _mouseY = Mathf.Clamp(_mouseY, -90, 90);
        CamTransform.localEulerAngles = new Vector3(-_mouseY, _mouseX);
        transform.localEulerAngles = new Vector3(0, _mouseX);
        CamTransform.position = transform.position + CamOffset;
    }
}
