using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Monster : MonoBehaviour
{
    public bool setHurt = false;
    public int ChaseMode = 0;
    int AttackMode = 0;
    public Transform ShootPoint;
    public GameObject bullet;
    public Transform target;
    public float target_ATK = 10;
    public float speed = 1;
    float originSpeed;
    public float AutoChaseRadio = 2.5f;
    float AutoHitRadio = 2.0f;

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
    bool attack1CoolTimeFlg = true;
    int attackCoolTime = 0;
    float attackCoolTimer = 0.0f;
    public float maxAttackCoolTime = 2.0f;

    bool hurtCoolTimeFlg = false;
    int hurtCoolTime = 0;
    float hurtCoolTimer = 0.0f;
    public float maxHurtCoolTime = 2.0f;

    float walkCoolTimer = 0.0f;

    public GameObject hitEffect;
    public GameObject hurtEffect;
    public GameObject dieEffect;
    public GameObject dust;
    public Sound[] sounds = null;

    //Initialization
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        originSpeed = speed;

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

        switch (ChaseMode){
            case 0:
                AutoHitRadio = 1.5f;
                AttackMode = 0;
                break;
            case 1:
                AutoHitRadio = 3.0f;
                AttackMode = 1;
                break;
            case 2:
                AutoHitRadio = 3.0f;
                AttackMode = 2;
                break;
            case 3:
                AutoHitRadio = 3.0f;
                AttackMode = 3;
                break;
            case 4:
                AutoHitRadio = 1.5f;
                AttackMode = 0;
                break;
            case 5:
                AutoHitRadio = 1.0f;
                AttackMode = 0;
                break;
            case 6:
                AutoHitRadio = 1.5f;
                AttackMode = 0;
                break;
            default:
                break;
        }
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
        walkCoolTimer += Time.fixedDeltaTime;
        attackCoolTimer += Time.fixedDeltaTime;
        hurtCoolTimer += Time.fixedDeltaTime;
        
        if (!attackCoolTimeFlg && attackCoolTimer > maxAttackCoolTime)
        {
            //Debug.Log("!attackCoolTimeFlg && attackCoolTimer > maxAttackCoolTime, attackCoolTimer= " + attackCoolTimer);
            attackCoolTimeFlg = true;
        }
        
        if (hurtCoolTimeFlg && hurtCoolTimer > maxHurtCoolTime)
        {
            //Debug.Log("!attackCoolTimeFlg && attackCoolTimer > maxAttackCoolTime, hurtCoolTimer= " + hurtCoolTimer);
            stateChange("Walk");
            hurtCoolTimeFlg = false;
        }

        if (walkCoolTimer >= 5.0f)
        {
            SoundPlay("Walk");
            walkCoolTimer = 0.0f;
        }
    }

    void traceTarget()
    {
        if (dieFlg) return;
        if (hurtCoolTimeFlg || !attackCoolTimeFlg) return;

        //Tracing directly when close enough
        //Debug.Log("(target.position - transform.position).magnitude= " + (target.position - transform.position).magnitude);
        if ((target.position - transform.position).magnitude < AutoChaseRadio && !attackFlg)
        {
            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 targ = target.position - transform.position;
            targ.y = 0.0f;
            moveDirection = targ;

            //Debug.Log("Tracing directly, " + moveDirection.normalized * speed  * Time.deltaTime);
            Rotate(face, 0.1f);
            //rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed  * Time.deltaTime);
            //monsterMove(transform.position + moveDirection.normalized * moveDirectionLen * speed  * Time.deltaTime);
            monsterMove(transform.position + moveDirection.normalized * speed  * Time.deltaTime);
        }
        else//A_star
        {
            //Debug.Log("A_star timer= " + timer % 3 + ", startFind= "+ startFind);
            if (timer % 3 < 1 && startFind)
            {
                //Debug.Log("A_star, " + transform.name);
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
        //Debug.Log("hurtCoolTimeFlg= " + hurtCoolTimeFlg);
        if (dieFlg) return;
        if (hurtCoolTimeFlg) return;

        float targetRadius = target.GetComponent<CapsuleCollider>().radius;
        Vector3 distence = target.position - transform.position;
        //Debug.Log("distence= "+ distence.magnitude + ", AutoHitRadio= " + AutoHitRadio);
        //if (distence.magnitude < targetRadius * 3)
        if (distence.magnitude < AutoHitRadio)
        {
            //Debug.Log("distence.magnitude < AutoHitRadio");
            //if(!attackCoolTimeFlg) return;
            StopCoroutine("FollowPath");
            monsterAttack(target.position);
            hitplayerSuccessful = true;
            attackFlg = true;

            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            Rotate(face, 0.1f);
        }
        else
        {
            hitplayerSuccessful = false;
            attackFlg = false;
        }
    }

    void monsterMove(Vector3 postion)
    {
        if (hurtCoolTimeFlg) return;

        //Debug.Log("monsterMove, hurtCoolTimeFlg, postion= " + postion);
        stateChange("Walk");
        rigidbody.MovePosition(postion);
    }

    void monsterAttack(Vector3 postion)
    {
        if (!attackCoolTimeFlg) return;

        //Debug.Log("monsterAttack, !attackCoolTimeFlg");
        //SoundPlay("Attack");
        //StartCoroutine(updateParticlesystem("Hit", postion));

        switch (AttackMode)
        {
            case 0:
                stateChange("Attack0");
                break;
            case 1:
                stateChange("Attack1");
                break;
            case 2:
                stateChange("Attack1");
                break;
            case 3:
                stateChange("Attack1");
                break;
            default:
                break;
        }
        shoot();
        attackCoolTimeFlg = false;
    }
    void shoot()
    {
        if (AttackMode == 0)
        {
            //bullet.SetActive(true);
            bullet.GetComponent<Collider>().enabled = true;
            return;
        }
        Rigidbody bulletRb;
        bulletRb = Instantiate(bullet, ShootPoint.position,Quaternion.identity).GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * AutoHitRadio, ForceMode.Impulse);
        bulletRb.AddForce(transform.up * 4.0f, ForceMode.Impulse);

        bulletRb.transform.SetParent(transform);
        SoundPlay("Throw");
    }

    void monsterDie()//monster be killed
    {
        if (dieFlg) return;

        dieFlg = true;
        FindObjectOfType<PlayerInfoMgr>().ModifyLevel(2.3f);//Player level up
        //transform.GetComponent<Collider>().enabled = false;
        //transform.GetChild(0).GetChild(0).GetComponent<Collider>().enabled = false;
        StopCoroutine("FollowPath");
        //SoundPlay("Die");
        stateChange("Die");
        Destroy(transform.gameObject, 3.0f);
    }

    public void monsterAttackSound()
    {
        SoundPlay("Attack");
    }
    public void monsterDieSound()
    {
        SoundPlay("Die");
    }

    public void setMonsterSpeed(float speedFactor)
    {
        speed = originSpeed * speedFactor;
    }

    //Change sound effect and particle system
    void OnCollisionEnter(Collision collision)
    {
        if (dieFlg) return;

        if (collision.gameObject.tag == "Ground" && ChaseMode == 0)
        {
            SoundPlay("Walk");
            StartCoroutine(updateParticlesystem("Dust", transform.position));
        }
        
        /*if (collision.gameObject.tag == "Player" && AttackMode == 0)
        {
            //Debug.Log("attackCoolTimeFlg= " + attackCoolTimeFlg);
            //Debug.Log("attack1CoolTimeFlg= " + attack1CoolTimeFlg);
            if (hurtCoolTimeFlg) return;
            if (!attack1CoolTimeFlg) return;
            attack1CoolTimeFlg = false;

            Debug.Log("SoundPlay(Attack);");
            SoundPlay("Attack");
            Vector3 hitPoint = collision.GetContact(0).point;
            StartCoroutine(updateParticlesystem("Hit", hitPoint));
        }*/
    }

    /*void OnCollisionExit(Collision collision)
    {
        rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //Debug.Log("player Exit gameObject");
        if (collision.gameObject.tag == "player")
        {
            //Debug.Log("player Exit");
            hitplayerSuccessful = false;
        }
    }*/

    //Animation changing
    void stateChange(String state)
    {
        //Debug.Log("stateChange= "+ state);
        switch (state)
        {
            case "Walk":
                animator.SetBool("Walk", true);
                break;
            case "Attack0":
                animator.SetBool("Idle", true);
                animator.SetTrigger("Attack");
                animator.SetBool("Walk", false);

                attack1CoolTimeFlg = true;
                attackCoolTimer = 0.0f;
                break;
            case "Attack1":
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);

                attackCoolTimer = 0.0f;
                break;
            case "Hurt":
                animator.SetTrigger("Hurt");
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
                break;
            case "Die":
                animator.SetTrigger("Die");
                break;
            case "Idle":
                break;
            default:
                break;
        }
    }

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
        if (!setHurt) return;
        if (other.name[0] == 'M')
        {
            Health -= target_ATK;
            
            if (Health > 0)
            {
                Vector3 moving = transform.position - moveDirection.normalized * moveDirectionLen * (target_ATK) * speed  * Time.deltaTime;
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
            monsterDie();
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

    //Start of A star-----------------------------------------------------------------------------------------------------
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && !hitplayerSuccessful)// && newPath.Length != 0
        {
            //Debug.Log("OnPathFound, " + transform.name);
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        //Debug.Log("FollowPath, " + transform.name);
        if (path.Length == 0)
        {
            stateChange("Idle");
            yield break;
        }

        Vector3 currentWaypoint = path[0];
        while (true)
        {
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
                        //rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed  * Time.deltaTime);
                        monsterMove(transform.position + moveDirection.normalized * moveDirectionLen * speed  * Time.deltaTime);
                        yield return null;
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            moveDirection = currentWaypoint - transform.position;
            moveDirectionLen = (currentWaypoint - transform.position).magnitude;
            Rotate(currentWaypoint, 0.1f);
            //rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed  * Time.deltaTime);//speed  * Time.deltaTime
            monsterMove(transform.position + moveDirection.normalized * moveDirectionLen * speed  * Time.deltaTime);

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

}