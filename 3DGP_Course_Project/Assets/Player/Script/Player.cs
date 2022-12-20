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

    GameObject fire;
    GameObject weapon;

    bool activateMoving;
    SkillMgr skillMgr;
    // PortalMgr portalMgr;
    Coroutine recordCoroutine;
    PlayerInfoMgr infoMgr;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        GetComponent<Rigidbody>().constraints = 
            // RigidbodyConstraints.FreezePositionY | 
            RigidbodyConstraints.FreezeRotation;

        walkSpeed = 2f;
        rotateSpeed = 1000f;
        cameraOffset = new Vector3(0f, 0.8f, -0.15f);
        anima = GetComponent<Animator>();
        pitch = 0f;
        target = FindObjectOfType<Crosshair>();
        flgPickGem = false;
        scaleRate = 0f;
        activateMoving = true;
        skillMgr = FindObjectOfType<SkillMgr>();
        infoMgr = FindObjectOfType<PlayerInfoMgr>();
        // portalMgr = FindObjectOfType<PortalMgr>();

        UpdateMainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        anima.SetBool("run", false);
        xMouseMove = yMouseMove = 0f;

        if (activateMoving)
        {
            UpdatePosition();
            UpdateRotation();
            PickGem();
        }

        UpdateMainCamera();
    }

    void UpdatePosition()
    {
        bool flgMove = false;

        if (Input.GetKey(KeyCode.W))
        {
            flgMove = true;
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            flgMove = true;
            transform.Translate(Vector3.back * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            flgMove = true;
            transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            flgMove = true;
            transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);
        }
        if (flgMove)
        {
            anima.SetBool("run", true);
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
            float distance = 1f * Vector3.Distance(Camera.main.transform.position, target.gem.transform.position);

            if (distance > 4.5f)
            {
                return;
            }

            flgPickGem = true;
            target.hitGem = false;
            scaleRate = 0f;
            Rigidbody rb = target.gem.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.detectCollisions = false;

            float throwingAngle = 30f;
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * throwingAngle * Mathf.Deg2Rad));
            float Vx = velocity * Mathf.Cos(throwingAngle * Mathf.Deg2Rad);
            float Vy = velocity * Mathf.Sin(throwingAngle * Mathf.Deg2Rad);
            Vector3 V0 = new Vector3(0, Vy, Vx);
            rb.velocity = Quaternion.LookRotation(-target.ray.direction) * V0;

            float during = 2 * velocity * Mathf.Sin(throwingAngle * Mathf.Deg2Rad) / Physics.gravity.magnitude;
            Destroy(target.gem, during);
        }

        if (flgPickGem && target.gem != null)
        {
            if (scaleRate < Time.deltaTime * 0.2f)
            {
                scaleRate += Time.deltaTime * 0.01f;
            }

            target.gem.transform.localScale -= Vector3.one * scaleRate;

            if (target.gem.transform.localScale.magnitude <= 0.01f)
            {
                flgPickGem = false;
            }
        }
    }

    public void ActivateMoving(bool value)
    {
        activateMoving = value;
    }

    public void setWalkSpeedFactor(float factor)
    {
        walkSpeed = 2f * factor;
        // anima.SetFloat("speed", factor);
    }

    public void Fly(float force = 500f, float delay = 0f)
    {
        StartCoroutine(FlyCoroutine(force, delay));
    }

    IEnumerator FlyCoroutine(float force = 500f, float delay = 0f)
    {
        // anima.SetInteger("jump", 1);
        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 acceleration = Vector3.up * Time.deltaTime * 2f;

        while (GetComponent<Rigidbody>().velocity.y < force)
        {
            GetComponent<Rigidbody>().velocity += acceleration;
            yield return null;
        }
        while (GetComponent<Rigidbody>().velocity.y > 0)
        {
            GetComponent<Rigidbody>().velocity -= acceleration;
            yield return null;
        }
        SetFloating(true, 0f);
        yield break;
    }

    public void SetFloating(bool toFloat = false, float delay = 0f)
    {
        if (toFloat)
        {
            recordCoroutine = StartCoroutine(FloatingCoroutine(delay));
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().useGravity = true;

            if (recordCoroutine != null)
            {
                StopCoroutine(recordCoroutine);
                // anima.SetInteger("jump", 2);
            }
        }
    }

    IEnumerator FloatingCoroutine(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 acceleration = Vector3.up * Time.deltaTime * -0.5f;

        while (true)
        {
            if (GetComponent<Rigidbody>().velocity.y < -1f || GetComponent<Rigidbody>().velocity.y > 1f)
            {
                acceleration = -acceleration;
            }
            GetComponent<Rigidbody>().velocity += acceleration;
            yield return null;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "monsterBullet")
        {
            infoMgr.ModifyHp(-0.1f);
        }
    }
}
