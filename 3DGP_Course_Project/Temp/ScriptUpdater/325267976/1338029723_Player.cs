using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float walkSpeed;
    float rotateSpeed;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Vector3 cameraOffset;
    Animator anima;
    float xMove;
    float yMove;
    float pitch;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezePositionY | 
            RigidbodyConstraints.FreezeRotationX | 
            RigidbodyConstraints.FreezeRotationZ;

        walkSpeed = 2f;
        rotateSpeed = 1000f;
        cameraOffset = new Vector3(0f, 0.7f, 0f);
        anima = GetComponent<Animator>();
        pitch = 0f;

        UpdateMainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        xMove = yMove = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);
        }
        if ((xMove = Input.GetAxis("Mouse X")) != 0 || (yMove = Input.GetAxis("Mouse Y")) != 0)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime * xMove, 0f);
            pitch -= rotateSpeed * Time.deltaTime * yMove;

            if (pitch < -30f)
            {
                pitch = -30f;
            }
            else if (pitch > 30f)
            {
                pitch = 30f;
            }
        }

        UpdateMainCamera();
    }

    void OnCollisionEnter(Collision collider)
    {
        GetComponent<Rigidbody>().rotation = Quaternion.identity;
    }

    void UpdateMainCamera()
    {
        mainCamera.transform.position = transform.position;
        mainCamera.transform.rotation = transform.rotation;
        mainCamera.transform.Translate(cameraOffset);
        mainCamera.transform.Rotate(pitch, 0f, 0f);
    }
}
