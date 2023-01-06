using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoMgr : MonoBehaviour
{
    Transform HealthBar;
    Transform MagicBar;
    Transform LevelBar;
    int MaxHealthAmount;
    int MaxMagicAmount;
    float Hp;
    float Mp;
    Text levelText;
    Image levelAmount;
    float levelAmountTemp;
    Transform FullScreen;
    Transform DieMenu;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = transform.Find("HealthBar");
        MagicBar  = transform.Find("MagicBar");
        LevelBar  = transform.Find("LevelBar");
        MaxHealthAmount = HealthBar.childCount;
        MaxMagicAmount  = MagicBar.childCount;
        Hp = 1f;
        Mp = 1f;
        levelText = LevelBar.Find("Number").GetComponent<Text>();
        levelText.text = "1";
        levelAmount = LevelBar.GetComponent<Image>();
        levelAmount.fillAmount = 0f;
        levelAmountTemp = 0f;
        FullScreen  = transform.Find("FullScreen");
        DieMenu = GameObject.Find("/Canvas").transform.Find("DieMenu");
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
        for(int i = 0; i < MaxHealthAmount; i++)
        {
            if (Hp * MaxHealthAmount > i)
            {
                HealthBar.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HealthBar.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateMp()
    {
        for(int i = 0; i < MaxMagicAmount; i++)
        {
            if (Mp * MaxMagicAmount > i)
            {
                MagicBar.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                MagicBar.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void AutoHeal()
    {
        ModifyHp(0.01f * Time.deltaTime);
        ModifyMp(0.03f * Time.deltaTime);
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
        }
        levelText.text = targetLevel.ToString();

        yield break;
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
