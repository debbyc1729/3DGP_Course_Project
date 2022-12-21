using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int MixMonsterNum = 4;
    public GameObject[] MonsterObjects;
    public Transform target;
    public float SpawnRadio = 5.0f;

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
        if (transform.childCount < MixMonsterNum + 1) {
            //距離玩家太近不要生成
            int index = Random.Range(0, MixMonsterNum);
            Debug.Log("index= "+ index);
            if (Vector3.Distance(target.position, SpawnPoint[index % MixMonsterNum]) < SpawnRadio) {
                index++;
                Debug.Log("new index= " + index);
            }
            int NumMonsterObjects = MonsterObjects.Length;
            GameObject newMonster = Instantiate(MonsterObjects[Random.Range(0, NumMonsterObjects)], SpawnPoint[index % MixMonsterNum], Quaternion.identity);
            newMonster.transform.SetParent(transform);
        }
    }
}
