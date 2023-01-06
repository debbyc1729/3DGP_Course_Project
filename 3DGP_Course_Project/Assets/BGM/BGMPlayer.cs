using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public int level = 1;
    public float maxVolume = 0.5f;
    public float minVolume = 0;
    //public Sound BGM;
    int levelTemp = 0;
    bool flg = true;
    private float fadeSpeed = 2.0f;
    private float deltaTime;
    bool fadeOutStarting = false;
    bool fadeInStarting = false;
    // Start is called before the first frame update
    void Start()
    {
        //transform.GetComponent<AudioSource>().volume = 0.5f;
        //transform.GetComponent<AudioSource>().Stop();
        deltaTime = Time.deltaTime;
        transform.GetComponent<AudioSource>().volume = 0;
        transform.GetComponent<AudioSource>().Play();
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

        if (fadeOutStarting)
        //if(Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("FadeOutMusic");
            FadeOutMusic();
        }
        if (fadeInStarting)
        {
            //Debug.Log("FadeInMusic");
            FadeInMusic();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (flg && other.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerStay Player transform.name= " + transform.name);
            BGMPlay();
            /*if (transform.name == "Maze_01")
            {
                FindObjectOfType<BGMManager>().Play("1");
            }*/
            if (transform.name == "MineMaze")
            {
                //FindObjectOfType<BGMManager>().Play("2");
                FindObjectOfType<MonsterManager>().genLevelTwoMonsters();
                FindObjectOfType<PortalController>().SetRespawnPortalID(1);
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
    public void BGMPlay()
    {
        fadeInStarting = true;
        fadeOutStarting = false;
    }

    public void BGMStop()
    {
        fadeOutStarting = true;
        fadeInStarting = false;
    }

    private void FadeOutMusic()
    {
        float volume = transform.GetComponent<AudioSource>().volume;
        transform.GetComponent<AudioSource>().volume = Mathf.Lerp(volume, minVolume, fadeSpeed * deltaTime);

        if(volume < maxVolume * 0.1f)
        {
            //Debug.Log("volume < maxVolume * 0.1f");
            transform.GetComponent<AudioSource>().volume = 0;
            transform.GetComponent<AudioSource>().Stop();
            fadeOutStarting = false;
        }
    }
    private void FadeInMusic()
    {
        float volume = transform.GetComponent<AudioSource>().volume;
        transform.GetComponent<AudioSource>().volume = Mathf.Lerp(volume, maxVolume, fadeSpeed * deltaTime);

        if (volume > maxVolume * 0.9f)
        {
            //Debug.Log("volume + 0.001f == maxVolume");
            transform.GetComponent<AudioSource>().volume = maxVolume;
            fadeInStarting = false;
        }
    }
    
}
