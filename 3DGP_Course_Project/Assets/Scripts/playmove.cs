using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playmove : MonoBehaviour
{
    public float speed = 1;

    public float jumpforce = 1;

    Rigidbody rigidbody;
    CharacterController characterController;
    //float speed = 3.0f; // 移動速度
    float rotate = 180.0f; // 旋轉速度
    float jumpH = 9.0f; // 跳躍高度
    float gravity = 20.0f; // 重力
    Vector3 moveDirection = Vector3.zero;


    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetKeyDown("space")) { moveDirection.y = jumpH; }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        /*//Move rigidbody.MovePosition
        if (Input.GetKey(KeyCode.W))
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            rigidbody.MovePosition(Vector3.forward + transform.position);
        if (Input.GetKey(KeyCode.S))
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
            rigidbody.MovePosition(Vector3.back + transform.position);
        if (Input.GetKey(KeyCode.A))
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            rigidbody.MovePosition(Vector3.left + transform.position);
        if (Input.GetKey(KeyCode.D))
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            rigidbody.MovePosition(Vector3.right + transform.position);
        if (Input.GetKeyDown(KeyCode.Space))
            rigidbody.AddForce(Vector3.up * jumpforce);*/
    }
}
