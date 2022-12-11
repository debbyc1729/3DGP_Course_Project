using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillMgr : MonoBehaviour
{
    public Skill[] skills;

    GameObject skillOnWeapon;
    GameObject player;
    Player playerScript;
    Animator anima;
    Transform weapon;
    Skill currentSkill;
    bool flgShowSkill;

    // Start is called before the first frame update
    void Awake()
    {
        skillOnWeapon = null;
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<Player>();
        anima = player.GetComponent<Animator>();
        weapon = player.transform.Find("root").Find("pelvis").Find("Weapon");
        flgShowSkill = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Alpha1))
        {
            UseSkill("Fire");
        }
        if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Alpha2))
        {
            UseSkill("SpeedUp");
        }
        if (Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Alpha3))
        {
            UseSkill("SlowDown");
        }
        if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.Alpha4))
        {
            UseSkill("Fly");
        }
    }

    void UseSkill(string name, float delay = 0f)
    {
        if (flgShowSkill)
        {
            return;
        }
        flgShowSkill = true;
        SelectSkill(name);
        StartCoroutine(UseSkillCoroutine(delay));
    }

    void SelectSkill(string name)
    {
        currentSkill = Array.Find(skills, skill => skill.name == name);
    }

    IEnumerator UseSkillCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        anima.SetBool("attack", true);
        ShowSkillOnWeapon();
        yield return new WaitForSeconds(1f);
        anima.SetBool("attack", false);
        yield return new WaitForSeconds(0.5f);
        ShowSkill();
        InvokeSkill(0f);
        yield return new WaitForSeconds(currentSkill.duration);
        flgShowSkill = false;
        yield break;
    }

    void ShowSkillOnWeapon()
    {
        GameObject skillPrefab = currentSkill.prefab[0];
        skillPrefab.transform.position = player.transform.position;
        skillPrefab.transform.rotation = player.transform.rotation;
        skillPrefab.transform.Translate(0.3f, 0.7f, 0.5f);
        skillOnWeapon = Instantiate(skillPrefab, skillPrefab.transform.position, skillPrefab.transform.rotation);
        Vector3 scale = skillOnWeapon.transform.localScale;
        skillOnWeapon.transform.parent = weapon;
        skillOnWeapon.transform.localScale = scale;
    }

    void ShowSkill()
    {
        if (skillOnWeapon != null)
        {
            Destroy(skillOnWeapon, 0f);
        }
        GameObject skillPrefab = currentSkill.prefab[1];
        skillPrefab.transform.position = player.transform.position;
        skillPrefab.transform.rotation = player.transform.rotation;
        skillPrefab.transform.Translate(currentSkill.offset);
        GameObject skill = Instantiate(skillPrefab, skillPrefab.transform.position, skillPrefab.transform.rotation);
        // skill.transform.parent = transform;
        Destroy(skill, currentSkill.lifeTime);
    }

    void InvokeSkill(float delay)
    {
        if (currentSkill.name == "Fire")
        {
            StartCoroutine(Fire(delay));
        }
        if (currentSkill.name == "SpeedUp")
        {
            StartCoroutine(SpeedUp(delay));
        }
        if (currentSkill.name == "SlowDown")
        {
            StartCoroutine(SlowDown(delay));
        }
        if (currentSkill.name == "Fly")
        {
            StartCoroutine(Fly(delay));
        }
    }

    IEnumerator Fire(float delay)
    {
        yield break;
    }

    IEnumerator SpeedUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerScript.setWalkSpeedFactor(3f);
        yield return new WaitForSeconds(currentSkill.duration);
        playerScript.setWalkSpeedFactor(1f);
        yield break;
    }

    IEnumerator SlowDown(float delay)
    {
        yield break;
    }

    IEnumerator Fly(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerScript.Fly(2.5f, 0.2f);
        // playerScript.SetFloating(true, 1f);
        yield return new WaitForSeconds(currentSkill.duration);
        playerScript.SetFloating(false, 0f);
        yield break;
    }
}
