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
    CharacterController _characterController;
    bool isCrouch=false;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //TrueMove();
        CharacterControll();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    void LateUpdate()
    {
        CameraFirstPerson();
    }

    void CharacterControll()
    {
        _characterController.Move(transform.TransformDirection(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime));
    }
    void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            speed = speed / 2;
            transform.position = new Vector3(transform.position.x, transform.position.y / 2, transform.position.z);
        }
        if (!isCrouch)
        {
            speed = speed * 2;
            transform.position = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);
        }
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
