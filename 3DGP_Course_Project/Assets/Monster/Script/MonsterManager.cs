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


    public Transform Level2SpawnPoint;
    Vector3[] Level2SP;

    public GameObject HealthBar;
    public Slider slider;
    public TextMeshProUGUI HPnumber;
    public Transform BossSpawnPoint;
    public GameObject EndMountain;
    public GameObject BossRoom;
    public bool isBossDie = false;

    Vector3[] SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint = new Vector3[MixMonsterNum];

        for (int i = 0; i < MixMonsterNum; i++)
        {
            SpawnPoint[i] = transform.GetChild(0).GetChild(i).position;
        }

        /*Level2SP = new Vector3[Level2SpawnPoint.childCount];
        for (int i = 0; i < Level2SpawnPoint.childCount; i++)
        {
            Level2SP[i] = Level2SpawnPoint.GetChild(i).position;
        }*/
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetChild(0).childCount < MixMonsterNum && nowLevel == 1)
        {
            //距離玩家太近不要生成
            int index = Random.Range(0, MixMonsterNum);
            Debug.Log("index= " + index);
            if (Vector3.Distance(target.position, SpawnPoint[index % MixMonsterNum]) < SpawnRadio)
            {
                index++;
                Debug.Log("new index= " + index);
            }
            int NumMonsterObjects = LevelOneMonsterObjects.Length;
            GameObject newMonster = Instantiate(LevelOneMonsterObjects[Random.Range(0, NumMonsterObjects)], SpawnPoint[index % MixMonsterNum], Quaternion.identity);
            newMonster.transform.SetParent(transform.GetChild(0));
        }
        if (nowLevel == 2)
        {
            genLevelTwoMonsters();
        }
        if (nowLevel == 3)
        {
            genBoss();
        }
        if (isBossDie)
        {
            Debug.Log("isBossDie");
            BossRoom.GetComponent<BGMPlayer>().BGMStop();
            EndMountain.SetActive(false);
        }
    }

    public void genLevelTwoMonsters()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        /*for (int i = 0; i < Level2SpawnPoint.childCount; i++)
        {
            GameObject Lv2Monster = Instantiate(LevelTwoMonsterObjects[i % 3], Level2SP[i], Quaternion.identity);
            Lv2Monster.transform.SetParent(transform.GetChild(1));
        }*/
    }
    public void genBoss()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);

        GameObject Boss = transform.GetChild(2).GetChild(0).gameObject;
        Boss.GetComponent<Boss>().target = target;
        Boss.GetComponent<Boss>().HealthBar = HealthBar;
        Boss.GetComponent<Boss>().slider = slider;
        Boss.GetComponent<Boss>().HPnumber = HPnumber;
        Boss.GetComponent<Boss>().EndMountain = EndMountain;
        /*if (transform.GetChild(2).childCount == 0)
        {
            GameObject Boss = Instantiate(BossObjects[0], BossSpawnPoint.position, Quaternion.identity);
            Boss.transform.SetParent(transform.GetChild(2));

            Boss.GetComponent<Boss>().target = target;
            Boss.GetComponent<Boss>().HealthBar = HealthBar;
            Boss.GetComponent<Boss>().slider = slider;
            Boss.GetComponent<Boss>().HPnumber = HPnumber;
            Boss.GetComponent<Boss>().EndMountain = EndMountain;
        }*/
    }
}
