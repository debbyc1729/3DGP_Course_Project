using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float walkSpeed;
    float rotateSpeed;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        walkSpeed = 2f;
        rotateSpeed = 1000f;
        cameraOffset = new Vector3(0f, 1.5f, -1f);
        mainCamera.transform.position = transform.position + cameraOffset;
        mainCamera.transform.rotation = transform.rotation;
        mainCamera.transform.Rotate(10f, 0f, 0f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
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
            Rotate(-90f);
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rotate(90f);
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        UpdateMainCamera();
    }

    void Rotate(float angle)
    {
        Quaternion from = transform.rotation;
        transform.Rotate(0f, angle, 0f);
        Quaternion to = transform.rotation;

        transform.rotation = Quaternion.Slerp(from, to, Time.deltaTime);
    }

    void UpdateMainCamera()
    {
        mainCamera.transform.position = transform.position;
        mainCamera.transform.rotation = transform.rotation;
        mainCamera.transform.Translate(Vector3.back);
        mainCamera.transform.Translate(0, 1.5f, 0);
        mainCamera.transform.Rotate(45f, 0f, 0f);
    }
}
