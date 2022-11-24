using UnityEngine;
using System.Collections;

public class Unit_backup : MonoBehaviour
{


    public Transform target;
    public float speed = 1;
    public float AutoChaseRadio = 1.0f;
    Vector3[] path;
    int targetIndex;
    Animator animator;
    bool hitplayerSuccessful = false;
    Vector3 targetPositionTemp;
    Vector3 currentWaypointTemp;
    Rigidbody rigidbody;
    CharacterController characterController;
    bool hitPlayer = false;
    float timer = 0;
    bool startFind = true;
    Vector3 leaveDistanceAB = Vector3.zero;
    Vector3 DistanceAB = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    float moveDirectionLen = 0.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Update()
    {
        //Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        //rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * speed);


        timer += Time.deltaTime;

        Debug.Log("hitPlayer= " + hitPlayer);
        Debug.Log(transform.name + " (target.position - transform.position).magnitude= " + (target.position - transform.position).magnitude);
        if ((target.position - transform.position).magnitude < AutoChaseRadio)// && !hitPlayer
        {
            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 targ = target.position - transform.position;
            targ.y = 0.0f;
            Debug.Log(transform.name + " face= " + face);

            moveDirection = targ;

            Rotate(face, 0.1f);
            rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);

            /*Debug.Log("(target.position - transform.position).magnitude < 1 && !hitPlayer");

            Vector3 targ = new Vector3(target.position.x + leaveDistanceAB.x+ DistanceAB.x/2, transform.position.y, target.position.z + leaveDistanceAB.z+ DistanceAB.z/2);

            //這裡會重疊
            Debug.Log("targ= "+ targ);

            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            Debug.Log(transform.name + " face= " + face);

            Rotate(face, 0.1f);
            transform.position = Vector3.MoveTowards(transform.position, targ, speed * Time.deltaTime);*/
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
            Debug.Log("OnCollisionEnter player");
            StopCoroutine("FollowPath");
            hitPlayer = true;
            //Rotate(target.position, 0.01f);
            //rigidbody.AddForce((target.position - transform.position) * speed);//
            //Debug.Log("player hit");
            animator.SetBool("Attack", true);
            hitplayerSuccessful = true;
        }
        
    }

    void OnCollisionExit(Collision collision)
    {
        rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //Debug.Log("player Exit gameObject");
        if (collision.gameObject.tag == "player")
        {
            hitPlayer = false;
            //Debug.Log("player Exit");
            animator.SetBool("Attack", false);
            hitplayerSuccessful = false;
        }
    }
    
    /*void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay");
        //if (collision.gameObject.tag != "player")
        //{
        Debug.Log("monster hit");

        //if (collision.gameObject.GetInstanceID() < transform.GetInstanceID())
        //{
        Vector3 PosA = transform.position;
        Vector3 PosB = collision.gameObject.transform.position;

        //Vector3 rA = transform.GetComponent<Collision>().GetContact(0).point - transform.position;
        //Vector3 rB = collision.GetContact(0).point - collision.gameObject.transform.position;

        DistanceAB = PosA - PosB;
        DistanceAB.y = 0;
        Vector3 v = DistanceAB / DistanceAB.magnitude;


        Debug.Log("collision.GetContact(0).normal= " + collision.GetContact(0).normal);
        Vector3 normal = collision.GetContact(0).normal;
        normal.y = 0;
        leaveDistanceAB = normal * DistanceAB.magnitude / 2;
        Debug.Log("leaveDistanceAB= " + leaveDistanceAB);
            //}
        //}
    }*/

    void OnTriggerEnter(Collider other)//??????????????????????????????????
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
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        //Debug.Log("OnPathFound");
        //Debug.Log("pathSuccessful= " + pathSuccessful);
        //Debug.Log("hitplayerSuccessful= " + hitplayerSuccessful);
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
        Debug.Log(transform.gameObject.name + " path len= " + path.Length);
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
            //Debug.Log("currentWaypoint= " + currentWaypoint + " transform.position= " + transform.position);
            //Debug.Log("Vector3.Distance(currentWaypoint, target.position)= " + Vector3.Distance(currentWaypoint, transform.position));
            //Debug.Log("leaveDistanceAB.magnitude= " + leaveDistanceAB.magnitude);
            //if (transform.position == currentWaypoint)
            //if (Distance(currentWaypoint, target.position) > Distance(transform.position, target.position))
            //if (Vector3.Distance(currentWaypoint, transform.position) <= leaveDistanceAB.magnitude*2)
            if (Vector3.Distance(currentWaypoint, transform.position) <= 0.7f)
            {
                //Debug.Log("if " + Vector3.Distance(currentWaypoint, target.position));
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    while ((target.position - transform.position).magnitude >= AutoChaseRadio)
                    {
                        //Debug.Log("targetIndex >= path.Length " + Vector3.Distance(target.position, transform.position));
                        Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
                        Vector3 targ = target.position - transform.position;
                        targ.y = 0.0f;

                        moveDirection = targ;
                        Rotate(face, 0.1f);
                        rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
                        Debug.Log(transform.name + " targetIndex >= path.Length moveDirectionLen= " + moveDirectionLen);
                        //transform.position = Vector3.MoveTowards(transform.position, target.position + leaveDistanceAB, speed * Time.deltaTime);
                        yield return null;
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            //if (hitPlayer)
            //{
            //    yield break;
            //}
            moveDirection = currentWaypoint - transform.position;
            moveDirectionLen = (currentWaypoint - transform.position).magnitude;
            Rotate(currentWaypoint + leaveDistanceAB, 0.1f);
            Debug.Log(transform.name + " face= " + currentWaypoint + leaveDistanceAB);
            //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint+ leaveDistanceAB, speed * Time.deltaTime);
            rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);//speed * Time.deltaTime

            //Debug.Log("rigidbody.MovePosition, currentWaypoint= " + currentWaypoint + " transform.position= " + transform.position);
            //rigidbody.MovePosition(transform.position + (currentWaypoint - transform.position) * speed * Time.deltaTime);//speed * Time.deltaTime
            //rigidbody.AddForce((currentWaypoint - transform.position) * speed * Time.deltaTime);//
            
            yield return null;

        }
        //yield return new WaitForSeconds(4);
    }
    public void Rotate(Vector3 targetPos, float fRotateSpeed)
    {
        Vector3 targetDir = targetPos - transform.position;
        if (targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, fRotateSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fRotateSpeed);
        }
    }
    float Distance(Vector3 PosA, Vector3 PosB)
    {
        float dstX = PosA.x - PosB.x;
        float dstZ = PosA.z - PosB.z;

        //return Mathf.Sqrt(dstX * dstX + dstZ * dstZ);
        return dstX * dstX + dstZ * dstZ;
    }

    //-----------------------------------------------------------------------------------------------
    /*public float[] actionWeight = { 3000, 3000, 4000 };
    private float lastActTime;
    private Quaternion targetRotation;
    void RandomAction()
    {
        //更新行動時間
        lastActTime = Time.time;
        //根據權重隨機
        float number = Random.Range(0, actionWeight[0] + actionWeight[1] + actionWeight[2]);
        if (number <= actionWeight[0])
        {
            currentState = MonsterState.STAND;
            animator.SetTrigger("Stand");
        }
        else if (actionWeight[0] < number && number <= actionWeight[0] + actionWeight[1])
        {
            currentState = MonsterState.CHECK;
            animator.SetTrigger("Check");
        }
        if (actionWeight[0] + actionWeight[1] < number && number <= actionWeight[0] + actionWeight[1] + actionWeight[2])
        {
            currentState = MonsterState.WALK;
            //隨機一個朝向
            targetRotation = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
            animator.SetTrigger("Walk");
        }
    }

    private float diatanceToPlayer;
    public float alertRadius;
    public float attackRange;
    void ChaseRadiusCheck()
    {
        diatanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        //diatanceToInitial = Vector3.Distance(transform.position, initialPosition);

        if (diatanceToPlayer < attackRange)
        {
            //SceneManager.LoadScene("Battle");
            currentState = MonsterState.CHASE;
        }
        
        if (diatanceToPlayer > alertRadius)//diatanceToInitial > chaseRadius || 
        {
            currentState = MonsterState.RETURN;
        }
    }*/

    /*public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }*/
}