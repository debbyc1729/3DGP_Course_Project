using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuffMgr : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    public Buff[] buffList;
    float xStart;
    float xStep;
    int buffCount;

    // Start is called before the first frame update
    void Awake()
    {
        xStart = 10f;
        xStep = 3f;
        buffCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InvokeBuff(string name, float duration)
    {
        Buff b = Array.Find(buffList, buff => buff.name == name);

        if (b == null)
        {
            return;
        }

        switch(b.type)
        {
            case Buff.Type.Single:
                if (b.objList.Length == 0)
                {
                    AddBuff(b, duration);
                }
                else
                {
                    RestartBuff(b, b.objList[0]);
                }
                break;
            case Buff.Type.Multiple:
                AddBuff(b, duration);
                break;
        }
    }

    void AddBuff(Buff b, float duration)
    {
        BuffObj obj = new BuffObj();
        obj.id = Guid.NewGuid();
        obj.prefab = Instantiate(prefab, transform);
        obj.index = buffCount;
        obj.duration = duration;

        SetAppearance(b, obj);
        SetPosition(obj);
        obj.countdown = StartCoroutine(CountDownCoroutine(b, obj));

        b.AddObj(obj);
        buffCount += 1;
    }

    void RestartBuff(Buff b, BuffObj obj)
    {
        StopCoroutine(obj.countdown);
        obj.countdown = StartCoroutine(CountDownCoroutine(b, obj));
    }

    void SetAppearance(Buff b, BuffObj obj)
    {
        Image bar = obj.prefab.GetComponent<Image>();
        Image image = obj.prefab.transform.Find("Mask").Find("Image").GetComponent<Image>();

        bar.color = b.barColor;
        image.sprite = b.sourceImage;
        image.color = b.imageColor;
    }

    void SetPosition(BuffObj obj)
    {
        RectTransform rtBuff = obj.prefab.GetComponent<RectTransform>();
        float x = xStart + obj.index * (rtBuff.rect.width + xStep);
        rtBuff.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, rtBuff.rect.width);
        rtBuff.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rtBuff.rect.height);
    }

    IEnumerator CountDownCoroutine(Buff b, BuffObj obj)
    {
        Image bar = obj.prefab.GetComponent<Image>();
        Image barBorder = obj.prefab.transform.Find("Border").GetComponent<Image>();
        float timer = 0f;

        while (bar.fillAmount > 0f)
        {
            timer += Time.deltaTime;
            bar.fillAmount = Mathf.Lerp(1f, 0f, timer / obj.duration);
            barBorder.fillAmount = bar.fillAmount;
            yield return null;
        }

        RemoveBuff(b, obj);
        yield break;
    }

    void RemoveBuff(Buff b, BuffObj obj)
    {
        buffCount -= 1;
        int removeIndex = b.IndexOf(obj);
        Destroy(obj.prefab, 0f);
        b.RemoveObj(obj);

        for (int i = 0; i < buffList.Length; i++)
        {
            for (int j = 0; j < buffList[i].objList.Length; j++)
            {
                if (buffList[i].objList[j].index > removeIndex)
                {
                    buffList[i].objList[j].index -= 1;
                    SetPosition(buffList[i].objList[j]);
                }
            }
        }
    }
}
