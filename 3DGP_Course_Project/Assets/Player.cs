using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float walkSpeed;
    float rotateSpeed;
    Vector3 cameraOffset;
    Animator anima;
    float xMouseMove;
    float yMouseMove;
    float pitch;
    Crosshair target;
    
    bool flgPickGem;
    float scaleRate;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezePositionY | 
            RigidbodyConstraints.FreezeRotation;

        walkSpeed = 2f;
        rotateSpeed = 1000f;
        cameraOffset = new Vector3(0f, 0.7f, -0.03f);
        anima = GetComponent<Animator>();
        pitch = 0f;
        target = FindObjectOfType<Crosshair>();
        flgPickGem = false;
        scaleRate = 0f;

        UpdateMainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        xMouseMove = yMouseMove = 0f;

        UpdatePosition();
        UpdateRotation();
        PickGem();

        UpdateMainCamera();
    }

    void UpdatePosition()
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
            transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);
        }
    }

    void UpdateRotation()
    {
        if ((xMouseMove = Input.GetAxis("Mouse X")) != 0 || (yMouseMove = Input.GetAxis("Mouse Y")) != 0)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime * xMouseMove, 0f);
            pitch -= 2 * rotateSpeed * Time.deltaTime * yMouseMove;

            if (pitch < -60f)
            {
                pitch = -60f;
            }
            else if (pitch > 60f)
            {
                pitch = 60f;
            }
        }
    }

    void UpdateMainCamera()
    {
        Camera.main.transform.position = transform.position;
        Camera.main.transform.rotation = transform.rotation;
        Camera.main.transform.Translate(cameraOffset);
        Camera.main.transform.Rotate(pitch, 0f, 0f);
    }

    void PickGem()
    {
        if (target.hitGem)
        {
            flgPickGem = true;
            target.hitGem = false;
            scaleRate = 0f;
            target.gem.GetComponent<Collider>().enabled = false;

            float distance = 1.3f * Vector3.Distance(transform.position, target.gem.transform.position);
            float throwingAngle = 35f;
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * throwingAngle * Mathf.Deg2Rad));
            float Vx = velocity * Mathf.Cos(throwingAngle * Mathf.Deg2Rad);
            float Vy = velocity * Mathf.Sin(throwingAngle * Mathf.Deg2Rad);
            Vector3 V0 = new Vector3(0, Vy, Vx);
            target.gem.GetComponent<Rigidbody>().velocity = Quaternion.LookRotation(transform.position - target.gem.transform.position) * V0;

            float during = 2 * velocity * Mathf.Sin(throwingAngle * Mathf.Deg2Rad) / Physics.gravity.magnitude;
            Destroy(target.gem, during);
        }

        if (flgPickGem && target.gem != null)
        {
            if (scaleRate > Time.deltaTime * -0.3f)
            {
                scaleRate += Time.deltaTime * -0.03f;
            }

            target.gem.transform.localScale += Vector3.one * scaleRate;

            if (target.gem.transform.localScale.x <= scaleRate ||
                target.gem.transform.localScale.y <= scaleRate ||
                target.gem.transform.localScale.z <= scaleRate)
            {
                flgPickGem = false;
            }
        }
    }
}
