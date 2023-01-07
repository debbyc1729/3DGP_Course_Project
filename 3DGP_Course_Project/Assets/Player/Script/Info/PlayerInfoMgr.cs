using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoMgr : MonoBehaviour
{
    Transform HealthBar;
    Transform MagicBar;
    Transform LevelBar;
    Text levelText;
    Image levelAmount;
    Transform FullScreen;
    Transform DieMenu;
    Transform NextLevelDialog;
    float Hp;
    float Mp;
    float healSpeed;
    float levelAmountTemp;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = transform.Find("HealthBar/Bar");
        Debug.Log(HealthBar);
        MagicBar  = transform.Find("MagicBar/Bar");
        LevelBar  = transform.Find("LevelBar");
        levelText = LevelBar.Find("Number").GetComponent<Text>();
        levelAmount = LevelBar.Find("Bar").GetComponent<Image>();
        FullScreen  = transform.Find("FullScreen");
        DieMenu = GameObject.Find("/Canvas").transform.Find("DieMenu");
        NextLevelDialog = GameObject.Find("/Canvas").transform.Find("NextLevelDialog");
        NextLevelDialog.Find("Background/Button/Okay").GetComponent<Button>().onClick.AddListener(CloseDialog);

        Hp = 1f;
        Mp = 1f;
        SetAutoHealFactor(1f);
        levelText.text = "1";
        levelAmount.fillAmount = 0f;
        levelAmountTemp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ModifyHp(-0.1f);
        if (Input.GetKeyDown(KeyCode.O))
            ModifyHp(0.1f);
        if (Input.GetKeyDown(KeyCode.K))
            ModifyMp(-0.1f);
        if (Input.GetKeyDown(KeyCode.L))
            ModifyMp(0.1f);
        if (Input.GetKeyDown(KeyCode.P))
            ModifyLevel(5.3f);

        // FullScreen.GetComponent<Animator>().SetBool("hurt", false);

        UpdateHp();
        UpdateMp();
        AutoHeal();
    }

    void UpdateHp()
    {
        HealthBar.GetComponent<Image>().fillAmount = Hp;
    }

    void UpdateMp()
    {
        MagicBar.GetComponent<Image>().fillAmount = Mp;
    }

    void AutoHeal()
    {
        ModifyHp(healSpeed * Time.deltaTime);
        ModifyMp(healSpeed * 2f * Time.deltaTime);
    }

    public void SetAutoHealFactor(float factor)
    {
        healSpeed = 0.02f * factor;
    }

    public float GetHp()
    {
        return Hp;
    }

    public float GetMp()
    {
        return Mp;
    }

    public int GetLevel()
    {
        return int.Parse(levelText.text);
    }

    public void ModifyHp(float value)
    {
        Hp += value;

        if (Hp > 1f) Hp = 1f;
        if (Hp < 0f) Hp = 0f;

        if (value < 0f)
        {
            StartCoroutine(GetHurt());
        }
        if (Hp == 0f)
        {
            DieMenu.gameObject.SetActive(true);
            Hp = 1f;
            Mp = 1f;
            FindObjectOfType<PortalController>().SetPosition(GameObject.Find("/Player"));
        }
    }

    public bool ModifyMp(float value)
    {
        Mp += value;

        if (Mp > 1f) Mp = 1f;
        if (Mp < 0f)
        {
            StartCoroutine(LowMagic());
            Mp -= value;
            return false;
        }

        return true;
    }

    public bool CheckEnableToUseSkill(float mpCost)
    {
        return Mp - mpCost >= 0f;
    }

    public void ModifyLevel(float value)
    {
        if (value < 0f)
        {
            return;
        }

        levelAmountTemp += value;
        int level = int.Parse(levelText.text);

        StartCoroutine(GainExperience(level));
    }

    IEnumerator GainExperience(int level)
    {
        while (levelAmountTemp >= 1f)
        {
            levelAmountTemp -= 1f;
            level += 1;
            yield return StartCoroutine(ExpAnimation(1f, level));
        }
        if (levelAmountTemp > 0f)
        {
            yield return StartCoroutine(ExpAnimation(levelAmountTemp, level));
        }
        yield break;
    }

    IEnumerator ExpAnimation(float targetAmount, int targetLevel)
    {
        float originAmount = levelAmount.fillAmount;
        float duration = (targetAmount - originAmount) * 0.4f;
        float timer = 0f;

        while (levelAmount.fillAmount < targetAmount)
        {
            timer += Time.deltaTime;
            levelAmount.fillAmount = Mathf.Lerp(originAmount, targetAmount, timer / duration);
            yield return null;
        }

        if (targetAmount >= 1f)
        {
            levelAmount.fillAmount = 0f;
        }
        if (targetLevel == 15 && int.Parse(levelText.text) != targetLevel)
        {
            FindObjectOfType<PortalController>().ShowPortals();
            FindObjectOfType<AudioMgr>().Play("Congratulation");
            NextLevelDialog.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        levelText.text = targetLevel.ToString();

        yield break;
    }

    public void CloseDialog()
    {
        NextLevelDialog.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator GetHurt()
    {
        FullScreen.GetComponent<Animator>().SetBool("hurt", true);
        int option = Random.Range(1, 4);
        FindObjectOfType<AudioMgr>().Play("Hurt" + option.ToString(), 0.5f);
        yield return new WaitForSeconds(0.05f);
        FullScreen.GetComponent<Animator>().SetBool("hurt", false);
        yield break;
    }

    IEnumerator LowMagic()
    {
        Debug.Log("LowMagic");
        FullScreen.GetComponent<Animator>().SetBool("deplete", true);
        FindObjectOfType<AudioMgr>().Play("LowMagic" , 0.5f);
        yield return new WaitForSeconds(0.05f);
        FullScreen.GetComponent<Animator>().SetBool("deplete", false);
        yield break;
    }
}
