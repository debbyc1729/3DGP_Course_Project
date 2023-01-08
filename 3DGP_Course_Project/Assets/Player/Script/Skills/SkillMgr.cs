using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillMgr : MonoBehaviour
{
    [SerializeField] GameObject skillUI;
    public Skill[] skills;

    Transform GainSkillDialog;
    Image dialogIcon;
    Button acceptBtn;
    GameObject skillOnWeapon;
    GameObject player;
    Player playerScript;
    Animator anima;
    Transform weapon;
    Skill currentSkill;
    PlayerInfoMgr infoMgr;
    bool flgShowSkill;

    // Start is called before the first frame update
    void Start()
    {
        GainSkillDialog = GameObject.Find("/Canvas").transform.Find("GainSkillDialog");
        GainSkillDialog.Find("Background/Button/Accept").GetComponent<Button>().onClick.AddListener(CloseDialog);
        dialogIcon = GainSkillDialog.Find("Background/Skill/Mask/Icon").GetComponent<Image>();
        skillOnWeapon = null;
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<Player>();
        anima = player.GetComponent<Animator>();
        weapon = player.transform.Find("root/pelvis/Weapon");
        infoMgr = FindObjectOfType<PlayerInfoMgr>();
        flgShowSkill = false;

        foreach (Skill s in skills)
        {
            s.cost = s.duration * 0.05f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkillEnable();

        if (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Alpha1))
        {
            UseSkill("Fire");
        }
        if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Alpha2))
        {
            UseSkill("Heal");
        }
        if (Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Alpha3))
        {
            UseSkill("SpeedUp");
        }
        if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.Alpha4))
        {
            UseSkill("SlowDown");
        }
        if (Input.GetKey(KeyCode.Keypad5) || Input.GetKey(KeyCode.Alpha5))
        {
            UseSkill("Fly");
        }
    }

    public void EnableSkill(Skill s)
    {
        Button skillBtn = skillUI.transform.Find("Skill_" + s.name + "/Border").GetComponent<Button>();
        Image skillIcon = skillUI.transform.Find("Skill_" + s.name + "/Mask/Icon").GetComponent<Image>();

        if (!skillBtn.interactable && s.enableLevel == infoMgr.GetLevel())
        {
            FindObjectOfType<AudioMgr>().Play("Congratulation");
            GainSkillDialog.gameObject.SetActive(true);
            dialogIcon.sprite = skillIcon.sprite;
            dialogIcon.color = skillIcon.color;
            Time.timeScale = 0f;
        }

        skillBtn.interactable = true;
        s.enable = true;
    }

    public void CloseDialog()
    {
        GainSkillDialog.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DisableSkill(Skill s)
    {
        Button skillBtn = skillUI.transform.Find("Skill_" + s.name).Find("Border").GetComponent<Button>();
        skillBtn.interactable = false;
        s.enable = false;
    }

    public void UpdateSkillEnable()
    {
        foreach (Skill s in skills)
        {
            if (infoMgr.CheckEnableToUseSkill(s.cost) && s.enableLevel <= infoMgr.GetLevel())
            {
                EnableSkill(s);
            }
            else
            {
                DisableSkill(s);
            }
        }
    }

    public void StopAllSkills()
    {
        foreach (Skill s in skills)
        {
            if (s.effect != null)
                StopCoroutine(s.effect);
            if (s.flgCooldown)
                s.IconImage.fillAmount = 0f;
        }

        infoMgr.SetAutoHealFactor(1f);
        playerScript.setWalkSpeedFactor(1f);
        playerScript.SetFlying(false);
        playerScript.SetFloating(false);
    }

    void UseSkill(string name)
    {
        Skill s = Array.Find(skills, skill => skill.name == name);

        if (s == null || s.enableLevel > infoMgr.GetLevel())
        {
            return;
        }
        if (flgShowSkill || s.flgCooldown)
        {
            return;
        }
        if (!infoMgr.ModifyMp(-s.cost))
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
        if (s.name != "Fire")
            FindObjectOfType<AudioMgr>().Play("UseMagic", 0.8f);
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
        if (s.name == "Heal")
        {
            s.effect = StartCoroutine(Heal(s, delay));
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
        float timer = 0f;
        while (s.IconImage.fillAmount > 0f)
        {
            timer += Time.deltaTime;
            s.IconImage.fillAmount = Mathf.Lerp(1f, 0f, timer / s.cooldown);
            yield return null;
        }

        s.IconImage.fillAmount = 1f;
        s.flgCooldown = false;
        yield break;
    }

    IEnumerator Fire(Skill s, float delay)
    {
        yield break;
    }

    IEnumerator Heal(Skill s, float delay)
    {
        FindObjectOfType<AudioMgr>().Play("Heal", s.ps.lifeTime);
        yield return new WaitForSeconds(delay);
        infoMgr.SetAutoHealFactor(5f);
        yield return new WaitForSeconds(s.duration);
        infoMgr.SetAutoHealFactor(1f);
        yield break;
    }

    IEnumerator SpeedUp(Skill s, float delay)
    {
        FindObjectOfType<AudioMgr>().Play("SpeedUp", s.ps.lifeTime);
        yield return new WaitForSeconds(delay);
        playerScript.setWalkSpeedFactor(3f);
        yield return new WaitForSeconds(s.duration);
        playerScript.setWalkSpeedFactor(1f);
        yield break;
    }

    IEnumerator SlowDown(Skill s, float delay)
    {
        FindObjectOfType<AudioMgr>().Play("SlowDown", s.duration);
        yield break;
    }

    IEnumerator Fly(Skill s, float delay)
    {
        FindObjectOfType<AudioMgr>().Play("Fly", s.ps.lifeTime);
        yield return new WaitForSeconds(delay);
        playerScript.SetFlying(true, 2f, 0.2f);
        // playerScript.SetFloating(true, 1f);
        yield return new WaitForSeconds(s.duration);
        playerScript.SetFloating(false, 0f);
        FindObjectOfType<AudioMgr>().Play("Falling", 1f);
        yield break;
    }
}
