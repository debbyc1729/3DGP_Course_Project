using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{


    public Transform target;
    public float speed = 1;
    public float AutoChaseRadio = 1.0f;
    
    public float Health;
    public float MixHealth;
    public GameObject HealthBar;
    public Slider slider;

    Vector3[] path;
    int targetIndex;
    Animator animator;
    bool hitplayerSuccessful = false;
    Vector3 targetPositionTemp;
    Vector3 currentWaypointTemp;
    Rigidbody rigidbody;
    
    float timer = 0;
    bool startFind = true;
    Vector3 moveDirection = Vector3.zero;
    float moveDirectionLen = 0.0f;

    void Start()
    {
        target = GameObject.FindWithTag("player").transform;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        Health = MixHealth;
        slider.value = ComputeHealth();
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Update()
    {
        //Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        //rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * speed);
        UpdateHealth();

        timer += Time.deltaTime;
        
        //Debug.Log(transform.name + " (target.position - transform.position).magnitude= " + (target.position - transform.position).magnitude);
        if ((target.position - transform.position).magnitude < AutoChaseRadio)
        {
            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 targ = target.position - transform.position;
            targ.y = 0.0f;
            //Debug.Log(transform.name + " face= " + face);

            moveDirection = targ;

            Rotate(face, 0.1f);
            rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
            
        }
        else
        {
            //Debug.Log(transform.name + " startFind= " + startFind);
            if (timer % 3 < 1 && startFind)
            {
                targetPositionTemp = target.position;
                hitplayerSuccessful = false;
                startFind = false;
                //Debug.Log("timer % 5 == 0");
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            }
            if (timer % 3 >= 1)
            {
                //Debug.Log("else");
                startFind = true;
            }
        }


            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData, Mathf.Infinity);
            //Debug.Log("hitData.point= " + hitData.point);
            if (targetPositionTemp != hitData.point)
            {
                targetPositionTemp = hitData.point;
                //Node playerNode = null;
                //if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitData, Mathf.Infinity))
                //{
                    //playerNode = NodeFromWorldPoint(hitData.point);
                    //Debug.Log("hitData.point= " + hitData.point);
                    //FindPath(seeker.position, hitData.point);
                    //if (hitData.transform.gameObject.layer != 6)
                    PathRequestManager.RequestPath(transform.position, hitData.point, OnPathFound);
                //}
            }*/
        }

    void OnCollisionEnter(Collision collision)
    {
        rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //Debug.Log("OnCollisionEnter");
        if (collision.gameObject.tag == "player")
        {
            //Debug.Log("OnCollisionEnter player");
            StopCoroutine("FollowPath");
            animator.SetBool("Attack", true);
            hitplayerSuccessful = true;

            Vector3 l = collision.transform.position - transform.position;
            collision.transform.position = Vector3.MoveTowards(collision.transform.position, collision.transform.position + l, speed * Time.deltaTime);
        }
        
    }

    void OnCollisionExit(Collision collision)
    {
        rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //Debug.Log("player Exit gameObject");
        if (collision.gameObject.tag == "player")
        {
            //Debug.Log("player Exit");
            animator.SetBool("Attack", false);
            hitplayerSuccessful = false;
        }
    }
    
    /*void OnTriggerEnter(Collider other)//??????????????????????????????????
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.tag == "player")
        {
            Debug.Log("player hit");
            //animator.SetBool("Eat", false);
            animator.SetBool("Attack", true);
            //animator.SetBool("Walk", false);
            hitplayerSuccessful = true;
            //Debug.Log("hit hitplayerSuccessful= " + hitplayerSuccessful);
            //Destroy(gameObject);
        }
    }
    void OnTriggerExit(Collider other)//??????????????????????????????????
    {
        //Debug.Log("player Exit gameObject");
        if (other.gameObject.tag == "player")
        {
            Debug.Log("player Exit");
            //animator.SetBool("Eat", false);
            animator.SetBool("Attack", false);
            //animator.SetBool("Walk", false);
            hitplayerSuccessful = false;
            //Debug.Log("Exit hitplayerSuccessful= " + hitplayerSuccessful);
        }
    }*/

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && !hitplayerSuccessful)// && newPath.Length != 0
        {
            //Debug.Log("FollowNewPath");
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        //Debug.Log("FollowPath");
        //Debug.Log(transform.gameObject.name + " path len= " + path.Length);
        if (path.Length == 0)
        {
            animator.SetBool("Walk", false);
            //animator.SetBool("Attack", true);
            yield break;
        }

        Vector3 currentWaypoint = path[0];

        while (true)
        {
            animator.SetBool("Walk", true);
            if (Vector3.Distance(currentWaypoint, transform.position) <= 0.7f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    while ((target.position - transform.position).magnitude >= AutoChaseRadio)
                    {
                        Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
                        Vector3 targ = target.position - transform.position;
                        targ.y = 0.0f;

                        moveDirection = targ;
                        Rotate(face, 0.1f);
                        rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
                        //Debug.Log(transform.name + " targetIndex >= path.Length moveDirectionLen= " + moveDirectionLen);
                        yield return null;
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            
            moveDirection = currentWaypoint - transform.position;
            moveDirectionLen = (currentWaypoint - transform.position).magnitude;
            Rotate(currentWaypoint, 0.1f);
            //Debug.Log(transform.name + " face= " + currentWaypoint);
            rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);//speed * Time.deltaTime
            
            
            yield return null;

        }
    }
    public void Rotate(Vector3 targetPos, float fRotateSpeed)
    {
        Vector3 targetDir = targetPos - transform.position;
        if (targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fRotateSpeed);
        }
    }
    float Distance(Vector3 PosA, Vector3 PosB)
    {
        float dstX = PosA.x - PosB.x;
        float dstZ = PosA.z - PosB.z;
        
        return dstX * dstX + dstZ * dstZ;
    }

    //Health Bar-------------------------------------------------------------------------------------------
    void UpdateHealth() {
        slider.value = ComputeHealth();
        if (Health < MixHealth)
        {
            HealthBar.SetActive(true);
        }

        if (Health <= 0)
        {
            //monster be killed
            Destroy(transform.gameObject);
        }

        if (Health > MixHealth)
        {
            Health = MixHealth;
        }
    }

    float ComputeHealth()
    {
        return Health / MixHealth;
    }
}