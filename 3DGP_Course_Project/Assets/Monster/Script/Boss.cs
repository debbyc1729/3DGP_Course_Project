using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class Boss : MonoBehaviour
{
    public bool setHurt = false;
    public Transform ShootPoint;
    public GameObject bullet;
    public Transform target;
    public float target_ATK = 10;
    public float speed = 1;
    public float AutoChaseRadio = 10.0f;
    float AutoHitRadio = 3.5f;

    public bool isStop = true;
    public bool isInvulnerable = false;
    public float Health;
    public float LowHealth;
    public float MixHealth;
    public GameObject HealthBar;
    public Slider slider;
    public GameObject SmashObject;
    public TextMeshProUGUI HPnumber;
    
    Animator animator;
    bool hitplayerSuccessful = false;
    Rigidbody rigidbody;

    float timer = 0;
    Vector3 moveDirection = Vector3.zero;

    bool attackFlg = false;
    bool dieFlg = false;
    bool angerFlg = false;

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
    public GameObject TornadoEffect;

    //Initialization
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
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
            s.source.maxDistance = 50.0f;
            s.source.minDistance = 0.0f;
        }

        Health = MixHealth;
        slider.value = ComputeHealth();
    }

    void Update()
    {
        //if (Health <= 0) return;
        timer += Time.deltaTime;

        if (isStop)
            return;

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
            if(angerFlg)
                SoundPlay("Run");
            else
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

            //Debug.Log("Tracing directly, " + moveDirection.normalized * speed * Time.deltaTime);
            //Rotate(face, 0.1f);
            //rigidbody.MovePosition(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
            //monsterMove(transform.position + moveDirection.normalized * moveDirectionLen * speed * Time.deltaTime);
            monsterMove(transform.position + moveDirection.normalized * speed * Time.deltaTime);
        }
        if((target.position - transform.position).magnitude >= AutoChaseRadio)
        {
            stateChange("Idle");
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
            //StopCoroutine("FollowPath");
            monsterAttack(target.position);
            hitplayerSuccessful = true;
            attackFlg = true;

            Vector3 face = new Vector3(target.position.x, transform.position.y, target.position.z);
            //Rotate(face, 0.1f);
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
        stateChange("Attack0");
        //shoot();
        attackCoolTimeFlg = false;
    }
    void shoot()
    {
        bullet.SetActive(true);
    }

    void monsterDie()//monster be killed
    {
        if (dieFlg) return;

        dieFlg = true;
        FindObjectOfType<PlayerInfoMgr>().ModifyLevel(2.3f);
        //transform.GetComponent<Collider>().enabled = false;
        //transform.GetChild(0).GetChild(0).GetComponent<Collider>().enabled = false;
        //StopCoroutine("FollowPath");
        //SoundPlay("Die");//"Die roar"
        stateChange("Die");
        HealthBar.SetActive(false);
        //Destroy(transform.gameObject, 10.0f);
    }
    public void bossRoar()
    {
        SoundPlay("Roar");
    }

    public void bossWalk()
    {
        SoundPlay("Walk");
    }
    public void bossRun()
    {
        SoundPlay("Run");
    }

    public void bossAngerAttack()
    {
        SoundPlay("AngerAttack");
    }
    public void bossAttack()
    {
        SoundPlay("Attack");
    }

    public void bossHurt()
    {
        SoundPlay("Hurt");
    }
    public void bossAngerHurt()
    {
        SoundPlay("AngerHurt");
    }

    public void bossAnger()
    {
        SoundPlay("Anger");
    }
    public void bossDie()
    {
        //SoundPlay("Die");
        FindObjectOfType<MonsterAttaclAudio>().Play("BossDie");
    }

    public void setBossSpeed(float speedFactor)
    {
        speed = speed * speedFactor;
    }

    //Change sound effect and particle system
    void OnCollisionEnter(Collision collision)
    {
        if (dieFlg) return;
        
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
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", true);
                break;
            case "Attack0":
                animator.SetTrigger("Attack");

                attack1CoolTimeFlg = true;
                attackCoolTimer = 0.0f;
                break;
            case "Anger":
                animator.SetBool("Anger", true);
                angerFlg = true;
                break;
            case "Hurt":
                animator.SetTrigger("Hurt");
                break;
            case "Die":
                animator.SetTrigger("Die");
                break;
            case "Idle":
                animator.SetBool("Idle", true);
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

    public void showHPbar()
    {
        HealthBar.SetActive(true);
    }
    public void setSmash()
    {
        SmashObject.SetActive(true);
    }
    public void OpenSmashCollider()
    {
        SmashObject.GetComponent<Collider>().enabled = true;
    }
    public void CloseSmashCollider()
    {
        SmashObject.GetComponent<Collider>().enabled = false;
    }

    public void setHit()
    {
        bullet.SetActive(true);
    }
    public void OpenHitCollider()
    {
        bullet.GetComponent<Collider>().enabled = true;
    }
    public void CloseHitCollider()
    {
        bullet.GetComponent<Collider>().enabled = false;
    }

    public void showTornado()
    {
        Debug.Log("showTornado");
        GameObject newTornadoEffect;
        newTornadoEffect = Instantiate(TornadoEffect, transform.position, transform.rotation);
        newTornadoEffect.transform.SetParent(transform);

        FindObjectOfType<MonsterAttaclAudio>().Play("Tornado");
    }


    public void DieParticlesystem()
    {
        //Debug.Log("DieParticlesystem");
        //SoundPlay("Die");
        StartCoroutine(updateParticlesystem("Die", transform.position));
        Destroy(transform.gameObject, 0.5f);
    }
    /*public void destroyBoss()
    {
        Destroy(transform.gameObject, 1.0f);
    }*/

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
                //Vector3 moving = transform.position - moveDirection.normalized * (target_ATK) * speed * Time.deltaTime;
                //rigidbody.MovePosition(moving);
                //SoundPlay("Hurt");
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
                GameObject die = Instantiate(dieEffect, position, transform.rotation);
                //die.transform.GetChild(0).localScale = new Vector3(2.0f,2.0f,2.0f);
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
        if (isInvulnerable)
            return;
        slider.value = ComputeHealth();
        HPnumber.text = Health.ToString();
        if (Health < MixHealth && Health > 0)
        {
            HealthBar.SetActive(true);
        }

        if (Health <= LowHealth)
        {
            speed = 1;
            AutoHitRadio = 5.0f;
            stateChange("Anger");
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
    public void Rotate(Vector3 targetPos, float fRotateSpeed)
    {
        Vector3 targetDir = targetPos - transform.position;
        if (targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fRotateSpeed);
        }
    }
    //End of A star-----------------------------------------------------------------------------------------------------

}
