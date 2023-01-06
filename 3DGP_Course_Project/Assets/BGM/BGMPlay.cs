using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour
{
    public int level = 1;
    //public Sound BGM;
    int levelTemp = 0;
    bool flg = true;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<AudioSource>().volume = 0.5f;
        transform.GetComponent<AudioSource>().Stop();
        //levelTemp = level;
        /*BGM.source = gameObject.AddComponent<AudioSource>();
        BGM.source.clip = BGM.clip;
        BGM.source.volume = BGM.volume;
        BGM.source.pitch = BGM.pitch;
        BGM.source.loop = BGM.loop;*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (levelTemp != level)
        {
            Debug.Log("BGMManager play= " + level);
            FindObjectOfType<BGMManager>().Play(level.ToString());
            levelTemp = level;
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (flg && other.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerStay Player transform.name= " + transform.name);
            //BGM.source.Play();
            transform.GetComponent<AudioSource>().Play();
            /*if (transform.name == "Maze_01")
            {
                FindObjectOfType<BGMManager>().Play("1");
            }*/
            if (transform.name == "MineMaze")
            {
                //FindObjectOfType<BGMManager>().Play("2");
                FindObjectOfType<MonsterManager>().genLevelTwoMonsters();
            }
            if (transform.name == "BossRoom")
            {
                //FindObjectOfType<BGMManager>().Play("3");
                FindObjectOfType<MonsterManager>().genBoss();
            }
            flg = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerExit Player");
            //BGM.source.Stop();
            BGMStop();
            flg = true;
        }
    }

    public void BGMStop()
    {
        transform.GetComponent<AudioSource>().Stop();
    }
}
