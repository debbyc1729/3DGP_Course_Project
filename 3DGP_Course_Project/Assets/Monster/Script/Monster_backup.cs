using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Monster_backup : MonoBehaviour
{
    public Transform target;
    public float target_ATK = 10;
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

    bool attackFlg = false;
    bool dieFlg = false;

    bool attackCoolTimeFlg = true;
    int attackCoolTime = 0;
    float attackCoolTimer = 0.0f;
    public float maxAttackCoolTime = 2.0f;

    bool hurtCoolTimeFlg = false;
    int hurtCoolTime = 0;
    float hurtCoolTimer = 0.0f;
    public float maxHurtCoolTime = 2.0f;

    public GameObject hitEffect;
    public GameObject hurtEffect;
    public GameObject dieEffect;
    public GameObject dust;
    public Sound[] sounds = null;

    //Initialization
    void Start()
    {
        target = GameObject.FindWithTag("player").transform;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.rolloffMode = AudioRolloffMode.Linear;
            s.source.spatialBlend = 1.0f;
            s.source.maxDistance = 10.0f;
            s.source.minDistance = 0.0f;
        }

        Health = MixHealth;
        slider.value = ComputeHealth();
    }

    void Update()
    {
        //if (Health <= 0) return;
        timer += Time.deltaTime;

        UpdateHealth();
        checkHitPlayer();

        traceTarget();

    }

    private void FixedUpdate()
    {
        attackCoolTimer += Time.fixedDeltaTime;
        hurtCoolTimer += Time.fixedDeltaTime;

        //Debug.Log("attackCoolTimer= " + attackCoolTimer + " attackCoolTimeFlg= "+ attackCoolTimeFlg);
        if (!attackCoolTimeFlg && attackCoolTimer > maxAttackCoolTime)
        {
            //Debug.Log("attackCoolTime end, " + transform.name);
            attackCoolTimeFlg = true;
        }

        //Debug.Log("hurtCoolTimer= " + transform.name);
        if (hurtCoolTimeFlg && hurtCoolTimer > maxHurtCoolTime)
        {
            //Debug.Log("attackCoolTime end, " + transform.name);
            stateChange("Walk");
            hurtCoolTimeFlg = false;
        }
    }

    //Start of Health Bar-------------------------------------------------------------------------------------------
    void UpdateHealth()
    {
        slider.value = ComputeHealth();
        if (Health < MixHealth)
        {
            HealthBar.SetActive(true);
        }

        if (Health <= 0)
        {
            die();
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
    //End of Health Bar--------------------------------------------------------------------------------

    void traceTarget()
    {
        if (dieFlg) return;
        //if (hurtCoolTimeFlg) return;
        //Debug.Log("traceTarget, " + transform.name);

        //Tracing directly when close enough
        if ((target.position - transform.position).magnitude < AutoChaseRadio && !attackFlg)
        {
            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 targ = target.position - transform.position;
            targ.y = 0.0f;
            moveDirection = targ;

            Rotate(face, 0.1f);
            //rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
            moveMonster(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
        }
        else//A_star
        {
            if (timer % 3 < 1 && startFind)
            {
                targetPositionTemp = target.position;
                hitplayerSuccessful = false;
                startFind = false;
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            }
            if (timer % 3 >= 1)
            {
                startFind = true;
            }
        }
    }

    void checkHitPlayer()
    {
        if (dieFlg) return;

        float targetRadius = target.GetComponent<CapsuleCollider>().radius;
        Vector3 distence = target.position - transform.position;
        //Debug.Log("distence.magnitude= " + distence.magnitude + ", targetRadius * 1.4= " + targetRadius * 1.1);
        if (distence.magnitude < targetRadius * 1.15)
        {
            //if(!attackCoolTimeFlg) return;
            //Debug.Log("distence.magnitude < targetRadius * 0.5");
            StopCoroutine("FollowPath");
            //stateChange("Attack On");
            monsterAttack(target.position);
            hitplayerSuccessful = true;
            attackFlg = true;

            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            Rotate(face, 0.1f);
        }
        else
        {
            //stateChange("Walk");
            //stateChange("Attack Off");
            hitplayerSuccessful = false;
            attackFlg = false;
        }
    }

    void die()//monster be killed
    {
        if (dieFlg) return;

        dieFlg = true;
        transform.GetComponent<SphereCollider>().enabled = false;
        transform.GetChild(0).GetChild(0).GetComponent<SphereCollider>().enabled = false;
        StopCoroutine("FollowPath");
        SoundPlay("Die");
        stateChange("Die");
        Destroy(transform.gameObject, 3.0f);
    }

    //Change sound effect and particle system
    void OnCollisionEnter(Collision collision)
    {
        if (dieFlg) return;

        if (collision.gameObject.tag == "Ground")
        {
            SoundPlay("Walk");
            StartCoroutine(updateParticlesystem("Dust", transform.position));
        }

        if (collision.gameObject.tag == "player")
        {
            SoundPlay("Attack");
            Vector3 hitPoint = collision.GetContact(0).point;
            StartCoroutine(updateParticlesystem("Hit", hitPoint));
            //monsterAttack(hitPoint);
        }
    }

    /*void OnCollisionExit(Collision collision)
    {
        rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //Debug.Log("player Exit gameObject");
        if (collision.gameObject.tag == "player")
        {
            //Debug.Log("player Exit");
            animator.SetBool("Attack", false);
            hitplayerSuccessful = false;
        }
    }*/

    //Play sound effect
    public void SoundPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    //Start of Particle system-----------------------------------------------------------------------------------------------------
    //Hit by fire ball
    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log(transform.name+ ", Health= " + Health);
        if (other.name[0] == 'M')
        {
            Health -= target_ATK;

            //StopCoroutine("FollowPath");
            //rigidbody.MovePosition(transform.position - moveDirection.normalized * moveDirectionLen * (target_ATK % 10) * speed * Time.deltaTime);

            if (Health > 0)
            {
                Vector3 moving = transform.position - moveDirection.normalized * moveDirectionLen * (target_ATK) * speed * Time.deltaTime;
                //Debug.Log(transform.name + " move " + moving.magnitude);
                rigidbody.MovePosition(moving);
                SoundPlay("Hurt");
                stateChange("Hurt");

                hurtCoolTimeFlg = true;
                hurtCoolTimer = 0.0f;
            }
        }
    }

    IEnumerator updateParticlesystem(String type, Vector3 position)
    {
        switch (type)
        {
            case "Dust":
                Instantiate(dust, position, transform.rotation);
                break;
            case "Hit":
                Instantiate(hitEffect, position, transform.rotation);
                break;
            case "Hurt":
                //Instantiate(hurtEffect, position, transform.rotation);
                break;
            case "Die":
                //Instantiate(dieEffect, position, transform.rotation);
                break;
            default:
                break;
        }
        yield return null;
    }
    //End of Particle system-----------------------------------------------------------------------------------------------------

    //Animation changing
    void stateChange(String state)
    {
        switch (state)
        {
            case "Walk":
                animator.SetBool("Walk", true);
                animator.SetBool("Attack", false);
                break;
            case "Attack":
                //Debug.Log("Attack");
                if (attackCoolTimeFlg)
                {
                    animator.SetBool("Idle", true);
                    animator.SetBool("Attack", true);
                    attackCoolTimeFlg = false;
                    attackCoolTimer = 0.0f;
                    animator.SetBool("Walk", false);
                }
                break;
            case "Hurt":
                //animator.SetBool("Hurt", true);
                //animator.SetBool("Hurt", false);
                animator.SetTrigger("test");
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
                break;
            case "Die":
                //animator.SetBool("Walk", false);
                //animator.SetBool("Attack", false);

                animator.SetTrigger("Die");
                break;
            case "Idle":
                break;
            default:
                break;
        }
    }

    //Start of A star-----------------------------------------------------------------------------------------------------
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && !hitplayerSuccessful)// && newPath.Length != 0
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length == 0)
        {
            stateChange("Idle");
            yield break;
        }

        Vector3 currentWaypoint = path[0];
        //stateChange("Walk");
        while (true)
        {
            //Debug.Log("FollowPath, " + transform.name);

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
                        //rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
                        moveMonster(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
                        yield return null;
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            moveDirection = currentWaypoint - transform.position;
            moveDirectionLen = (currentWaypoint - transform.position).magnitude;
            Rotate(currentWaypoint, 0.1f);
            //rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);//speed * Time.deltaTime
            moveMonster(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);

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
    //End of A star-----------------------------------------------------------------------------------------------------

    void moveMonster(Vector3 postion)
    {
        if (hurtCoolTimeFlg) return;

        stateChange("Walk");
        rigidbody.MovePosition(postion);
    }


    void monsterAttack(Vector3 postion)
    {
        //Debug.Log("monsterAttack");
        if (!attackCoolTimeFlg) return;

        //SoundPlay("Attack");
        //StartCoroutine(updateParticlesystem("Hit", postion));
        stateChange("Attack");
        attackCoolTimeFlg = false;
    }
}