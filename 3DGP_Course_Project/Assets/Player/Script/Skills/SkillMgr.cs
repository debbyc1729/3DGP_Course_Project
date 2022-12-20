using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    PlayerInfoMgr infoMgr;
    bool flgShowSkill;

    // Start is called before the first frame update
    void Awake()
    {
        skillOnWeapon = null;
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<Player>();
        anima = player.GetComponent<Animator>();
        weapon = player.transform.Find("root").Find("pelvis").Find("Weapon");
        infoMgr = FindObjectOfType<PlayerInfoMgr>();
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

    void UseSkill(string name)
    {
        Skill s = Array.Find(skills, skill => skill.name == name);

        if (s == null)
        {
            return;
        }
        if (flgShowSkill || s.flgCooldown)
        {
            return;
        }
        if (!infoMgr.ModifyMp(-s.duration * 0.05f))
        {
            return;
        }
        flgShowSkill = true;
        StartCoroutine(UseSkillCoroutine(s));
    }

    IEnumerator UseSkillCoroutine(Skill s)
    {
        anima.SetBool("attack", true);
        ShowSkillOnWeapon(s);
        yield return new WaitForSeconds(0.8f);
        anima.SetBool("attack", false);
        ShowSkill(s);
        InvokeSkill(s, 0f);
        FindObjectOfType<BuffMgr>().InvokeBuff(s.name, s.duration);
        CoolDown(s);
        yield return new WaitForSeconds(0.2f);
        flgShowSkill = false;
        yield break;
    }

    void ShowSkillOnWeapon(Skill s)
    {
        GameObject skillPrefab = s.ps.prefab[0];
        skillPrefab.transform.position = player.transform.position;
        skillPrefab.transform.rotation = player.transform.rotation;
        skillPrefab.transform.Translate(0.3f, 0.7f, 0.5f);
        skillOnWeapon = Instantiate(skillPrefab, skillPrefab.transform.position, skillPrefab.transform.rotation);
        Vector3 scale = skillOnWeapon.transform.localScale;
        skillOnWeapon.transform.parent = weapon;
        skillOnWeapon.transform.localScale = scale;
    }

    void ShowSkill(Skill s)
    {
        if (skillOnWeapon != null)
        {
            Destroy(skillOnWeapon, 0f);
        }
        GameObject skillPrefab = s.ps.prefab[1];
        if (s.isDirectional)
        {
            skillPrefab.transform.position = Camera.main.transform.position;
            skillPrefab.transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            skillPrefab.transform.position = player.transform.position;
            skillPrefab.transform.rotation = player.transform.rotation;
        }
        skillPrefab.transform.Translate(s.ps.offset);
        GameObject skill = Instantiate(skillPrefab, skillPrefab.transform.position, skillPrefab.transform.rotation);
        Destroy(skill, s.ps.lifeTime);
    }

    void InvokeSkill(Skill s, float delay)
    {
        if (s.effect != null)
        {
            StopCoroutine(s.effect);
        }
        if (s.name == "Fire")
        {
            s.effect = StartCoroutine(Fire(s, delay));
        }
        if (s.name == "SpeedUp")
        {
            s.effect = StartCoroutine(SpeedUp(s, delay));
        }
        if (s.name == "SlowDown")
        {
            s.effect = StartCoroutine(SlowDown(s, delay));
        }
        if (s.name == "Fly")
        {
            s.effect = StartCoroutine(Fly(s, delay));
        }
    }

    void CoolDown(Skill s)
    {
        if (!s.flgCooldown)
        {
            StartCoroutine(CoolDownCoroutine(s));
        }
    }

    IEnumerator CoolDownCoroutine(Skill s)
    {
        s.flgCooldown = true;
        Image skillImage = s.IconImage;
        float timer = 0f;
        while (skillImage.fillAmount > 0f)
        {
            timer += Time.deltaTime;
            skillImage.fillAmount = Mathf.Lerp(1f, 0f, timer / s.cooldown);
            yield return null;
        }

        skillImage.fillAmount = 1f;
        s.flgCooldown = false;
        yield break;
    }

    IEnumerator Fire(Skill s, float delay)
    {
        yield break;
    }

    IEnumerator SpeedUp(Skill s, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerScript.setWalkSpeedFactor(3f);
        yield return new WaitForSeconds(s.duration);
        playerScript.setWalkSpeedFactor(1f);
        yield break;
    }

    IEnumerator SlowDown(Skill s, float delay)
    {
        yield break;
    }

    IEnumerator Fly(Skill s, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerScript.Fly(2.5f, 0.2f);
        // playerScript.SetFloating(true, 1f);
        yield return new WaitForSeconds(s.duration);
        playerScript.SetFloating(false, 0f);
        yield break;
    }
}
