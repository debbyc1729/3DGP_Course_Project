using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterManager : MonoBehaviour
{
    public int nowLevel = 1;
    public int MixMonsterNum = 4;
    public GameObject[] LevelOneMonsterObjects;
    public GameObject[] LevelTwoMonsterObjects;
    public GameObject[] BossObjects;
    public Transform target;
    public float SpawnRadio = 5.0f;

    public GameObject HealthBar;
    public Slider slider;
    public TextMeshProUGUI HPnumber;

    Vector3[] SpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint = new Vector3[MixMonsterNum];

        for (int i=0;i< MixMonsterNum;i++) {
            SpawnPoint[i] = transform.GetChild(i).position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount < MixMonsterNum + 1 && nowLevel == 1) {
            //距離玩家太近不要生成
            int index = Random.Range(0, MixMonsterNum);
            Debug.Log("index= "+ index);
            if (Vector3.Distance(target.position, SpawnPoint[index % MixMonsterNum]) < SpawnRadio) {
                index++;
                Debug.Log("new index= " + index);
            }
            int NumMonsterObjects = LevelOneMonsterObjects.Length;
            GameObject newMonster = Instantiate(LevelOneMonsterObjects[Random.Range(0, NumMonsterObjects)], SpawnPoint[index % MixMonsterNum], Quaternion.identity);
            newMonster.transform.SetParent(transform);
        }
        if(nowLevel == 2)
        {
            genLevelTwoMonsters();
        }
        if (nowLevel == 3)
        {
            genBoss();
        }

    }

    public void genLevelTwoMonsters()
    {

    }
    public void genBoss()
    {

    }
}
